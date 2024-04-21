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
        WeaponRifle,
        WeaponPistol,
        WeaponSword
    }
    public event EventHandler OnEquipmentChanged; //! duoc += col 63 UI_chatacterEquipment || tesing Awake () - run SetCharacterEquipment()
    // [SerializeField] private Transform activeWeaponSpawnPoint; //? noi se spawn vu khi ra
    // [SerializeField] private Transform activePistolSpawnPoint; //? noi se spawn vu khi ra
    // [SerializeField] private Transform activeSwordSpawnPoint; //? noi se spawn vu khi ra


    [SerializeField] private Transform activeArmorSpawnPoint; //? noi se spawn armor
    [SerializeField] private Transform activeHelmetSpawnPoint; //? noi se spawn helmet
    // [SerializeField] private Transform iSword_SpawnPoint; // vi tri sinh ra cay kiem iSword

    private GameObject armorEquipedCurrent;
    private GameObject helmetEquipedCurrent;


    private Item weaponItem;
    private Item weaponPistolItem;
    private Item weaponSwordItem;
    private Item helmetItem;
    private Item armorItem;

    private ActiveGun activeGun;
    private ActiveSword activeSword;
    private ActiveWeapon activeWeapon;
    private PlayerHealth playerHealth;


    [SerializeField] private RaycastWeapon gunPrefabRifleTemp_Raycast; //! testing
    [SerializeField] private RaycastWeapon gunPrefabPistolTemp_Raycast; //! testing
    [SerializeField] private HandSwordWeapon swordPrefabTemp_HandSword; //! testing
    [SerializeField] private GameObject I_SwordPrefabTemp; // game object cua cay kiem

    public RaycastWeapon GetPrefab_RifleTemp {get{return gunPrefabRifleTemp_Raycast;}}
    public RaycastWeapon GetPrefab_PistolTemp {get{return gunPrefabPistolTemp_Raycast;}}
    public HandSwordWeapon GetPrefab_SwordTemp {get{return swordPrefabTemp_HandSword;}}
    public GameObject GetI_SwordPrefabTemp {get{return I_SwordPrefabTemp;}}

    //TODO TEST LOAT BO VU KHI RA KHOI PLAYER KHI DEATH
    public void RemoveItemOutWorld_CharacterEquipment(Item item) {
        Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount};
            item.GetItemHolder().RemoveItemEquipment(item);
            ItemWorld3D.DropItem(PlayerController.Instance.GetPosition(),duplicateItem);
    }

    //TODO TEST LOAT BO VU KHI RA KHOI PLAYER KHI DEATH

    private void Awake() {
        activeGun = GetComponent<ActiveGun>();
        activeSword = GetComponent<ActiveSword>();
        activeWeapon = GetComponent<ActiveWeapon>();
        activeArmorSpawnPoint = transform.Find("ActiveArmor");
        activeHelmetSpawnPoint = transform.Find("ActiveHelmet");
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update() {
        // if(Input.GetKey(KeyCode.P)) {
        //     Debug.Log("co nhan nut P");
        //     swordPrefabTemp_HandSword.transform.SetParent(swordHolster_Point, false);
        //     swordPrefabTemp_HandSword.transform.SetParent(swordHolster_Point, true);
        // }

        //todo khi player die thi goi ham de out item khoi nguoi
        if(playerHealth.IsDead) {
            if(weaponItem != null) RemoveItemOutWorld_CharacterEquipment(weaponItem);
            if(weaponPistolItem != null) RemoveItemOutWorld_CharacterEquipment(weaponPistolItem);
            if(weaponSwordItem != null) RemoveItemOutWorld_CharacterEquipment(weaponSwordItem);
            if(helmetItem != null) RemoveItemOutWorld_CharacterEquipment(helmetItem);
            if(armorItem != null) RemoveItemOutWorld_CharacterEquipment(armorItem);

            
        }
    }


    public Item GetWeaponRifleItem(){
        Debug.Log("return weaponItem from characterEquipment.cs coll 47");
        return weaponItem;
    }
    public Item GetWeaponPistolItem(){
        Debug.Log("return weaponItem from characterEquipment.cs coll 52");
        return weaponPistolItem;
    }
    public Item GetWeaponSwordItem() {
        Debug.Log("return weaponItem from characterEquipment.cs coll 62");
        return weaponSwordItem;
    }
    public Item SetWeaponItemNull() => weaponItem = null;
    public Item GetHelmetItem() => helmetItem;
    public Item GetArmorItem() => armorItem;
    
    //todo set sword
    private void SetSword_WeaponItem(Item sword) {
        this.weaponSwordItem = sword;
        if (sword != null) {
            sword.SetItemHolder(this);
        }

        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);

        // todo set sword to activeSword
        /* if(sword == null) {
            Destroy(swordPrefabTemp_HandSword.gameObject);
            if(!activeSword.IsHolstered_Sword &&
                (int)swordPrefabTemp_HandSword.GetComponent<HandSwordWeapon>().swordSlot == activeSword.GetActiveSwordIndex){
                activeSword.ToggleActiveSword();
            }
            return;
        }
        if(sword != null) {
            int weaponSlotIndex = (int)weaponSwordItem.itemScriptableObject.handSwordPrefab.GetComponent<HandSwordWeapon>().swordSlot; //=1
            Debug.Log("weaponSlotIndex " + weaponSlotIndex);
            if(!activeGun.IsHolstered) activeGun.ToggleActiveWeapon(); //? neu co bat ki sung nao dang trang bi theo bien isHolstered thi toggle het
            StartCoroutine(DelaytimeToSpawnSword(sword, weaponSlotIndex));
        } */

        // todo set sword to ActiveWeapon Interface
        if(sword == null) {
            
            activeWeapon.SetDefaultWeapon(); // de su dung tay khong
            if(!activeWeapon.IsHolstered_Sword){
                activeWeapon.ToggleActiveSword(); //? xet isHoslter = true khi da ko con trang bi
            }

            Destroy(I_SwordPrefabTemp); // I_SwordPrefabTemp.GetComponent<ISword>()
            return;
        }
        if(sword != null) {
            if(!activeGun.IsHolstered) activeGun.ToggleActiveWeapon();

            int weaponSlotIndex = (int)weaponSwordItem.itemScriptableObject.pfWeaponInterface.GetComponent<ISword>().swordSlot; //=0
            StartCoroutine(DelayTimeToSpawn_WeaponInterface(sword, weaponSlotIndex));
        }
    }
    IEnumerator DelayTimeToSpawn_WeaponInterface(Item iSword, int weaponSlotIndex) {
        yield return new WaitForSeconds(0.6f);
        I_SwordPrefabTemp = Instantiate(iSword.itemScriptableObject.pfWeaponInterface, activeWeapon.swordSlots[weaponSlotIndex].position,
                activeWeapon.swordSlots[weaponSlotIndex].transform.rotation, activeWeapon.swordSlots[weaponSlotIndex].transform);
        activeWeapon.NewWeapon(I_SwordPrefabTemp.GetComponent<MonoBehaviour>()); // khi new weapon da set active luon
    }
    IEnumerator DelaytimeToSpawnSword(Item sword, int weaponSlotIndex) {
        yield return new WaitForSeconds(1f);
        swordPrefabTemp_HandSword = Instantiate(sword.itemScriptableObject.handSwordPrefab, activeSword.swordSlots[weaponSlotIndex].position,
                activeSword.swordSlots[weaponSlotIndex].transform.rotation, activeSword.swordSlots[weaponSlotIndex]);
            activeSword.EquipSword(swordPrefabTemp_HandSword);
    }

#region Set GunPrefabRaycast
    private void SetBothWeaponItem(Item weaponItem, ref RaycastWeapon raycastWeaponTemp) {
        if (weaponItem != null) {
            weaponItem.SetItemHolder(this); //! item se SetItemHolder() tai day, de biet class nao se goi ham trong Interface
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

            //? neu loai raycastWeaponTemp co weaponSlot tra ve kieu (int) == activeWeaponIndex
            //? loai sung dang cam tren tay bi emty - thi thuc hien hanh dong toggleActiveWeapon
            //? neu sung dang trong tui bi empty - thi KO thuc hien hanh dong toggleActiveWeapon
            if(!activeGun.IsHolstered &&
                (int)raycastWeaponTemp.GetComponent<RaycastWeapon>().weaponSlot == activeGun.GetActiveWeaponIndex){
                activeGun.ToggleActiveWeapon();
            }
            
            return;
        }

        if(weaponItem != null) {
            Debug.Log("weaponItem != null");
            if(!activeWeapon.IsHolstered_Sword) activeWeapon.ToggleActiveSword(); //? neu sword dang trang bi thi toggle het

            int weaponSlotIndex = (int)weaponItem.itemScriptableObject.gunPrefabRaycast.GetComponent<RaycastWeapon>().weaponSlot; //=0
            Debug.Log("weaponSlotIndex " + weaponSlotIndex);

            raycastWeaponTemp = Instantiate(weaponItem.itemScriptableObject.gunPrefabRaycast, activeGun.weaponSlots[weaponSlotIndex].position,
                activeGun.weaponSlots[weaponSlotIndex].transform.rotation, activeGun.weaponSlots[weaponSlotIndex]);
            raycastWeaponTemp.transform.SetParent(activeGun.weaponSlots[weaponSlotIndex], false);
            
            //activeGun.Equip(raycastWeaponTemp); //co the dung OK
            StartCoroutine(DelayTimeToSpawnEquipGuns(0.5f, raycastWeaponTemp));
            
        }
    }
    IEnumerator DelayTimeToSpawnEquipGuns(float time, RaycastWeapon raycastWeaponTemp) {
        yield return new WaitForSeconds(time);

        activeGun.Equip(raycastWeaponTemp);
    }

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
        case EquipSlot.Armor:           SetArmorItem(item);             break;
        case EquipSlot.Helmet:          SetHelmetItem(item);            break;
        case EquipSlot.WeaponRifle:     SetWeaponRifleItem(item);       break;
        case EquipSlot.WeaponPistol:    SetPistolWeaponItem(item);      break;
        case EquipSlot.WeaponSword:     SetSword_WeaponItem(item);      break;
        }
    }
    public Item GetEquippedItem(EquipSlot equipSlot) {
        switch (equipSlot) {
        default:
        case EquipSlot.Armor:           return GetArmorItem();
        case EquipSlot.Helmet:          return GetHelmetItem();
        case EquipSlot.WeaponRifle:     return GetWeaponRifleItem();
        case EquipSlot.WeaponPistol:    return GetWeaponPistolItem();
        case EquipSlot.WeaponSword:     return GetWeaponSwordItem();
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
        if (GetWeaponRifleItem() == item)       SetWeaponRifleItem(null);
        if (GetWeaponPistolItem() == item)      SetPistolWeaponItem(null);
        if (GetWeaponSwordItem() == item)       SetSword_WeaponItem(null);


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
