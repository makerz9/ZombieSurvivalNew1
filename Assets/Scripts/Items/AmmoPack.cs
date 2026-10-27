using UnityEngine;
using UnityEngine.Events;

public class AmmoPack : MonoBehaviour, IItem
{
    public int ammo = 50;


    public UnityEvent OnUse{get; set;} = new UnityEvent();
    public void Use(GameObject target)
    {
        Debug.Log("ㅅㅂ");
        PlayerShooter shooter = target.GetComponent<PlayerShooter>();
        if (shooter == null || shooter.gun == null)
        {
            return;
        }
        shooter.gun.AddAmmo(ammo);
        gameObject.SetActive(false);
    }

}
