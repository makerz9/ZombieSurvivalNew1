using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,

        Attack,
        Die
    }
    

    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 10f;

    public ParticleSystem bloodEffect;
    public Collider zombieCollider;
    public Renderer zombieRenderer;//좀비마다 색 바꿔줄때 쓸려고

    public AudioClip hitClip;
    public AudioClip deathClip;

    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;

    public float damage = 10;
    private float lastAttackTime;

    private Status currentStatus;
    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
        public static readonly int IdDie = Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus; //그냥 리턴
        set
        {
            currentStatus = value;
            Debug.Log(currentStatus);

            //스위치문
            switch (currentStatus)
            {
                case Status.Idle:
                    zombieAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    zombieAnimator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;


                case Status.Attack:
                    zombieAnimator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    agent.isStopped = true;
                    zombieAnimator.SetTrigger("IdDie");
                    zombieCollider.enabled = false;
                    zombieAudioSource.PlayOneShot(deathClip);
                    break;
                    //열거형 스위치 사용가능

            }
        }

        
        
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
    }


    //초기화 부분
    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;
        agent.ResetPath();//네비메시 에이전트가 이동할때 경로를 계산하는데 그 경로를 초기화



        if(NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        zombieCollider.enabled = true;
        CurrentStatus = Status.Idle;

    }
    

    void Update()
    {
        switch (currentStatus)
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
                //열거영 스위치 사용가능

        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnDamage(10f, Vector3.zero, Vector3.zero);
        }
    }

    private void UpdateDie()
    {
        throw new NotImplementedException();
    }

    private void UpdateAttack()
    {
        throw new NotImplementedException();
    }

    private void UpdateTrace()
    {
        if(target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        //호출할때마다 새 경로가 만들어짐
        agent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        
        if(target != null && Vector3.Distance(target.position, transform.position) < traceDistance)
        {
            //타겟이 있고, 타겟과의 거리가 traceDistance보다 작으면 추적상태로 바꿔라
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
    }

    private Transform FindTarget(float radius)
    {
        //필요한 중점, 반경, 레이어
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        //이 레이어에 들어온 특정 오브젝트를 모두찾아 배열에 넣음

        Transform closet = null;
        float closetDistance = float.MaxValue;

        for(int i = 0; i < colliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);

            if(distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
                
            }
        }


        return closet;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(IsDead)
        {
            
            
            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        zombieAudioSource.PlayOneShot(hitClip);
        bloodEffect.transform.position = hitPoint;
        bloodEffect.transform.forward = hitNormal;
        bloodEffect.Play();
    }

    public override void Die()
    {
        base.Die();

                zombieAnimator.SetTrigger(IdDie);
        zombieAudioSource.PlayOneShot(deathClip);



        CurrentStatus = Status.Die;
    }

}
