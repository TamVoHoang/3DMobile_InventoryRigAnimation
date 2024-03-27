using System.Collections;
using UnityEngine;

public class ActiveSword : Singleton<ActiveSword>
{
    
    public enum SwordSlots
    {
        Primary = 0,
        Secondary = 1,
    }
    [Header ("Sword")]
    public Transform[] swordSlots; //? dung de bo chi so index vao de instatiate vu khi ra dung vi tri transform

    [SerializeField] HandSwordWeapon[] equipped_swords = new HandSwordWeapon[1]; // noi se chua handSwordWeapon.cs

    [SerializeField] private int activeSwordIndex = 1;
    public int GetActiveSwordIndex { get { return activeSwordIndex; } }
    [SerializeField] private bool isHolstered_Sword = false; // false = dang equip
    public bool IsHolstered_Sword { get { return isHolstered_Sword; } }
    private Animator animator;
    public Transform swordHolster_Point; //? vi tri sword se cat
    private CharacterEquipment characterEquipment;

    protected override void Awake() {
        base.Awake();
    }
    private void Start() {
        animator = GetComponent<Animator>();
        characterEquipment = GetComponent<CharacterEquipment>();
    }

    HandSwordWeapon GetSword(int index) {
        if(index < 0 || index >= swordSlots.Length)
        {
            return null;
        }
        return equipped_swords[index];
    }

    private void Update() {
        var sword = GetSword(activeSwordIndex);
        if(sword && !isHolstered_Sword) {
            //Debug.Log("co sword in activeSword.cs");
            if(InputManager.Instance.IsAttackButton) sword.UpdateSword(Time.deltaTime);
        }
    }

    public void EquipSword(HandSwordWeapon newSword) {
        int swordSlotIndex = (int)newSword.swordSlot;
        var sword = GetSword(swordSlotIndex);
        if(sword) {
            Destroy(sword.gameObject);
            Debug.Log("Destroy old sword");
        }
        sword = newSword;
        equipped_swords[swordSlotIndex] = sword;
        SetActiveSword(newSword.swordSlot);
    }
    public void ToggleActiveSword()
    {
        bool isHolstered = animator.GetBool("holster_sword"); //?false = dang equip
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
    IEnumerator SwitchSword(int holsterIndex,int activeIndex) // ok
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(HolsterSword(holsterIndex));
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(ActivateSword(activeIndex));
        activeSwordIndex = activeIndex;
    }
    IEnumerator HolsterSword(int index)
    {
        isHolstered_Sword = true;
        var sword = GetSword(index); // kiem tra xem cai o equiped_Weapon dang co hay ko de chuan bi thay, neu varWeapon co thi ko thuc hien animation cat sung
        if (sword)
        {
            Debug.Log(" SetBool holster True sword animation");
            animator.SetBool("holster_sword", true);
            characterEquipment.GetPrefab_SwordTemp.transform.SetParent(swordHolster_Point, false);
            characterEquipment.GetPrefab_SwordTemp.transform.SetParent(swordHolster_Point, true);
            yield return new WaitForSeconds(.5f);
        }
    }
    IEnumerator ActivateSword(int index)
    {
        var sword = GetSword(index); // kiem tra cay sung moi vua pickup da co trong cai o equiped_weapons[] chua
        if (sword)
        {
            Debug.Log("SetBool holster True sword animation");
            animator.SetBool("holster_sword", false);
            characterEquipment.GetPrefab_SwordTemp.transform.SetParent(swordSlots[index], false);
            characterEquipment.GetPrefab_SwordTemp.transform.SetParent(swordSlots[index], true);
            isHolstered_Sword = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}
