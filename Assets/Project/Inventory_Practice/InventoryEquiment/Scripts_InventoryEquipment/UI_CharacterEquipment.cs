using System;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CharacterEquipment : MonoBehaviour
{
    //todo gameobject = folder cha cua cac weaponSlot
    //todo dung de kiem tra keo tha dung doi tuong tu o weaponContainer into UI characterEquipment

    [SerializeField] private Transform pfUI_Item;
    private CharacterEquipment characterEquipment; // bien duoc gan ben testing.cs
    private Transform itemContainer;
    
    private UI_CharacterEquipmentSlot weaponRifleSlot;
    private UI_CharacterEquipmentSlot weaponPistolSlot;
    private UI_CharacterEquipmentSlot armorSlot;
    private UI_CharacterEquipmentSlot helmetSlot;

    private Item itemTemp;


    private void Awake() {
        itemContainer = transform.Find("itemContainer");

        weaponRifleSlot = transform.Find("weaponRifleSlot").GetComponent<UI_CharacterEquipmentSlot>();
        weaponPistolSlot = transform.Find("weaponPistolSlot").GetComponent<UI_CharacterEquipmentSlot>();

        armorSlot = transform.Find("armorSlot").GetComponent<UI_CharacterEquipmentSlot>();
        helmetSlot = transform.Find("helmetSlot").GetComponent<UI_CharacterEquipmentSlot>();

        //! event col 27 UI_CharacterEquipmentSlot.cs goi khi co item dat vao weaponSlot
        weaponRifleSlot.OnItemDropped += WeaponRifleSlot_OnItemDropped;
        weaponPistolSlot.OnItemDropped += WeaponPistolSlot_OnItemDropped;

        armorSlot.OnItemDropped += ArmorSlot_OnItemDropped;
        helmetSlot.OnItemDropped += HelmetSlot_OnItemDropped;

        // //! click mouse 1 de remove khoi weaponSLot trang bi 3 o
        // weaponSlot.OnItemPointerRightClicked += WeaponSlot_OnItemPointerRightClicked;

        //? - wepontSlot (ITEM duoc bo Droped vao) - event UI_characterEquipment.cs - reun WeaponSlot_OnItemDropped() 
        //? - run SetWeaponItem(e.item) -> dem ITEM chuyen qua CharacterEquipment.cs (player) - event OnEquipmentChanged col 38 45 50
        //? - characterEquipment.OnEquipmentChanged duco gan dia chi ham ben testing Awake()
        //? - run += CharacterEquipment_OnEquipmentChnaged() => update hinh anh Item len WeaponSLot
    }

    //#region rightClick to remove Item on weaponSLot
    // private void Helmetlot_OnItemPointerRightClicked(object sender, UI_CharacterEquipmentSlot.OnItemPointerClick e)
    // {
    //     characterEquipment.SetHelmetItem(e.item);
    // }
    // #endregion rightClick to remove Item on weaponSLot

#region droped Item on weaponSLot
    private void WeaponPistolSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        Debug.Log("doi tuong weaponSlot thong bao || equipweapon " + e.item.itemScriptableObject.itemType);

        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.WeaponPistol;
        if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            Debug.Log("weapon move from weaponInventory to weaponSlotEquipment");
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        }
    }
    private void WeaponRifleSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in weapon slot
        Debug.Log("doi tuong weaponSlot thong bao || equipweapon " + e.item.itemScriptableObject.itemType);

        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.WeaponRifle;
        
        if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            Debug.Log("weapon move from weaponInventory to weaponSlotEquipment");
            e.item.RemoveFromItemHolder(); // xoa item khoi slot vua move qua slot khac
            characterEquipment.EquipItem(e.item);
        }
    }
    private void ArmorSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in Armor slot
        Debug.Log("doi tuong armorSlot thong bao || equiparmor " + e.item);

        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Armor;
        if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        }
    }
    private void HelmetSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in Helmet slot
        Debug.Log("doi tuong helmetSlot thong bao || equipHelmet " + e.item);

        CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Helmet;
        if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
            e.item.RemoveFromItemHolder();
            characterEquipment.EquipItem(e.item);
        }

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

    //todo hien thi loai item keo tu pfUI_Item - tu o weapon Container len weaponSlot
    private void UpdateVisual() {
        //cai loai itemType nao dang xet
        Debug.Log("item dang kich hoat UpdateVisual " + itemTemp);
        foreach (Transform child in itemContainer) {

            //?  truoc khi Item bi thay the thi xet isEquip = false || col 33 Item.cs || col 22 UI_characterEquipment.cs
            // Item item = child.GetComponent<UI_Item>().TryGetType(); // loai item dang co ben trong doi tuong icontainer
            // Debug.Log("item ben trong child" +item);
            // if(item.GetEquipSlot() == itemTemp.GetEquipSlot())
            //     child.GetComponent<UI_Item>().SetIsEquiped(false);
            
            Destroy(child.gameObject); //? xoa item trong slot khi keo tren xuong weaponInventory
        }

        //todo hien thi pistol gun len weaponSlot Equipment
        Item weaponPistolItem = characterEquipment.GetWeaponPistolItem(); //lay loai weapon ben characterEquipment.cs dang co tren nguoi
        if(weaponPistolItem != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = weaponPistolSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(weaponPistolItem);
            weaponPistolSlot.transform.Find("emptyImage").gameObject.SetActive(false);

            //? testing doi dung thong qua nut nhan tren UI_WeaponItem - click vao item tren weaponSlotEquipment
            uiItemTransform.GetComponent<RectTransform>().GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                Debug.Log("click vao weaponPistolItem tren weaponSlot");
                
                if(!ActiveGun.Instance.IsHolstered && ActiveGun.Instance.GetActiveWeaponIndex == 1)
                    ActiveGun.Instance.ToggleActiveWeapon();
                else
                    ActiveGun.Instance.SetActiveWeapon(weaponPistolItem.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot);
            };
        }
        else {
            weaponPistolSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }
        
        //todo hien thi rifle gun len weaponSlot Equipment
        Item weaponRifleItem = characterEquipment.GetWeaponItem(); //lay loai weapon ben characterEquipment.cs dang co tren nguoi
        if(weaponRifleItem != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = weaponRifleSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(weaponRifleItem);
            weaponRifleSlot.transform.Find("emptyImage").gameObject.SetActive(false);

            //? testing doi dung thong qua nut nhan tren UI_WeaponItem - click vao item tren weaponSlotEquipment
            uiItemTransform.GetComponent<RectTransform>().GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                Debug.Log("click vao weaponRifleItem tren weaponSlot");

                // neu player dang cam aiming + activeWeaponSLot = 0 (dang aiming Rifle) => toggle rifle
                // neu player dang aiming nhung activeWeaponSlot = 1(dag aiming Pistol) thi toggle pistol + aiming Rifle
                if(!ActiveGun.Instance.IsHolstered && ActiveGun.Instance.GetActiveWeaponIndex == 0)
                    ActiveGun.Instance.ToggleActiveWeapon();
                else
                    ActiveGun.Instance.SetActiveWeapon(weaponRifleItem.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot);
            };
        }
        else {
            weaponRifleSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }

        //todo hien thi armor len armorSlot Equipment
        Item armorItem = characterEquipment.GetArmorItem(); //lay loai weapon ben characterEquipment.cs
        if(armorItem  != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = armorSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(armorItem);
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        }
        else {
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }

        Item helmetItem = characterEquipment.GetHelmetItem(); //lay loai weapon ben characterEquipment.cs
        if(helmetItem  != null) {
            // sinh pfItem len
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = helmetSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;

            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(helmetItem);

            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        }
        else {
            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }


    }





}
