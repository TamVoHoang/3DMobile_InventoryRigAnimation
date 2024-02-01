using System;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour,IItemHolder
{
    //! gameobject = player
    //? pickup - weapon container - keo tha vao characterEquipment slot - player trang bi

    public enum EquipSlot {
        None,
        Helmet,
        Armor,
        WeaponRifle,
        WeaponPistol
    }
    public event EventHandler OnEquipmentChanged; //! duoc += col 63 UI_chatacterEquipment || tesing Awake () - run SetCharacterEquipment()
    [SerializeField] private Transform activeWeaponSpawnPoint; //? noi se spawn vu khi ra
    [SerializeField] private Transform activePistolSpawnPoint; //? noi se spawn vu khi ra

    [SerializeField] private Transform activeArmorSpawnPoint; //? noi se spawn vu khi ra
    [SerializeField] private Transform activeHelmetSpawnPoint; //? noi se spawn vu khi ra

    private GameObject equipedWeaponTemp_GO;
    private GameObject armorEquipedCurrent;
    private GameObject helmetEquipedCurrent;

    private Item weaponItem;
    private Item weaponPistolItem;
    private Item helmetItem;
    private Item armorItem;


    [SerializeField] private RaycastWeapon gunPrefabRifleTemp_Raycast; //! testing
    [SerializeField] private RaycastWeapon gunPrefabPistolTemp_Raycast; //! testing


    private void Awake() {
        //activeWeaponSpawnPoint = transform.Find("ActiveWeapon");
        activeArmorSpawnPoint = transform.Find("ActiveArmor");
        activeHelmetSpawnPoint = transform.Find("ActiveHelmet");
    }

    public Item GetWeaponRifleItem(){
        Debug.Log("return weaponItem from characterEquipment.cs coll 47");
        return weaponItem;
    }
    public Item GetWeaponPistolItem(){
        Debug.Log("return weaponItem from characterEquipment.cs coll 52");
        return weaponPistolItem;
    }
    public Item SetWeaponItemNull() => weaponItem = null;
    public Item GetHelmetItem() => helmetItem;
    public Item GetArmorItem() => armorItem;

    private void SetBothWeaponItem(Item weaponItem, ref RaycastWeapon raycastWeaponTemp) {
        if (weaponItem != null) {
            weaponItem.SetItemHolder(this);
        }

        //! kich hoat chay UpdateVisual() thong qua delegate col 61 UI_characterEquipment.cs
        //! co dia chi ham de chay la nho vao awke() da kich hoat cho CharacterEquipment
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        //todo xet trang bi loai vu khi cho player. o equipSlot go item ra
        //todo xet currentWeapon tai day khi da biet loai weapon nao co trong player
        // todo tuong tu khi quet duoc vu khi trong activeInventory - instantiate item.prefab - gan currentweapon

        if(weaponItem == null) {
            Debug.Log("weaponItem == null");
            Destroy(raycastWeaponTemp.gameObject);

            if(!ActiveGun.Instance.IsHolstered) {
                ActiveGun.Instance.ToggleActiveWeapon();
            }
            return;
        }

        if(weaponItem != null) {
            Debug.Log("weaponItem != null");
            int weaponSlotIndex = (int)weaponItem.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot; //=0
            Debug.Log("weaponSlotIndex " + weaponSlotIndex);

            raycastWeaponTemp = Instantiate(weaponItem.itemScriptableObject.gunPrefabRaycast, ActiveGun.Instance.weaponSlots[weaponSlotIndex].position,
                ActiveGun.Instance.weaponSlots[weaponSlotIndex].transform.rotation, ActiveGun.Instance.weaponSlots[weaponSlotIndex]);
                
            raycastWeaponTemp.transform.SetParent(ActiveGun.Instance.weaponSlots[weaponSlotIndex], false);

            ActiveGun.Instance.Equip(raycastWeaponTemp);
        }
    }

#region Set GunPrefabRaycast
    private void SetPistolWeaponItem(Item weaponPistolItem) {
        this.weaponPistolItem = weaponPistolItem;
        Debug.Log("checking weaponItem on characterEquipment" + this.weaponPistolItem);

        //? neu muon gan gia tri cho bien gunPrefabPistolTemp_Raycast thong qua ham trung gian thi phai them ref
        SetBothWeaponItem(weaponPistolItem, ref gunPrefabPistolTemp_Raycast);

        /* if (weaponPistolItem != null) {
            weaponPistolItem.SetItemHolder(this);
        }

        //! kich hoat chay UpdateVisual() thong qua delegate col 61 UI_characterEquipment.cs
        //! co dia chi ham de chay la nho vao awke() da kich hoat cho CharacterEquipment
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        //todo xet trang bi loai vu khi cho player. o equipSlot go item ra
        //todo xet currentWeapon tai day khi da biet loai weapon nao co trong player
        // todo tuong tu khi quet duoc vu khi trong activeInventory - instantiate item.prefab - gan currentweapon

        if(weaponPistolItem == null) {
            Debug.Log("weaponItem == null");
            Destroy(gunPrefabPistolTemp_Raycast.gameObject);

            if(!ActiveGun.Instance.IsHolstered) {
                ActiveGun.Instance.ToggleActiveWeapon();
            }
            return;
        }

        if(weaponPistolItem != null) {
            Debug.Log("weaponItem != null");
            int weaponSlotIndex = (int)weaponPistolItem.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot; //=0
            Debug.Log("weaponSlotIndex " + weaponSlotIndex);

            gunPrefabPistolTemp_Raycast = Instantiate(weaponPistolItem.itemScriptableObject.gunPrefabRaycast, ActiveGun.Instance.weaponSlots[weaponSlotIndex].position,
                ActiveGun.Instance.weaponSlots[weaponSlotIndex].transform.rotation, ActiveGun.Instance.weaponSlots[weaponSlotIndex]);
                
            gunPrefabPistolTemp_Raycast.transform.SetParent(ActiveGun.Instance.weaponSlots[weaponSlotIndex], false);

            ActiveGun.Instance.Equip(gunPrefabPistolTemp_Raycast);
        } */
    }

    // col 42 UI_CharacterEquipment.cs goi -> gan loai weapon trong UI_weaponSlot vao this.weaponItem
    private void SetWeaponRifleItem(Item weaponItem) {
        this.weaponItem = weaponItem;
        Debug.Log("checking weaponItem on characterEquipment" + this.weaponItem);

        //? neu muon gan gia tri cho bien gunPrefabRifleTemp_Raycast thong qua ham trung gian thi phai them ref
        SetBothWeaponItem(weaponItem, ref gunPrefabRifleTemp_Raycast);

        /* if (weaponItem != null) {
            weaponItem.SetItemHolder(this);
        }

        //! kich hoat chay UpdateVisual() thong qua delegate col 61 UI_characterEquipment.cs
        //! co dia chi ham de chay la nho vao awke() da kich hoat cho CharacterEquipment
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        //todo xet trang bi loai vu khi cho player. o equipSlot go item ra
        //todo xet currentWeapon tai day khi da biet loai weapon nao co trong player
        // todo tuong tu khi quet duoc vu khi trong activeInventory - instantiate item.prefab - gan currentweapon

        if(weaponItem == null) {
            Debug.Log("weaponItem == null");
            Destroy(gunPrefabRifleTemp_Raycast.gameObject);

            if(!ActiveGun.Instance.IsHolstered) {
                ActiveGun.Instance.ToggleActiveWeapon();
            }
            return;
        }

        if(weaponItem != null) {
            Debug.Log("weaponItem != null");
            int weaponSlotIndex = (int)weaponItem.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot; //=0
            Debug.Log("weaponSlotIndex " + weaponSlotIndex);

            gunPrefabRifleTemp_Raycast = Instantiate(weaponItem.itemScriptableObject.gunPrefabRaycast, ActiveGun.Instance.weaponSlots[weaponSlotIndex].position,
                ActiveGun.Instance.weaponSlots[weaponSlotIndex].transform.rotation, ActiveGun.Instance.weaponSlots[weaponSlotIndex]);
                
            gunPrefabRifleTemp_Raycast.transform.SetParent(ActiveGun.Instance.weaponSlots[weaponSlotIndex], false);

            ActiveGun.Instance.Equip(gunPrefabRifleTemp_Raycast);
        } */
    }
#endregion Set GunPrefabRaycast

    public void SetHelmetItem(Item helmetItem) {
        this.helmetItem = helmetItem;
        if (helmetItem != null) {
            helmetItem.SetItemHolder(this);
        }

        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        if(helmetItem == null) {
            Destroy(helmetEquipedCurrent);
            return;
        }
        if(helmetItem != null) {
            Destroy(helmetEquipedCurrent);
            GameObject helmetToSpawn = helmetItem.GetPrefab();
            helmetEquipedCurrent = Instantiate(helmetToSpawn, activeHelmetSpawnPoint.position, Quaternion.identity);
            activeHelmetSpawnPoint.transform.rotation = Quaternion.Euler(0,0,0);
            helmetEquipedCurrent.transform.parent = activeHelmetSpawnPoint.transform;
        }
    }

    public void SetArmorItem(Item armorItem) {
        this.armorItem = armorItem;

        if (armorItem != null) {
            armorItem.SetItemHolder(this);
        }

        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        if(armorItem == null) {
            Destroy(armorEquipedCurrent);
            return;
        }
        if(armorItem != null) {
            Destroy(armorEquipedCurrent);

            GameObject armorToSpawn = armorItem.GetPrefab();
            armorEquipedCurrent = Instantiate(armorToSpawn, activeArmorSpawnPoint.position, Quaternion.identity);
            activeArmorSpawnPoint.transform.rotation = Quaternion.Euler(0,0,0);
            armorEquipedCurrent.transform.parent = activeArmorSpawnPoint.transform;
        }
    }

    public void EquipItem(Item item) {
        switch (item.GetEquipSlot()) {
        default:
        case EquipSlot.Armor:   SetArmorItem(item);     break;
        case EquipSlot.Helmet:  SetHelmetItem(item);    break;
        case EquipSlot.WeaponRifle:  SetWeaponRifleItem(item);    break;
        case EquipSlot.WeaponPistol:  SetPistolWeaponItem(item);    break;

        }
    }
    public Item GetEquippedItem(EquipSlot equipSlot) {
        switch (equipSlot) {
        default:
        case EquipSlot.Armor:   return GetArmorItem();
        case EquipSlot.Helmet:  return GetHelmetItem();
        case EquipSlot.WeaponRifle:  return GetWeaponRifleItem();
        case EquipSlot.WeaponPistol:  return GetWeaponPistolItem();
        }
    }

    public bool IsEquipSlotEmpty(EquipSlot equipSlot) {
        //kiem tra bien item wepaon co null ko
        return GetEquippedItem(equipSlot) == null; // Nothing currently equipped
    }

    public bool CanEquipItem(EquipSlot equipSlot, Item item) {
        return equipSlot == item.GetEquipSlot(); // Item matches this EquipSlot
    }

#region interface IItemHolder
    public void RemoveItemEquipment(Item item) {
        if (GetWeaponRifleItem() == item)    SetWeaponRifleItem(null);
        if (GetWeaponPistolItem() == item)    SetPistolWeaponItem(null);

        if (GetHelmetItem() == item)    SetHelmetItem(null);
        if (GetArmorItem() == item)     SetArmorItem(null);
    }

    public void AddItemEquipment(Item item) {
        EquipItem(item);
    }

    public bool CanAddItemEquipment() {
        return true;
    }
    
    public void AddItemAfterSliting(Item item) {
        EquipItem(item);
    }

#endregion interface IItemHolder

    //todo 
}
