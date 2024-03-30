using UnityEngine;

public class HitBox : MonoBehaviour
{
    public AIHealth aIHealth;
    public void  OnRaycastHit(RaycastWeapon raycastWeapon, Vector3 direction) {
        aIHealth.TakeDamage(raycastWeapon.Damage, direction);
    } 
}
