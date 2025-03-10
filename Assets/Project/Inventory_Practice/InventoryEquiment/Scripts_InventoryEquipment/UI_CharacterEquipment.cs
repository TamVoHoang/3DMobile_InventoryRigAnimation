using System;
using System.Collections;
using CodeMonkey.Utils;
using UnityEngine;

public class UI_CharacterEquipment : MonoBehaviour
{
    //todo gameobject = folder cha cua cac weaponSlot
    //todo dung de kiem tra keo tha dung doi tuong tu o weaponContainer into UI characterEquipment

    [SerializeField] private Transform pfUI_Item;
    [SerializeField] private float pfUI_ItemSCale = 1f;
    private CharacterEquipment characterEquipment; // bien duoc gan ben testing.cs
    private Transform itemContainer;
    
    private UI_CharacterEquipmentSlot weaponRifleSlot;
    private UI_CharacterEquipmentSlot weaponPistolSlot;
    private UI_CharacterEquipmentSlot weaponSwordSlot;

    private UI_CharacterEquipmentSlot armorSlot;
    private UI_CharacterEquipmentSlot helmetSlot;

    private Item itemTemp;

    bool isClicked = false;

    private void Awake() {
        
        itemContainer = transform.Find("itemContainer");

        weaponRifleSlot = transform.Find("weaponRifleSlot").GetComponent<UI_CharacterEquipmentSlot>();
        weaponPistolSlot = transform.Find("weaponPistolSlot").GetComponent<UI_CharacterEquipmentSlot>();
        weaponSwordSlot = transform.Find("weaponSwordSlot").GetComponent<UI_CharacterEquipmentSlot>();


        armorSlot = transform.Find("armorSlot").GetComponent<UI_CharacterEquipmentSlot>();
        helmetSlot = transform.Find("helmetSlot").GetComponent<UI_CharacterEquipmentSlot>();

        //! event col 27 UI_CharacterEquipmentSlot.cs goi khi co item dat vao weaponSlot
        weaponRifleSlot.OnItemDropped += WeaponRifleSlot_OnItemDropped;
        weaponPistolSlot.OnItemDropped += WeaponPistolSlot_OnItemDropped;
        weaponSwordSlot.OnItemDropped += WeaponSwordSlot_OnItemDropped;


        armorSlot.OnItemDropped += ArmorSlot_OnItemDropped;
        helmetSlot.OnItemDropped += HelmetSlot_OnItemDropped;

        //? - wepontSlot (ITEM duoc bo Droped vao) - event UI_characterEquipment.cs - reun WeaponSlot_OnItemDropped() 
        //? - run SetWeaponItem(e.item) -> dem ITEM chuyen qua CharacterEquipment.cs (player) - event OnEquipmentChanged col 38 45 50
        //? - characterEquipment.OnEquipmentChanged duco gan dia chi ham ben testing Awake()
        //? - run += CharacterEquipment_OnEquipmentChnaged() => update hinh anh Item len WeaponSLot

        isClicked = false;
    }


    #region droped Item on weaponSLot
    private void WeaponSwordSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        //! testing for ISword Interface
        Debug.Log("doi tuong weaponSlot thong bao || equipweapon " + e.item.itemScriptableObject.itemType);
        CharacterEquipment.EquipSlot equipSlot_Interface = CharacterEquipment.EquipSlot.WeaponSword;
        if (characterEquipment.IsEquipSlotEmpty(equipSlot_Interface) && characterEquipment.CanEquipItem(equipSlot_Interface, e.item)) {
            Debug.Log("I_sword move from weaponInventory to weaponSlot_CharacterEquipment");
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        }
        else {
            Debug.Log("co bo kiem xanh vao kiem do nhung ko du dieu kien de add vao characterEquipment");
        }
        //! testing for ISword Interface

    }

    private void WeaponPistolSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        Debug.Log("doi tuong weaponSlot thong bao || equipweapon " + e.item.itemScriptableObject.itemType);

        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.WeaponPistol; // kiem tra slot tren player khi keo tu duoi WeponInvetory len
        ItemSlot_OnItemDropped(equipSlot, e.item);

        /* if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            Debug.Log("weapon move from weaponInventory to weaponSlotEquipment");
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        } */
    }

    private void WeaponRifleSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in weapon slot
        Debug.Log("doi tuong weaponSlot thong bao || equipweapon " + e.item.itemScriptableObject.itemType);

        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.WeaponRifle;
        
        ItemSlot_OnItemDropped(equipSlot, e.item);
    }

    private void ItemSlot_OnItemDropped(CharacterEquipment.EquipSlot equipSlot, Item item) {
        if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, item)) {
            Debug.Log("weapon move from weaponInventory to weaponSlotEquipment");
            item.RemoveFromItemHolder(); //! se remove item khi item nay duoc keo tu iventory sang equipment slot
            characterEquipment.EquipItem(item);
        }
    }

    private void ArmorSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in Armor slot
        Debug.Log("doi tuong armorSlot thong bao || equiparmor " + e.item);
        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Armor;

        ItemSlot_OnItemDropped(equipSlot, e.item);
        /* if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        } */
    }
    private void HelmetSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in Helmet slot
        Debug.Log("doi tuong helmetSlot thong bao || equipHelmet " + e.item);
        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Helmet;

        ItemSlot_OnItemDropped(equipSlot, e.item);
        /* if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        } */
    }
#endregion droped Item on weaponSLot

    //todo testing.cs - lay weapon CharacterEquipment.cs - 
    public void SetCharacterEquipment(CharacterEquipment characterEquipment) {
        this.characterEquipment = characterEquipment;
        UpdateVisual();

        characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChnaged; //! ham start ben testing da chay o day
    }

    private void CharacterEquipment_OnEquipmentChnaged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisualBothGun(Item item, UI_CharacterEquipmentSlot weaponSlot) {
        if(item != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = weaponSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * pfUI_ItemSCale;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(item);
            weaponSlot.transform.Find("emptyImage").gameObject.SetActive(false);

            //? testing doi dung thong qua nut nhan tren UI_WeaponItem - click vao item tren weaponSlotEquipment
            uiItemTransform.GetComponent<RectTransform>().GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                Debug.Log("click vao weaponPistolItem tren weaponSlot");
                
                // co the su dung tai day thi ko dung 168 => dat tai day co the chua cat kiem xong thi tay da trang bi sung
                //if(!ActiveWeapon.Instance.IsHolstered_Sword) ActiveWeapon.Instance.ToggleActiveSword(); // cat kiem xong -delay-lay sung
                
                if(!isClicked) {
                    isClicked = true;
                    StartCoroutine(DelayTimeCountine(0.8f, item)); // click vao sung, cat kiem - delay 0.5s - setActiveWeapon(sung)
                }
                
            };
        } else {
            weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }
    }

    // click len o sung -> cat kime -> trang bi sung
    IEnumerator DelayTimeCountine(float time, Item item) {
        //? move tu 158 xuong 168 => de cat kiem hoan toan thi trnag bi sung
        if(!ActiveWeapon.Instance.IsHolstered_Sword) ActiveWeapon.Instance.ToggleActiveSword(); //! cat kiem xong -delay-lay sung

        yield return new WaitForSeconds(time);

        if(!ActiveGun.Instance.IsHolstered && 
            (int)item.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot == ActiveGun.Instance.GetActiveWeaponIndex) {
            ActiveGun.Instance.ToggleActiveWeapon();
        }
        else {
            ActiveGun.Instance.SetActiveWeapon(item.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot);
        }

        isClicked = false;
    }


    //todo hien thi loai item keo tu pfUI_Item - tu o weapon Container len weaponSlot
    private void UpdateVisual() {
        //cai loai itemType nao dang xet
        Debug.Log("item dang kich hoat UpdateVisual " + itemTemp);
        foreach (Transform child in itemContainer) {
            Destroy(child.gameObject); //? xoa item trong slot khi keo tren xuong weaponInventory
        }

        //todo hien thi pistol gun len UI weaponSlot Equipment
        Item weaponPistolItem = characterEquipment.GetWeaponPistolItem(); //lay loai weapon ben characterEquipment.cs dang co tren nguoi
        UpdateVisualBothGun(weaponPistolItem, weaponPistolSlot); //! testing

        //todo hien thi rifle gun len UI weaponSlot Equipment
        Item weaponRifleItem = characterEquipment.GetWeaponRifleItem(); //lay loai weapon ben characterEquipment.cs dang co tren nguoi
        UpdateVisualBothGun(weaponRifleItem, weaponRifleSlot); //! testing

        //todo hien thi armor len UI armorSlot Equipment
        Item armorItem = characterEquipment.GetArmorItem(); //lay loai armor ben characterEquipment.cs
        UpdateVisualArmorHelmet(armorItem, armorSlot);
        /* if(armorItem  != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = armorSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(armorItem);
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        }
        else {
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        } */

        //todo hien thi armor len UI helmet Equipment
        Item helmetItem = characterEquipment.GetHelmetItem(); //lay loai helmet ben characterEquipment.cs
        UpdateVisualArmorHelmet(helmetItem, helmetSlot);
        /* if(helmetItem  != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = helmetSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(helmetItem);

            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        }
        else {
            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        } */

        //todo hien thi Sword len UI weaponSlot Equipment
        Item weaponSwordItem = characterEquipment.GetWeaponSwordItem(); //lay loai weapon ben characterEquipment.cs dang co tren nguoi
        if(weaponSwordItem  != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = weaponSwordSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * pfUI_ItemSCale;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(weaponSwordItem);
            weaponSwordSlot.transform.Find("emptyImage").gameObject.SetActive(false);

            //? testing doi dung thong qua nut nhan tren UI_WeaponItem - click vao item tren weaponSlotEquipment
            uiItemTransform.GetComponent<RectTransform>().GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                Debug.Log("click vao weaponSwordItem tren weaponSlot");

                //? cat sung neu co -> trang bi sword => DUNG OK
                /* if(!ActiveGun.Instance.IsHolstered) ActiveGun.Instance.ToggleActiveWeapon();
                // !ActiveSword_OldVersion.Instance.IsHolstered_Sword && 
                if((int)weaponSwordItem.itemScriptableObject.pfWeaponInterface.GetComponent<ISword>().swordSlot == ActiveWeapon.Instance.GetActiveSwordIndex) {
                    ActiveWeapon.Instance.ToggleActiveSword();
                } else {
                    ActiveWeapon.Instance.SetActiveSword(weaponSwordItem.itemScriptableObject.pfWeaponInterface.GetComponent<ISword>().swordSlot);
                } */

                if(!isClicked) {
                    isClicked = true;
                    StartCoroutine(EquipSword_Countine(0.7f, weaponSwordItem));
                }
                
            };
        }
        else {
            weaponSwordSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }
    }

    // click len o sword -> cat sung -> trang bi kiem
    IEnumerator EquipSword_Countine(float time, Item weaponSwordItem) {
        if(!ActiveGun.Instance.IsHolstered) ActiveGun.Instance.ToggleActiveWeapon();

        yield return new WaitForSeconds(time);

        if((int)weaponSwordItem.itemScriptableObject.pfWeaponInterface.GetComponent<ISword>().swordSlot == ActiveWeapon.Instance.GetActiveSwordIndex) {
            ActiveWeapon.Instance.ToggleActiveSword();
        } else {
            ActiveWeapon.Instance.SetActiveSword(weaponSwordItem.itemScriptableObject.pfWeaponInterface.GetComponent<ISword>().swordSlot);
        }

        isClicked = false;
    }


    private void UpdateVisualArmorHelmet(Item item, UI_CharacterEquipmentSlot slot) {
        if(item != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * pfUI_ItemSCale;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(item);
            slot.transform.Find("emptyImage").gameObject.SetActive(false);
        }
        else {
            slot.transform.Find("emptyImage").gameObject.SetActive(true);
        }
    }


    //todo
}
