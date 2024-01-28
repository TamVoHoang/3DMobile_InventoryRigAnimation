using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveGun : Singleton<ActiveGun>
{
    public enum WeaponSlots
    {
        Primary = 0,
        Secondary = 1,
        //Tertiary = 2
    }

    public Transform[] weaponSlots;
    [SerializeField] RaycastWeapon[] equipped_weapons = new RaycastWeapon[2]; // ko destroy sung 1 khi pickup sung 2
    public int activeWeaponIndex =1;
    public bool isHolstered = false; // ko co dang de sung trong tui hoac tren lung
    public bool isReload = false; // ko co dang thay dan
    public Transform crossHairTarget;
    [SerializeField] private RaycastWeapon weapon; //? gun tren nguoi player
    [SerializeField] private Animator rigAnimator;

    protected override void Awake() {
        base.Awake();
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
    
    RaycastWeapon GetWeapon(int index)
    {
        if(index < 0 || index >= weaponSlots.Length)
        {
            return null;
        }
        return equipped_weapons[index];
    }
    public RaycastWeapon GetActiveWeapon() // deatchMag() ReloadWeapon goi . kiem tra loai sung thay dan
    {
        return GetWeapon(activeWeaponIndex);
    }

    private void Update() {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        
        var weapon = GetWeapon(activeWeaponIndex);
        if(weapon && !isHolstered) {
            if(Input.GetButtonDown("Fire1")) {
                weapon.StartFiring();
            } else if(Input.GetButtonUp("Fire1")) {
                weapon.StopFiring();
            }

            // //? chuyen doi trang thai equip and holster
            // if(Input.GetKeyDown(KeyCode.X)) {
            //     bool isHolster = rigAnimator.GetBool("holster_weapon");
            //     rigAnimator.SetBool("holster_weapon", !isHolster);
            // }
        }

        if(Input.GetKeyDown(KeyCode.X)) 
            ToggleActiveWeapon();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveWeapon(WeaponSlots.Primary);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveWeapon(WeaponSlots.Secondary);
    }

    //TODO equip gun (touch pickup trigger or attactched gun)
    public void Equip(RaycastWeapon newWeapon) {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex); // kiem tra sung vua cham [number]
        if(weapon)
        {
            Destroy(weapon.gameObject);
            Debug.Log("Destroy old weapon");
        }
        weapon = newWeapon;
        weapon.SetRaycastDes(crossHairTarget);
        //rigAnimator.Play("equip_" + weapon.weaponName);//

        equipped_weapons[weaponSlotIndex] = weapon;
        //activeWeaponIndex = weaponSlotIndex;//
        SetActiveWeapon(newWeapon.weaponSlot);

    }
    #region HOLSTER AND SWITCHING GUN
    public void ToggleActiveWeapon()
    {
        bool isHolstered = rigAnimator.GetBool("holster_weapon");
        //dao nguoc bien trong script va gan cho bien torng aniamtior
        if (isHolstered) StartCoroutine(ActivateWeapon(activeWeaponIndex));
        else StartCoroutine(HolsterWeapon(activeWeaponIndex));
    }
    void SetActiveWeapon(WeaponSlots weaponSlot)
    {
        int holsterIndex = activeWeaponIndex; // con so de kt xem co sung equip tai 0 1 2
        int activeIndex = (int)weaponSlot;
        // dieu kien de khi cham lai cay sung cung loai animation se ko thuc hien
        if (holsterIndex == activeIndex) holsterIndex = -1;
        StartCoroutine(SwitchWeapon(holsterIndex, activeIndex));
    }
    IEnumerator SwitchWeapon(int holsterIndex,int activeIndex) // ok
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
        activeWeaponIndex = activeIndex;
    }
    IEnumerator HolsterWeapon(int index)
    {
        isHolstered = true;
        var weapon = GetWeapon(index); // kiem tra xem cai o equiped_Weapon dang co hay ko de chuan bi thay, neu varWeapon co thi ko thuc hien animation cat sung
        if (weapon)
        {
            rigAnimator.SetBool("holster_weapon", true); // holster_weapon = cat sung equiped_weapons[] hien co neu co
        do
            {
                yield return new WaitForEndOfFrame();
            } while (rigAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index); // kiem tra cay sung moi vua pickup da co trong cai o equiped_weapons[] chua
        if (weapon)
        {
            rigAnimator.SetBool("holster_weapon", false); //neu ko xet false, sau khi o tren holster_weapon = true cat sung
                                                            //den day van true, cho du co animation equip sung loai nao xong thi cung se tro ve trang thai cat sung
                                                            // do bien SetBool("holster_weapon") trong riglayer sung chung ca loai sung
                                                            // XET FALSE de san sang thuc hien hanh dong equip khac
            rigAnimator.Play("equip_" + weapon.weaponName);
            // DO XET FALSE NEN SE GIU NGUYEN TRNAG THAI EQUIP
            
        do
            {
                yield return new WaitForEndOfFrame();
            } while (rigAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        isHolstered = false;
        }
    }
    #endregion HOLSTER AND SWITCHING GUN


    //todo
}
