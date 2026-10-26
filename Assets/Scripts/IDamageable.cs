using UnityEngine;

public interface IDamageable
{
    //데미지 양 / 총알맞을떄 피격이펙트 위치 / 
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
