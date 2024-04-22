using UnityEngine;

public class HitBox : MonoBehaviour
{
    //? gameobject = cac gameobject co rigidbody trong nguoi Ai
    //? hitbox.cs duoc add vao cac doi tuong co rb tai coll 36 AiHealth.cs Start()
    //? vien dan se detect hitbox.cs tai collum 188 raycasWeapn.cs RaycastSement()

    //? game object nao chua class ke thua Health.cs nay, 
    //? thi bien health tai day se la aihealth || playerHealth
    
    public Health health; 

    // private void Start() {
    //     aIHealth = GetComponentInParent<AiHealth>();
    // }
    
    public void  OnRaycastHit(RaycastWeapon raycastWeapon, Vector3 direction) {
        if(health.IsReadyToTakeDamage)
            health.TakeDamage(raycastWeapon.Damage, direction);
    }
}
