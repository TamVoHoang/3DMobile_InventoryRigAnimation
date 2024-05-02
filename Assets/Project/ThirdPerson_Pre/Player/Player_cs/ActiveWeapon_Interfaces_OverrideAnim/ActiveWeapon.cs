
using System;
using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    //todo gameObject = Player
    //todo goi weapon through interface

    public enum SwordSlots
    {
        Primary = 0,
        Secondary = 1,
    }

    public Transform[] swordSlots; //? dung de bo chi so index vao de instatiate vu khi ra dung vi tri transform
    public Transform swordHolster_Point; //? vi tri sword se cat
    [SerializeField] ISword[] equipped_swords = new ISword[1];
    ISword GetSword(int index) {
        if(index < 0 || index >= swordSlots.Length)
        {
            return null;
        }
        return equipped_swords[index];
    }

    public bool IsHolstered_Sword { get { return isHolstered_Sword; } }
    public int GetActiveSwordIndex { get { return activeSwordIndex; } }
    [SerializeField] private int activeSwordIndex = 1;
    [SerializeField] private bool isHolstered_Sword = false; // false = dang equip
    private CharacterEquipment characterEquipment;

    //todo override
    [SerializeField] Animator playerAnimator;
    [SerializeField] ItemScriptableObject default_IWeapon; //scriptable vu khi mac dinh tay khong
    [SerializeField] MonoBehaviour defaultActiveWeapon; // gameobject chua class chua scriptable ISword
    [SerializeField] MonoBehaviour currenActiveWeapon;
    private MonoBehaviour weaponTemp;

    public AnimatorOverrideController overrideControllers;
    [SerializeField] private float timeBetweenAttacks;
    private string weaponName;
    //private int weaponDamage;
    private bool isAttacking = false;

    //todo swords raycast
    [SerializeField] GameObject swordSpawnPoint_Raycast;
    // [SerializeField] GameObject leftHand;
    // [SerializeField] GameObject rightHand;
    [SerializeField] float minSwordDisRaycast = 1f;


    protected override void Awake() {
        base.Awake();
    }
    
    private void Start() {
        isHolstered_Sword = true;
        characterEquipment = GetComponent<CharacterEquipment>();
        playerAnimator = GetComponent<Animator>();
        
        ////CurrenActiveWeapon = defaultWeapon.pfItem.GetComponent<MonoBehaviour>(); //todo gameobject cua phan chung
        defaultActiveWeapon = default_IWeapon.pfWeaponInterface.GetComponent<MonoBehaviour>();//todo gameObject cua phan interface
        currenActiveWeapon = defaultActiveWeapon; // dau tien vao la mac dinh khoi dong tay

        AttackCoolDown();// testing thu
    }
    private void Update() {
        AttackCurrentWeapon();
    }
    
    //? xet override aniton tro ve default weapon danh tya khong khi khong con trang bi chracterEquiment
    public void SetDefaultWeapon() {
        overrideControllers = (defaultActiveWeapon as IWeapon).GetWeaponInfo().animatorOverrideController;
        playerAnimator.runtimeAnimatorController = overrideControllers;
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        weaponTemp = newWeapon;
        int swordSlotIndex = (int)newWeapon.GetComponent<ISword>().swordSlot;
        var sword = GetSword(swordSlotIndex);
        if(sword) {
            Destroy(sword.gameObject);
            Debug.Log("co xoa tai loi cho nay khong");
        }

        currenActiveWeapon = newWeapon;
        
        weaponName = (currenActiveWeapon as IWeapon).GetWeaponInfo().itemName;
        timeBetweenAttacks = (currenActiveWeapon as IWeapon).GetWeaponInfo().coolDownTime;
        overrideControllers = (currenActiveWeapon as IWeapon).GetWeaponInfo().animatorOverrideController;
        playerAnimator.runtimeAnimatorController = overrideControllers;

        sword = currenActiveWeapon.GetComponent<ISword>();
        equipped_swords[swordSlotIndex] = sword;
        SetActiveSword(sword.swordSlot);
    }

    private void AttackCurrentWeapon() {
        if(InputManager.Instance.IsAttackButton && !isAttacking && currenActiveWeapon && ActiveGun.Instance.IsHolstered) {
            isAttacking = true;
            ////StopAllCoroutines(); // dang test
            AttackCoolDown();
            (currenActiveWeapon as IWeapon).Attack();
            playerAnimator.SetTrigger("Attack");
        }

        if(currenActiveWeapon != null && !isHolstered_Sword && InputManager.Instance.IsAttackButton) {
            CheckSwordRaycast(swordSpawnPoint_Raycast.transform, Vector3.up);
        }

    }
    IEnumerator TimeBetweenAttackRoutine() {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    //? gia tri float coolDown quyet dinh khoang cach thuc hien hanh dong attack moi loai vu khi
    private void AttackCoolDown() {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttackRoutine()); //isAttacking = false;
    }

    public void EquipSword(ISword newSword) {
        int swordSlotIndex = (int)newSword.swordSlot;
        var sword = GetSword(swordSlotIndex);
        if(sword) {
            Destroy(sword.gameObject);
            Debug.Log("Destroy old sword");
        }
    }
    public void HolsterSwordsBeforeDeath() {
        playerAnimator.SetBool("holster_sword", true);
        playerAnimator.SetBool("ReadyAttack", false);
    } 
    public void ToggleActiveSword() {
        bool isHolstered = playerAnimator.GetBool("holster_sword"); //?false = dang equip
        //dao nguoc bien trong script va gan cho bien torng aniamtior
        if (isHolstered) StartCoroutine(ActivateSword(activeSwordIndex));
        else StartCoroutine(HolsterSword(activeSwordIndex));
    }

    public void SetActiveSword(SwordSlots swordSlots){
        int holsterIndex = activeSwordIndex;
        int activeIndex = (int)swordSlots;
        if (holsterIndex == activeIndex) holsterIndex = -1;
        StartCoroutine(SwitchSword(holsterIndex, activeIndex));
    }

    IEnumerator SwitchSword(int holsterIndex,int activeIndex) {
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(HolsterSword(holsterIndex));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(ActivateSword(activeIndex));
        activeSwordIndex = activeIndex;
    }
    IEnumerator HolsterSword(int index) {
        var sword = GetSword(index); // kiem tra xem cai o equiped_Weapon dang co hay ko de chuan bi thay, neu varWeapon co thi ko thuc hien animation cat sung
        if (sword)
        {
            Debug.Log(" SetBool holster True sword animation");
            playerAnimator.SetBool("holster_sword", true); // thuc hien animation holster
            playerAnimator.SetBool("ReadyAttack", false);
            yield return new WaitForSeconds(0.5f);
            if(characterEquipment.GetI_SwordPrefabTemp != null) {
                characterEquipment.GetI_SwordPrefabTemp.transform.SetParent(swordHolster_Point, false);
                characterEquipment.GetI_SwordPrefabTemp.transform.SetParent(swordHolster_Point, true);
            }
            Switch_DeafaultWeapon(defaultActiveWeapon);
            
            isHolstered_Sword = true;
        }
    }
    private void Switch_DeafaultWeapon(MonoBehaviour weapon) {
        currenActiveWeapon = weapon;
        overrideControllers = (currenActiveWeapon as IWeapon).GetWeaponInfo().animatorOverrideController;
        playerAnimator.runtimeAnimatorController = overrideControllers;
    }

    IEnumerator ActivateSword(int index) {
        var sword = GetSword(index); // kiem tra cay sung moi vua pickup da co trong cai o equiped_weapons[] chua
        
        if (sword)
        {
            Debug.Log("SetBool holster True sword animation");
            playerAnimator.SetBool("holster_sword", false);
            playerAnimator.SetBool("ReadyAttack", true);
            yield return new WaitForSeconds(.5f); //! cho cat sung xong se sin hra cay kiem
            characterEquipment.GetI_SwordPrefabTemp.transform.SetParent(swordSlots[index], false);
            characterEquipment.GetI_SwordPrefabTemp.transform.SetParent(swordSlots[index], true);

            Switch_DeafaultWeapon(weaponTemp); //? tro ve vu khi tep la vu khi luc dau khi no xet o NewWeapon()

            isHolstered_Sword = false;
        }
    }
    
    private void CheckSwordRaycast(Transform RHand, Vector3 aimDirection) {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Character"); //! player and enemy have same LayerMask

        if(Physics.Raycast(RHand.position, RHand.transform.TransformDirection(aimDirection), out hit, minSwordDisRaycast, layerMask)) {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.yellow);

            if(hit.collider.gameObject.CompareTag("Player")) return;

            var hitBox = hit.collider.GetComponent<HitBox>();
            var hitEnemy = hitBox.GetComponent<Health>();

            var damage = (currenActiveWeapon as IWeapon).GetWeaponInfo().damage;
            if(hitBox && !hitEnemy.IsDead) {
                hitBox.OnSwordRaycastHit(damage, hitBox.transform.position); //ray.direction
            }
        } else {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.red);

        }
    }
    
    //todo
}
