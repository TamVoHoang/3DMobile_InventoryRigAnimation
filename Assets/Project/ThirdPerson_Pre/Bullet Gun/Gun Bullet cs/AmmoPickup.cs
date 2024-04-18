using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int clipAmount = 1;

    private void OnTriggerEnter(Collider other) {
        //tang dan cho player khi va cham. co the dung de khi use bang dan trong kho do
        /* ActiveGun activeGun = other.GetComponent<ActiveGun>();      //co activeGun.cs tren player
        var weapon = activeGun.GetActiveWeapon();                  // lay weapon cua player
        if(activeGun && weapon) {
            activeGun.RefillAmmo(clipAmount);
            Destroy(this.gameObject, 0.2f);
        } */

        // tang dan cho ai agen
        AiWeapons aiWeapons = other.GetComponent<AiWeapons>();
        if (aiWeapons && aiWeapons.IsLowAmmo_AiWeapon()) {
            aiWeapons.RefillAmmo_AiWeapon(clipAmount);
            Destroy(this.gameObject, 0.2f);
        }
    }
}
