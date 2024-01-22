
using UnityEngine;

public class ActiveGun : MonoBehaviour
{
    public Transform crossHairTarget;
    public Transform weaponParent;
    [SerializeField] private RaycastWeapon weapon; //? gun tren nguoi player
    private void Start() {
        //?kiem tra co san vu khi hay khong
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if(existingWeapon != null)
        {
            Equip(existingWeapon);
        }
    }

    private void Update() {
        if(Input.GetButtonDown("Fire1"))
        {
            weapon.StartFiring();
        } else if(Input.GetButtonUp("Fire1"))
        {
            weapon.StopFiring();
        }
    }

    //TODO equip gun (touch pickup trigger or attactched gun)
    public void Equip(RaycastWeapon newWeapon) {
        if(weapon != null)
        {
            Destroy(weapon.gameObject);
            Debug.Log("Destroy old weapon");
        }
        
        weapon = newWeapon;
        weapon.SetRaycastDes(crossHairTarget);
        weapon.transform.SetParent(weaponParent);

    }

}
