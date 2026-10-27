using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    public int Score {get; private set; }

    public UnityEvent OnDead;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = startingHealth;
        Score = 0;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }

        Health -= damage;
        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }
    }

    public virtual void Die()
    {
        IsDead = true;
        OnDead.Invoke();
    }

    public virtual void AddHealth(int add)
    {
        Health = Mathf.Clamp(Health + add, 0, startingHealth);
    }

    public virtual void AddScore(int score)
    {
        
        Score += score;
        
    }

}
