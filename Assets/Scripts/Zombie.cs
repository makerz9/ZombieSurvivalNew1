
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }
    
    public Transform target;
    public LayerMask targetLayer;

    public Image uiZombieHealth;
    public float traceDistance = 8f;
    public float attackDistance = 1f; //이 시도는 맞았음

    public ParticleSystem bloodEffect;
    public Collider zombieCollider;
    public Renderer zombieRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;

    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;

    public GameObject zombieHitBox;

    public float damage = 10;
    public float attackInterval = 1f;
    private float lastAttackTime;

    private Status currentStatus;

    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDie = Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;
            Debug.Log(currentStatus);
            switch (currentStatus)
            {
                case Status.Idle:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    zombieAnimator.SetBool(IdHasTarget, true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;

                    break;
                case Status.Die:
                    agent.isStopped = true;
                    zombieAnimator.SetTrigger(IdDie);
                    zombieCollider.enabled = false;
                    zombieAudioSource.PlayOneShot(deathClip);
                    //deactiveTime = Time.time + 5f;
                    break;
            }
        }
    }

    //매개변수로 스크립터블데이터를 받아서 세팅하는 함수
    public void Setup(ZombieData data)
    {
        startingHealth = data.maxHP;
        damage = data.damage;

        //이동은 네비게이션이담당
        //1이다 = 1초당 유니티단위로 1거리만큼이동
        agent.speed = data.speed;
        zombieRenderer.material.color = data.skinColor;
        
        OnEnable();
        //Debug.Log("셋업");
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.enabled = true;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }




        zombieCollider.enabled = true;

        CurrentStatus = Status.Idle;

        uiZombieHealth.gameObject.SetActive(true);
        uiZombieHealth.fillAmount = Health / startingHealth;

    }

    private void OnDisable()
    {
        if(agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }


    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }



    private void UpdateDie()
    {

    }

    private void UpdateAttack()
    {

        //이 시도도 맞았음
        if (target == null || Vector3.Distance(transform.position, target.position) > attackDistance)
        {

            CurrentStatus = Status.Trace;
            return;
        }

        //!@# 공격시간 계산
        if(Time.time >= lastAttackTime + attackInterval)
        {
            lastAttackTime = Time.time;

            IDamageable damageable = target.GetComponent<IDamageable>();
            var hitNormal = (transform.position - target.position).normalized;
            damageable.OnDamage(damage, target.position, hitNormal);


        }

    }

    private void UpdateTrace()
    {

        //이 시도도 맞았음
        //우선순위따라 위로 올리고 return 해야 상태 겹치는걸 방지
        if (target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            //target = null;
            CurrentStatus = Status.Attack;
            return;
        }

        if (target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }


        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }




        target = FindTarget(traceDistance);
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closet = null;
        float closetDistacne = float.MaxValue;

        for (int i = 0; i < colliders.Length; ++i)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closetDistacne)
            {
                closetDistacne = distance;
                closet = colliders[i].transform;
            }
        }

        return closet;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        uiZombieHealth.fillAmount = Health / startingHealth;
        zombieAudioSource.PlayOneShot(hitClip);
        bloodEffect.transform.position = hitPoint;
        bloodEffect.transform.forward = hitNormal;
        bloodEffect.Play();
    }

    // IEnumerator OnAttack()
    // {
    //     zombieHitBox.SetActive(true);
    //     yield return new WaitForSeconds(0.5f);
    //     //hitbox를 활성화하고 0.5초 후에 공격을 처리

    //     // hitbox에 닿으면 그 충돌체에 IDamageable 인터페이스가 있는지 확인하고, 
    //     // 있다면 OnDamage를 호출하여 데미지를 입힘
        
    //     if (Time.time >= lastAttackTime + 1f)
    //     {
    //         lastAttackTime = Time.time;

    //         if (target != null)
    //         {
    //             IDamageable damageable = target.GetComponent<IDamageable>();
    //             if (damageable != null)
    //             {
    //                 damageable.OnDamage(damage, target.position, -target.forward);
                    
    //             }
    //         }
    //     }
    //     zombieHitBox.SetActive(false);
    //     StartCoroutine(OnAttack());
    // }

    public override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;
    }
}
