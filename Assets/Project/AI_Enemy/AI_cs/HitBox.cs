using UnityEngine;

public class HitBox : MonoBehaviour
{
    public AiHealth aIHealth;
    public void  OnRaycastHit(RaycastWeapon raycastWeapon, Vector3 direction) {
        aIHealth.TakeDamage(raycastWeapon.Damage, direction);
    } 
}
