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

    //tao ra raycast de tan cong
    private void CheckSword(Transform sword, Vector3 aimDirection) {
        ray.origin = sword.position;
        ray.direction = aimDirection;

        int layerMask = LayerMask.GetMask("Character"); //! player and enemy have same LayerMask

        if(Physics.Raycast(sword.position, sword.transform.TransformDirection(aimDirection), out hit, minSwordDisRaycast, layerMask)) {
            Debug.Log("co sinh ra tia raycast yellow");
            Debug.DrawRay(sword.position, sword.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.yellow);
            var enemyHit = hit.transform.GetComponent<AiHealth>();
            var hitEnemy = hit.collider.GetComponent<AiHealth>();

            if(enemyHit != null && hitEnemy != null) {
                enemyHit.TakeDamage(100, ray.direction);
            }
        } else {
            Debug.Log("co sinh ra tia raycast red");
            Debug.DrawRay(sword.position, sword.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.red);

        }
    }
    


    //todo 
}
