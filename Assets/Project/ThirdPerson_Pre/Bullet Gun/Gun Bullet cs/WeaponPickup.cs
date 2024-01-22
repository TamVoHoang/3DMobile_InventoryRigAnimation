
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //? instantiate gun prefab on player

    [SerializeField] private RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other) {
        ActiveGun activeGun = other.GetComponent<ActiveGun>();
        if(activeGun)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab, activeGun.weaponParent.transform.position, 
                activeGun.weaponParent.transform.rotation, activeGun.weaponParent);

            activeGun.Equip(newWeapon);

            //Destroy(gameObject);
        }
    }
}
