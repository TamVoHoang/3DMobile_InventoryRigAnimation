
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    [SerializeField] private Animator rigController;
    [SerializeField] private WeaponAnimationEvents animationEvents;
    [SerializeField] private Transform leftHand;
    [SerializeField] private AmmoWidget ammoWidget;
    private ActiveGun activeGun;
    private GameObject magHand;
    [SerializeField] private bool isReloading = false;
    public bool GetIsReloading {get {return isReloading;}}

    void Start() {
        isReloading = false; // ko co thay dan
        activeGun = GetComponent<ActiveGun>();
        
        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    void Update() {
        RaycastWeapon weapon = activeGun.GetActiveWeapon();
        
        if (weapon) {
            if ((Input.GetKeyDown(KeyCode.R) || weapon.ShouldReload()) && !activeGun.IsHolstered && !activeGun.IsChangingGun) {
                isReloading = true; // dang thay dan
                rigController.SetTrigger("reload_weapon");
            }

            if (weapon.IsFiring && ammoWidget) {
                ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount);
            } 
        }
        else {
            ammoWidget.Clear(0); // ko con sung trang bi tren nguoi player do da keo het ve inventory
        }
    }

    void OnAnimationEvent(string eventName) {
        Debug.Log(eventName);
        switch (eventName) {
            case "deatch_mag":
                deatchMag();
                break;
            case "drop_mag":
                dropMag();
                break;
            case "refill_mag":
                refillMag();
                break;
            case "attach_mag":
                attachMag();
                break;

            default:
                break;
        }
    }

    void deatchMag() {
        RaycastWeapon weapon= activeGun.GetActiveWeapon();
        magHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }
    void dropMag() {
        GameObject droppedMag = Instantiate(magHand, magHand.transform.position, magHand.transform.rotation);
        droppedMag.AddComponent<Rigidbody>();
        droppedMag.AddComponent<BoxCollider>();
        magHand.SetActive(false);
    }
    void refillMag() {
        magHand.SetActive(true);
    }
    void attachMag() {
        RaycastWeapon weapon = activeGun.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magHand);

        //weapon.ammoCount = weapon.clipSize;
        weapon.RefillAmmo();

        rigController.ResetTrigger("reload_weapon");
        ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount);
    }

    //todo
}
