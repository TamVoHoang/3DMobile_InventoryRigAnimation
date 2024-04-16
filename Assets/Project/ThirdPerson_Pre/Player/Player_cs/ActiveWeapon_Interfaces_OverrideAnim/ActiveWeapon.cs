
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
    [SerializeField] private int activeSwordIndex = 1;
    public int GetActiveSwordIndex { get { return activeSwordIndex; } }
    [SerializeField] private bool isHolstered_Sword = false; // false = dang equip
    public bool IsHolstered_Sword { get { return isHolstered_Sword; } }
    private CharacterEquipment characterEquipment;

    //todo override
    [SerializeField] private Animator playerAnimator;
    [SerializeField] ItemScriptableObject default_IWeapon; //scriptable vu khi mac dinh tay khong
    [SerializeField] private MonoBehaviour defaultActiveWeapon; // gameobject chua class chua scriptable ISword
    [SerializeField] private MonoBehaviour currenActiveWeapon;
    private MonoBehaviour weaponTemp;

    [SerializeField] private float timeBetweenAttacks = .5f;
    public AnimatorOverrideController overrideControllers;
    private string weaponName;
    //private int weaponDamage;
    private bool isAttacking = false;

    protected override void Awake() {
        base.Awake();
    }
    
    private void Start() {
        isHolstered_Sword = true;
        characterEquipment = GetComponent<CharacterEquipment>();
        playerAnimator = GetComponent<Animator>();
        
        //CurrenActiveWeapon = defaultWeapon.pfItem.GetComponent<MonoBehaviour>(); //todo gameobject cua phan chung
        defaultActiveWeapon = default_IWeapon.pfWeaponInterface.GetComponent<MonoBehaviour>();//todo gameObject cua phan interface
        currenActiveWeapon = defaultActiveWeapon; // dau tien vao la mac dinh khoi dong tay
    }
    private void Update() {
        AttackCurrentWeapon();
        
    }
    public void SetDefaultWeapon() //? xet override aniton tro ve default weapon danh tya khong khi khong con trang bi chracterEquiment
    {
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
        overrideControllers = (currenActiveWeapon as IWeapon).GetWeaponInfo().animatorOverrideController;
        playerAnimator.runtimeAnimatorController = overrideControllers;

        sword = currenActiveWeapon.GetComponent<ISword>();
        equipped_swords[swordSlotIndex] = sword;
        SetActiveSword(sword.swordSlot);
    }

    private void AttackCurrentWeapon() {
        if(InputManager.Instance.IsAttackButton && !isAttacking && currenActiveWeapon && ActiveGun.Instance.IsHolstered) {
            isAttacking = true;
            StopAllCoroutines();
            AttackCoolDown();
            (currenActiveWeapon as IWeapon).Attack();
            playerAnimator.SetTrigger("Attack");
        }

    }
    IEnumerator TimeBetweenAttackRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
    private void AttackCoolDown() //? gia tri float coolDown quyet dinh khoang cach thuc hien hanh dong attack moi loai vu khi
    {
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

    public void ToggleActiveSword()
    {
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
    
    
    

}
