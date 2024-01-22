
using UnityEngine;

public class ActiveGun : MonoBehaviour
{
    public Transform crossHairTarget;
    public Transform weaponParent;
    [SerializeField] private RaycastWeapon weapon; //? gun tren nguoi player
    [SerializeField] private Animator rigAnimator;

    private void Awake() {
        // rigAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        // rigAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        // rigAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        // rigAnimator.updateMode = AnimatorUpdateMode.Normal;
    }
    private void Start() {

        //?kiem tra co san vu khi hay khong
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if(existingWeapon != null)
        {
            Equip(existingWeapon);
        }
    }

    private void Update() {
        if(weapon) {
            if(Input.GetButtonDown("Fire1")) {
                weapon.StartFiring();
            } else if(Input.GetButtonUp("Fire1")) {
                weapon.StopFiring();
            }

            //? chuyen doi trang thai equip and holster
            if(Input.GetKeyDown(KeyCode.X)) {
                bool isHolster = rigAnimator.GetBool("holster_weapon");
                rigAnimator.SetBool("holster_weapon", !isHolster);
            }
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

        rigAnimator.Play("equip_" + weapon.weaponName);
    }

}
