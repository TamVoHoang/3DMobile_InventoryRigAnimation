using UnityEngine;

public class ISword : MonoBehaviour, IWeapon
{
    //todo game object = cay kiem kieu interface
    public ActiveWeapon.SwordSlots swordSlot;
    [SerializeField] private ItemScriptableObject weaponScriptableObject;

    //tao ra raycast
    [SerializeField] private float minSwordDisRaycast = 10f; // do dai tia raycast tao ra
    Ray ray;
    RaycastHit hit;
    public ItemScriptableObject GetWeaponInfo() {
        Debug.Log("GetWeaponInfo() ISword.cs called by ActiveWeapon.cs through IWeapon.cs");
        return weaponScriptableObject;
    }
    public void Attack() {
        Debug.Log("Attack() ISword.cs called by ActiveWeapon.cs through IWeapon.cs");
        //CheckSword(this.gameObject.transform, transform.position); // transfom cua kiem, vector3 cua kiem - this.gameobject;
    }

    //? tao ra raycast tren cay kiem de take damage enemy
    /* private void CheckSwordRaycast(Transform RHand, Vector3 aimDirection) {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Character"); //! player and enemy have same LayerMask

        if(Physics.Raycast(RHand.position, RHand.transform.TransformDirection(aimDirection), out hit, minSwordDisRaycast, layerMask)) {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.yellow);

            if(hit.collider.gameObject.CompareTag("Player")) return;

            var hitBox = hit.collider.GetComponent<HitBox>();
            var hitEnemy = hitBox.GetComponent<Health>();

            var damage = GetWeaponInfo().damage;
            if(hitBox && !hitEnemy.IsDead) {
                hitBox.OnSwordRaycastHit(damage, hitBox.transform.position); //ray.direction
            }
        } else {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.red);

        }
    } */
    


    //todo 
}
