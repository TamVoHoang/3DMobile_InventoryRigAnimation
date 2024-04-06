
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //? giu gun prefab on player || Ai
    //? layer = pickup => chi trigger voi player and character Ai
    [SerializeField] private RaycastWeapon gunPrefab;

    private void OnTriggerEnter(Collider other) {
        int weaponSlotIndex = (int)gunPrefab.weaponSlot; //=0

        ActiveGun activeGun = other.GetComponent<ActiveGun>();
        if(activeGun)
        {
            RaycastWeapon newWeapon = Instantiate(gunPrefab, activeGun.weaponSlots[weaponSlotIndex].position,
                activeGun.weaponSlots[weaponSlotIndex].transform.rotation, activeGun.weaponSlots[weaponSlotIndex]);
                
            newWeapon.transform.SetParent(activeGun.weaponSlots[weaponSlotIndex], false);
            activeGun.Equip(newWeapon);

            Destroy(gameObject);
        }

        AiWeapons aiWeapons = other.gameObject.GetComponent<AiWeapons>();
        if(aiWeapons) {
            RaycastWeapon newWeapon = Instantiate(gunPrefab);
            aiWeapons.EquipWeapon(newWeapon);
            Destroy(gameObject);
        }
    }
}