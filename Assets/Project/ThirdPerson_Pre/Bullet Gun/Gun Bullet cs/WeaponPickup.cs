
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //? instantiate gun prefab on player
    [SerializeField] private RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other) {
        ActiveGun activeGun = other.GetComponent<ActiveGun>();
        int weaponSlotIndex = (int)weaponPrefab.weaponSlot; //=0

        if(activeGun)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab, activeGun.weaponSlots[weaponSlotIndex].position,
                activeGun.weaponSlots[weaponSlotIndex].transform.rotation, activeGun.weaponSlots[weaponSlotIndex]);
                
            newWeapon.transform.SetParent(activeGun.weaponSlots[weaponSlotIndex], false);

            activeGun.Equip(newWeapon);

            //Destroy(gameObject);
        }
    }
}