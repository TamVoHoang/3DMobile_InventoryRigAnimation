
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    //? giu gun prefab on player || Ai
    //? layer = pickup => chi trigger voi player and character Ai
    [SerializeField] private RaycastWeapon gunPrefab;

    private void OnTriggerEnter(Collider other) {
        int weaponSlotIndex = (int)gunPrefab.weaponSlot; //=0

        //todo gun pickup trigger with ActiveGun.cs - gameobject = player
        //todo gun nay se dinh chat ko go ra khoi player
        /* ActiveGun activeGun = other.GetComponent<ActiveGun>();
        if(activeGun)
        {
            RaycastWeapon newWeapon = Instantiate(gunPrefab, activeGun.weaponSlots[weaponSlotIndex].position,
                activeGun.weaponSlots[weaponSlotIndex].transform.rotation, activeGun.weaponSlots[weaponSlotIndex]);
                
            newWeapon.transform.SetParent(activeGun.weaponSlots[weaponSlotIndex], false);
            activeGun.Equip(newWeapon);

            Destroy(gameObject);
        } */

        //todo gun pickup trigger with AiWeapons.cs - gameobject = ai gunner
        AiWeapons aiWeapons = other.gameObject.GetComponent<AiWeapons>();
        if(aiWeapons) {
            RaycastWeapon newWeapon = Instantiate(gunPrefab);
            aiWeapons.Equip(newWeapon);
            Destroy(gameObject);
        }
    }
}