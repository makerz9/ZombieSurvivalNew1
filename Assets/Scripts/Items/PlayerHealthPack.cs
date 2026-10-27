using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthPack : MonoBehaviour, IItem
{
    public int heal = 50;

    public UnityEvent OnUse{get; set;} = new UnityEvent();

    public void Use(GameObject target)
    {
        LivingEntity livingEntity = target.GetComponent<LivingEntity>();
        if(livingEntity != null)
        {
            livingEntity.AddHealth(heal);
        }
        gameObject.SetActive(false);

    }


}
