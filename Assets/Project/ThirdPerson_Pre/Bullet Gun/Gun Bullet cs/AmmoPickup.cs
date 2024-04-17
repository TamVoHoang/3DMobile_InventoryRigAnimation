using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int clipAmount = 2;

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
        if (aiWeapons && aiWeapons.IsLowAmmo()) {
            aiWeapons.RefillAmmo(clipAmount);
            Destroy(this.gameObject, 0.2f);
        }
    }
}
