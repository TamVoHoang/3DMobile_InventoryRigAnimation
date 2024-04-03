
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //? instantiate gun prefab on player
    [SerializeField] private RaycastWeapon gunPrefab;

    private void OnTriggerEnter(Collider other) {
        ActiveGun activeGun = other.GetComponent<ActiveGun>();
        int weaponSlotIndex = (int)gunPrefab.weaponSlot; //=0

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