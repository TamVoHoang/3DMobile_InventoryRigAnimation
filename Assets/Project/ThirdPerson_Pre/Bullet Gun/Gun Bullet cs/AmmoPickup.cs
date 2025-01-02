using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int clipAmount = 1;
    private void Start() {
        Destroy(this, 30f);
    }
    
    private void OnTriggerEnter(Collider other) {
        //? tang dan cho player khi va cham. co the dung de khi use bang dan trong kho do
        ActiveGun activeGun = other.GetComponent<ActiveGun>();      //co activeGun.cs tren player
        
        if(activeGun !=null) {
            var weapon = activeGun.GetActiveWeapon();                  // lay weapon cua player
            if(weapon) {
                activeGun.RefillAmmo(clipAmount);
                Destroy(this.gameObject, 0.2f);
            } 
        }

        //? tang dan cho ai agen
        AiWeapons aiWeapons = other.GetComponent<AiWeapons>();
        if(aiWeapons != null) {
            if (aiWeapons && aiWeapons.IsLowAmmo_AiWeapon()) {
                aiWeapons.RefillAmmo_AiWeapon(clipAmount);
                Destroy(this.gameObject, 0.2f);
            }
        }
        
    }
}
