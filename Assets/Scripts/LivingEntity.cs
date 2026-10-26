using UnityEngine;
using UnityEngine.Events; 

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health {get; private set;}
    public bool IsDead {get; private set;}

    public UnityEvent OnDead;//죽었을때 호출할 함수 등록

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = startingHealth;
        
    }


    //crtl + . = 인터페이스 구현
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(IsDead)
        {
            return;
        }

        Health -= damage;
        if(Health <= 0)
        {
            Health = 0;

            Die();
        }
    }

    public virtual void Die()
    {
        IsDead = true;
        OnDead.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
