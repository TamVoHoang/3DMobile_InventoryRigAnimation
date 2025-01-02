using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGun : Singleton<ActiveGun>
{
    //todo game object = player
    //todo 
    public enum WeaponSlots
    {
        Primary = 0,
        Secondary = 1,
    }

    [Header ("Gun")]
    public Transform[] weaponSlots; //? dung de bo chi so index vao de instatiate vu khi ra dung vi tri transform
    [SerializeField] RaycastWeapon[] equipped_weapons = new RaycastWeapon[2]; // ko destroy sung 1 khi pickup sung 2
    [SerializeField] private int activeWeaponIndex = 1; //? slot nao dang trang bi sung
    [SerializeField] private bool isHolstered = false; // false = dang equip
    [SerializeField] private bool isChangingGun = false; // dang qua trinh switch(active + holster) || Toggle(active + holster)
    [SerializeField] private float delayTime_ChangeGunCountine = 1f; 

    //? crossHairTarget tren mainCamera lam gamobject co cs (diem vacham cua raycast maincamera voi cac object khac)
    [SerializeField] private Transform crossHairTarget;
    [SerializeField] private Animator rigAnimator;
    [SerializeField] private AmmoWidget ammoWidget;
    public int GetActiveWeaponIndex { get { return activeWeaponIndex; } }
    public bool IsHolstered { get { return isHolstered; } }
    public bool IsChangingGun{get => isChangingGun;}
    //public bool isReload = false; // ko co dang thay dan

    protected override void Awake() {
        base.Awake();
        /* rigAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigAnimator.updateMode = AnimatorUpdateMode.Normal; */
    }

    private void Start() {
        isHolstered = true; //! dang ko trang bi sung (sung sung ko co tren tay player)
        crossHairTarget = GameObject.Find("CroosHairTarget").transform;
        //?kiem tra co san vu khi hay khong
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if(existingWeapon != null) {

            Equip(existingWeapon);
        }
    }
    
    RaycastWeapon GetWeapon(int index) {
        if(index < 0 || index >= weaponSlots.Length) return null;
        return equipped_weapons[index];
    }

    // deatchMag() ReloadWeapon goi . kiem tra loai sung thay dan
    public RaycastWeapon GetActiveWeapon() {
        return GetWeapon(activeWeaponIndex);
    }

    private void Update() {
        var weapon = GetWeapon(activeWeaponIndex);
        var canFire = !isHolstered && !isChangingGun;// ko dang cat sung + ko dang change sung + ko dang toggle
        
        if(SceneManager.GetActiveScene().name == "Testing_SpawnPlayer") return;
        if(weapon) {
            if(InputManager.Instance.IsAttackButton && !weapon.IsFiring & canFire) {
                weapon.StartFiring();

                // sound
                SoundManager.Instance.PlaySound(SoundType.PistolGun, 1);
            }
                
            if((!InputManager.Instance.IsAttackButton && weapon.IsFiring) || !canFire) {
                weapon.StopFiring();
            }

            weapon.UpdateWeapon(Time.deltaTime, crossHairTarget.position);
            
        }

        /* if(Input.GetKeyDown(KeyCode.X)) 
            ToggleActiveWeapon();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveWeapon(WeaponSlots.Primary);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveWeapon(WeaponSlots.Secondary); */
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
        ////weapon.SetRaycastDes(crossHairTarget);
        ////rigAnimator.Play("equip_" + weapon.weaponName);// ko lay

        equipped_weapons[weaponSlotIndex] = weapon;
        ////activeWeaponIndex = weaponSlotIndex;// ko lay
        SetActiveWeapon(newWeapon.weaponSlot);
        ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount);
    }
    #region HOLSTER AND SWITCHING GUN
    public void HolsterGunsBeforeDeath() => rigAnimator.SetBool("holster_weapon", true);
    public void ToggleActiveWeapon()
    {
        isChangingGun = true; // dang thay sung

        bool isHolstered = rigAnimator.GetBool("holster_weapon"); // = false
        //dao nguoc bien trong script va gan cho bien torng aniamtior
        if (isHolstered) StartCoroutine(ActivateWeapon(activeWeaponIndex));
        else StartCoroutine(HolsterWeapon(activeWeaponIndex));

        StartCoroutine(DelayTime_ChangeGunCountine(delayTime_ChangeGunCountine));
    }
    public void SetActiveWeapon(WeaponSlots weaponSlot)
    {
        isChangingGun = true; // dang thay sung

        int holsterIndex = activeWeaponIndex; // con so de kt xem co sung equip tai 0 1 2
        int activeIndex = (int)weaponSlot;
        // dieu kien de khi cham lai cay sung cung loai animation se ko thuc hien
        if (holsterIndex == activeIndex) holsterIndex = -1;
        StartCoroutine(SwitchWeapon(holsterIndex, activeIndex));

        StartCoroutine(DelayTime_ChangeGunCountine(delayTime_ChangeGunCountine));
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
            
            ammoWidget.Clear(0); //! sung con tren nguoi dung dang Holster => ko in sl dan len UI
        }
    }
    IEnumerator ActivateWeapon(int index) // lay sung len tay isHolster = false
    {
        var weapon = GetWeapon(index); // kiem tra cay sung moi vua pickup da co trong cai o equiped_weapons[] chua
        if (weapon)
        {
            rigAnimator.SetBool("holster_weapon", false);   //neu ko xet false, sau khi o tren holster_weapon = true cat sung
                                                            //den day van true, cho du co animation equip sung loai nao xong thi cung se tro ve trang thai cat sung
                                                            // do bien SetBool("holster_weapon") trong riglayer sung chung ca loai sung
                                                            // XET FALSE de san sang thuc hien hanh dong equip khac
            rigAnimator.Play("equip_" + weapon.WeaponName);
            // DO XET FALSE NEN SE GIU NGUYEN TRNAG THAI EQUIP
            
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
            
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount); //? in thong tin ammoWidget sau khi doi sung bang nut UI
        }

    }
    #endregion HOLSTER AND SWITCHING GUN

    public void DropWeapon() {
        var currentWeapon = GetActiveWeapon();
        if(currentWeapon) {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();

            equipped_weapons[activeWeaponIndex] = null;
        }
    }

    public void RefillAmmo(int clipCount) {
        var weapon = GetActiveWeapon();
        if(weapon) {
            weapon.clipCount += clipCount;
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount);
        }
    }
    IEnumerator DelayTime_ChangeGunCountine(float time) {
        yield return new WaitForSeconds(time);
        isChangingGun = false;
    }
    //todo
}
