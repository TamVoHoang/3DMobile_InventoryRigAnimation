using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour,IItemHolder
{
    //! gameobject = player
    //? pickup - weapon container - keo tha vao characterEquipment slot - player trang bi

    public enum EquipSlot {
        None,
        Helmet,
        Armor,
        Weapon
    }
    public event EventHandler OnEquipmentChanged; //! duoc += col 63 UI_chatacterEquipment || tesing Awake () - run SetCharacterEquipment()
    private PlayerController playerController;
    [SerializeField] private Transform activeWeaponSpawnPoint; //? noi se spawn vu khi ra
    [SerializeField] private Transform activeArmorSpawnPoint; //? noi se spawn vu khi ra
    [SerializeField] private Transform activeHelmetSpawnPoint; //? noi se spawn vu khi ra


    private GameObject weaponEquipedCurrent;
    private GameObject armorEquipedCurrent;
    private GameObject helmetEquipedCurrent;


    private Item weaponItem;
    private Item helmetItem;
    private Item armorItem;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
        activeWeaponSpawnPoint = transform.Find("ActiveWeapon");
        activeArmorSpawnPoint = transform.Find("ActiveArmor");
        activeHelmetSpawnPoint = transform.Find("ActiveHelmet");

    }

    private void Update() {
        // Debug.Log(helmetItem);
        // Debug.Log(armorItem);
        // Debug.Log(weaponItem);
    }

    public Item GetWeaponItem(){
        return weaponItem;
    }
    public Item SetWeaponItemNull() {
        return null;
    }

    public Item GetHelmetItem(){
        return helmetItem;
    }

    public Item GetArmorItem(){
        return armorItem;
    }


    // col 42 UI_CharacterEquipment.cs goi -> gan loai weapontrong weaponSlot vao this.weaponItem
    public void SetWeaponItem(Item weaponItem) {
        this.weaponItem = weaponItem;
        if (weaponItem != null) {
            weaponItem.SetItemHolder(this);
        } 
        else {
            // Unequipped weapon
            //player.SetEquipment(Item.ItemType.SwordNone);
        }

        //! kich hoat chay UpdateVisual() thong qua delegate col 61 UI_characterEquipment.cs
        //! co dia chi ham de chay la nho vao awke() da kich hoat cho CharacterEquipment
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        //todo xet trang bi loai vu khi cho player
        //todo xet currentWeapon tai day khi da biet loai weapon nao co trong player
        // todo tuong tu khi quet duoc vu khi trong activeInventory - instantiate item.prefab - gan currentweapon

        if(weaponItem == null) {
            Destroy(weaponEquipedCurrent);
            return;
        }
        if(weaponItem != null) {
            Destroy(weaponEquipedCurrent);
            GameObject weaponToSpawn = weaponItem.GetPrefab();
            weaponEquipedCurrent = Instantiate(weaponToSpawn, activeWeaponSpawnPoint.position, Quaternion.identity);
            activeWeaponSpawnPoint.transform.rotation = Quaternion.Euler(0,0,0);
            weaponEquipedCurrent.transform.parent = activeWeaponSpawnPoint.transform;
        }

    }
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

    public void TryEquipItem(EquipSlot equipSlot, Item item) {
        // if (equipSlot == item.GetEquipSlot()) {
        //     // Item matches this EquipSlot
        //     switch (equipSlot) {
        //     default:
        //     case EquipSlot.Armor:   SetArmorItem(item);     break;
        //     case EquipSlot.Helmet:  SetHelmetItem(item);    break;
        //     case EquipSlot.Weapon:  SetWeaponItem(item);    break;
        //     }
        // }
    }
    public void EquipItem(Item item) {
        switch (item.GetEquipSlot()) {
        default:
        case EquipSlot.Armor:   SetArmorItem(item);     break;
        case EquipSlot.Helmet:  SetHelmetItem(item);    break;
        case EquipSlot.Weapon:  SetWeaponItem(item);    break;
        }
    }
    public Item GetEquippedItem(EquipSlot equipSlot) {
        switch (equipSlot) {
        default:
        case EquipSlot.Armor:   return GetArmorItem();
        case EquipSlot.Helmet:  return GetHelmetItem();
        case EquipSlot.Weapon:  return GetWeaponItem();
        }
    }

    public bool IsEquipSlotEmpty(EquipSlot equipSlot) {
        return GetEquippedItem(equipSlot) == null; // Nothing currently equipped
    }

    public bool CanEquipItem(EquipSlot equipSlot, Item item) {
        return equipSlot == item.GetEquipSlot(); // Item matches this EquipSlot
    }

    public void RemoveItemEquipment(Item item) {
        if (GetWeaponItem() == item)    SetWeaponItem(null);
        if (GetHelmetItem() == item)    SetHelmetItem(null);
        if (GetArmorItem() == item)     SetArmorItem(null);
    }

    public void AddItemEquipment(Item item) {
        EquipItem(item);
    }

    public bool CanAddItemEquipment() {
        return true;
    }

    
    public void AddItemAfterSliting(Item item)
    {
        EquipItem(item);
    }
}
