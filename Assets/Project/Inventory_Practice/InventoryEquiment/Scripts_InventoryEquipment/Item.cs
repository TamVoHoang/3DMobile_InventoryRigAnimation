using System;
using UnityEngine;


[Serializable]
public class Item
{
    //? Invenotry.cs goi khi khoi tao Inventoty()
    public enum ItemType
    {
        None,
        Sword_01,
        Sword_02,
        Sword_broken,
        Sword_iron,
        Sword_gold,

        Helmet_01,
        Armor_01,
        Armor_02,

        HealthPotion,
        ManaPotion,
        Coin,
        Medkit,

        Iron,
        Gold,
        
        Sword3D_01,
        GunSMG3D_01,
        GunPistol3D_01,
        IHand, //todo IWeapon
        ISword_Red_01,
        ISword_Green_02,
        IMagPistol3D_01,
        IHealthPickup3D_01,
        GunSMWEP_01,
        GunSMWEP_02,

        GunPistol3D_02,
        GunPistol3D_03,
    }
    public Item() {
        
    }
    
    // todo them vao de khoi tao Json| KO DUNG DUOC do khi kiem tra Istackable(dung itemscriptableObject)
    //public string guid;
    public ItemType itemType; // dung doi tuong nay de luu playfab - khi load ve se dung dao nguoc lai item
    public int amount = 1;
    public ItemScriptableObject itemScriptableObject; // GUID

    private IItemHolder itemHolder;
    public bool isSplited; //! them vao truong hop double click chia doi amout - khi crafting

    public void SetItemHolder(IItemHolder itemHolder) {
        this.itemHolder = itemHolder;
    }

    public IItemHolder GetItemHolder() {
        return itemHolder;
    }

    // Remove item tu slot nay sang slot khac
    public void RemoveFromItemHolder() {
        if(itemHolder == null) Debug.Log("itemHolder == null");
        if (itemHolder != null) {
            Debug.Log("itemHolder != null");
            // Remove from current Item Holder
            itemHolder.RemoveItemEquipment(this); //! rat quan trong de remove khi keo tu characterEquipment slot sang Inventory
        }
    }

    /* public void MoveToAnotherItemHolder(IItemHolder newItemHolder) {
        RemoveFromItemHolder();
        // Add to new Item Holder
        newItemHolder.AddItemEquipment(this);
    } */


    // todo duoc xet true khi tu container weapon -> weaponSlot
    // todo duoc xet false khi tu bi thay the boi 1 loai tuong tu keo tu container weapon len
    //[SerializeField ] private bool isEquiped;

    //todo duoc Set col 22 UI_characterEquipmentSlot.cs + col 73 UI_characterEquipment.cs
    /* public bool SetIsEquipedItem(bool isEquiped)
    {
        return this.isEquiped = isEquiped;
    }
    public bool GetIsEquipedItem() //todo duoc Get kiem tra o dong col 254 UI_inventory.cs truoc khi remove or Drop
    {
        return isEquiped;
    } */

    public override string ToString() {
        ////return itemType.ToString();
        return itemScriptableObject.name;
    }

    public static Sprite GetSprite(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.Sword_01:             return ItemAssets.Instance.swordSprite_01;
            case ItemType.Sword_02:             return ItemAssets.Instance.swordSprite_02;
            case ItemType.Sword_broken:         return ItemAssets.Instance.swordSprite_broken;
            case ItemType.Sword_iron:           return ItemAssets.Instance.swordSprite_iron;
            case ItemType.Sword_gold:           return ItemAssets.Instance.swordSprite_gold;

            case ItemType.Helmet_01:            return ItemAssets.Instance.helmetSprite_01;
            case ItemType.Armor_01:             return ItemAssets.Instance.armorSprite_01;
            case ItemType.Armor_02:             return ItemAssets.Instance.armorSprite_02;

            case ItemType.HealthPotion:         return ItemAssets.Instance.healthPotionSprite;
            case ItemType.ManaPotion:           return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Coin:                 return ItemAssets.Instance.coinSprite;
            case ItemType.Medkit:               return ItemAssets.Instance.medkitSprite;

            case ItemType.Iron:                 return ItemAssets.Instance.iron;
            case ItemType.Gold:                 return ItemAssets.Instance.gold;

            case ItemType.Sword3D_01:           return ItemAssets.Instance.swordSprite3D_01;

            case ItemType.GunPistol3D_01:       return ItemAssets.Instance.gunPistolSprite3D_01;
            case ItemType.GunPistol3D_02:       return ItemAssets.Instance.gunPistolSprite3D_02;
            case ItemType.GunPistol3D_03:       return ItemAssets.Instance.gunPistolSprite3D_03;

            case ItemType.GunSMG3D_01:          return ItemAssets.Instance.gunSMGSprite3D_01;
            case ItemType.GunSMWEP_01:          return ItemAssets.Instance.gunSMWEPSprite3D_01;
            case ItemType.GunSMWEP_02:          return ItemAssets.Instance.gunSMWEPSprite3D_02;

            case ItemType.IHand:                return ItemAssets.Instance.IHandSprite3D; //todo IWeapon
            case ItemType.ISword_Red_01:        return ItemAssets.Instance.ISword_Sprite3D_Red01; //todo IWeapon
            case ItemType.ISword_Green_02:      return ItemAssets.Instance.ISword_Sprite3D_Green02; //todo IWeapon

            case ItemType.IMagPistol3D_01:      return ItemAssets.Instance.IMagPistol3D_01; //todo IWeapon
            case ItemType.IHealthPickup3D_01:   return ItemAssets.Instance.IHealthPickup3D_01; //todo IWeapon

        }
    }

    //? so sanh loai item trong Enum va tra ve loai sprite image dang luu trong ItemAssets.cs
    public Sprite GetSprite() {
        ////return GetSprite(itemType);
        return GetSprite(itemScriptableObject.itemType);
    }

    /* public Sprite GetSprite() {
        switch (itemType) {
        default:
        case ItemType.Sword_01:         return ItemAssets.Instance.swordSprite_01;
        case ItemType.Sword_02:         return ItemAssets.Instance.swordSprite_02;

        case ItemType.Helmet_01:        return ItemAssets.Instance.helmetSprite_01;
        case ItemType.Armor_01:         return ItemAssets.Instance.armorSprite_01;
        case ItemType.Armor_02:         return ItemAssets.Instance.armorSprite_02;


        case ItemType.HealthPotion:     return ItemAssets.Instance.healthPotionSprite;
        case ItemType.ManaPotion:       return ItemAssets.Instance.manaPotionSprite;
        case ItemType.Coin:             return ItemAssets.Instance.coinSprite;
        case ItemType.Medkit:           return ItemAssets.Instance.medkitSprite;
        }
    } */

    public GameObject GetPrefab() {
        return GetPrefab(itemScriptableObject.itemType);
    }
    public GameObject GetPrefab(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.Sword_01:         return ItemAssets.Instance.swordPrefab_01;
            case ItemType.Sword_02:         return ItemAssets.Instance.swordPrefab_02;
            case ItemType.Sword_broken:     return ItemAssets.Instance.swordPrefab_broken;
            case ItemType.Sword_iron:       return ItemAssets.Instance.swordPrefab_iron;
            case ItemType.Sword_gold:       return ItemAssets.Instance.swordPrefab_gold;


            case ItemType.Armor_01:         return ItemAssets.Instance.armorPrefab_01;
            case ItemType.Armor_02:         return ItemAssets.Instance.armorPrefab_02;
            case ItemType.Helmet_01:        return ItemAssets.Instance.helmetPrefab_01;

            case ItemType.Sword3D_01:       return ItemAssets.Instance.swordPrefab3D_01;

            case ItemType.GunPistol3D_01:   return ItemAssets.Instance.gunPistolPrefab3D_01;
            case ItemType.GunPistol3D_02:   return ItemAssets.Instance.gunPistolPrefab3D_02;
            case ItemType.GunPistol3D_03:   return ItemAssets.Instance.gunPistolPrefab3D_03;

            case ItemType.GunSMG3D_01:      return ItemAssets.Instance.gunSMGPrefab3D_01;
            case ItemType.GunSMWEP_01:      return ItemAssets.Instance.gunSMWEPPrefab3D_01;
            case ItemType.GunSMWEP_02:      return ItemAssets.Instance.gunSMWEPPrefab3D_02;
            
            case ItemType.IHand:            return ItemAssets.Instance.IHandPrefab;                 //todo IWeapon
            case ItemType.ISword_Red_01:    return ItemAssets.Instance.ISword_Prefab3D_Red01;       //todo IWeapon
            case ItemType.ISword_Green_02:  return ItemAssets.Instance.ISword_Prefab3D_Green02;     //todo IWeapon
            case ItemType.IMagPistol3D_01:  return ItemAssets.Instance.IMagPistolPrefab3D_01;       //todo IWeapon
            case ItemType.IHealthPickup3D_01: return ItemAssets.Instance.IHealthPickupPrefab3D_01;  //todo IWeapon


        }
    }

    public ItemScriptableObject GetScriptableObject() {
        return GetScriptableObject(itemType);
    }

    public ItemScriptableObject GetScriptableObject(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.GunPistol3D_01:       return ItemAssets.Instance.SO_Pistol01_3D_01;
            case ItemType.GunPistol3D_02:       return ItemAssets.Instance.SO_Pistol01_3D_02;
            case ItemType.GunPistol3D_03:       return ItemAssets.Instance.SO_Pistol01_3D_03;

            case ItemType.GunSMG3D_01:          return ItemAssets.Instance.SO_SMG01_3D_01;
            case ItemType.GunSMWEP_01:          return ItemAssets.Instance.SO_SMWEP_3D_01;
            case ItemType.GunSMWEP_02:          return ItemAssets.Instance.SO_SMWEP_3D_02;
            

            case ItemType.ISword_Red_01:        return ItemAssets.Instance.SO_ISword_01;        //todo IWeapon
            case ItemType.ISword_Green_02:      return ItemAssets.Instance.SO_ISword_02;        //todo IWeapon

            case ItemType.IMagPistol3D_01:      return ItemAssets.Instance.SO_IMagPistol_01;    //todo IWeapon
            case ItemType.IHealthPickup3D_01:   return ItemAssets.Instance.SO_IHealthPickup_01; //todo IWeapon
        }
    }

    //todo loai vat pham nao co the duoc cong don
    public bool IsStackable_() {
        return IsStackable(itemType);
    }
    public bool IsStackable() {
        //return true;
        return IsStackable(itemScriptableObject.itemType); // OK dung itemScriptable de kiem tra itemType
    }
    
    public bool IsStackable(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
            case ItemType.Medkit:
            case ItemType.Coin:
            case ItemType.Iron:
            case ItemType.Gold:

            case ItemType.IMagPistol3D_01:
            case ItemType.IHealthPickup3D_01:
                return true;

            case ItemType.Sword_01:
            case ItemType.Sword_02:
            case ItemType.Sword_broken:
            case ItemType.Sword_iron:
            case ItemType.Sword_gold:

            case ItemType.Helmet_01:
            case ItemType.Armor_01:
            case ItemType.Armor_02:
            
            case ItemType.Sword3D_01:

            case ItemType.GunPistol3D_01:
            case ItemType.GunPistol3D_02:
            case ItemType.GunPistol3D_03:

            case ItemType.GunSMG3D_01:
            case ItemType.GunSMWEP_01:
            case ItemType.GunSMWEP_02:
            

            case ItemType.IHand: //todo IWeapon
            case ItemType.ISword_Red_01: //todo IWeapon
            case ItemType.ISword_Green_02: //todo IWeapon

                return false;
        }
    }

    public CharacterEquipment.EquipSlot GetEquipSlot() {
        return itemScriptableObject.equipSlot;

        /* switch (itemType) {
        default:
        //case ItemType.ArmorNone:
        case ItemType.Armor_01:
        case ItemType.Armor_02:
            return CharacterEquipment.EquipSlot.Armor;
        //case ItemType.HelmetNone:
        case ItemType.Helmet_01:
            return CharacterEquipment.EquipSlot.Helmet;
        //case ItemType.SwordNone:
        case ItemType.Sword_01:
        case ItemType.Sword_02:
        case ItemType.Sword_iron:
        case ItemType.Sword_broken:
        case ItemType.Sword_gold:
            return CharacterEquipment.EquipSlot.Weapon;
        } */
    }

}

