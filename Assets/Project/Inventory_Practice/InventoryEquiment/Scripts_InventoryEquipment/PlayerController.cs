using CodeMonkey.Utils;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using System.Collections;
public class PlayerController : Singleton<PlayerController>, IData_InventoryPersistence
{
    [SerializeField] private int itemSlotAmount = 10;
    ////[SerializeField] private int itemSlotAmount_scroll = 10;

    [SerializeField] Inventory inventory; // se duoc Awake() goi de khoi tao new inventory
    [SerializeField] Inventory inventoryEquipment;
    [SerializeField] Inventory inventory_scroll;    // dung de chua nhung thu chuan bi pickup - notUse
    
    private ActiveGun activeGun;
    private Health playerHealth;
    private CharacterEquipment characterEquipment;

    //TODO SAVE AND LOAD LIST<ITEM>
    public List<Item> listItemsToSaveJson;
    [SerializeField] ItemScriptableObject SMG01_3D_01;

    //TODO SAVE AND LOAD LIST<ITEM>

    [SerializeField] bool isPicked = false;
    
    protected override void Awake() {
        base.Awake();
        
        inventory_scroll = new Inventory(UseItemScroll);                        // inventory ao, pickup
        inventory = new Inventory(UseItem);                                     // => khoi tao Inventory() => itemList (vat pham co the chong len nhau)
        inventoryEquipment = new Inventory(UseItemEquipment, itemSlotAmount);   // inventoryEquipment - nhung thu !istackable
        
        activeGun = GetComponent<ActiveGun>();
        playerHealth = GetComponent<PlayerHealth>();
        characterEquipment = GetComponent<CharacterEquipment>();

        isPicked = false;
    }
    private void Start() {
        //? dung static itemWorld goi phuong thuc Spawnworld ra vat phan world
        /* ItemWorld.SpawnItemWorld(new Vector3(3,3), new Item {itemType = Item.ItemType.HealthPotion, amount =1});
        ItemWorld.SpawnItemWorld(new Vector3(-3,3), new Item {itemType = Item.ItemType.ManaPotion, amount =1});
        ItemWorld.SpawnItemWorld(new Vector3(0,-3), new Item {itemType = Item.ItemType.Sword, amount =1});
        ItemWorld.SpawnItemWorld(new Vector3(0,3), new Item {itemType = Item.ItemType.Medkit, amount =1}); */
    }

    public Vector3 GetPosition() {
        return transform.position + new Vector3(0f, 0f, 0f);
    }

    public Inventory GetInventoryEquipment() {
        return inventoryEquipment;
    }
    public Inventory GetInventory() {
        return inventory;
    }
    public Inventory GetInventory_scroll() {
        return inventory_scroll;
    }

    private void UseItemScroll(Item item){
        Debug.Log("su dung item trong itemScroll");
        inventoryEquipment.AddItemEquipment(item);
        inventory_scroll.RemoveItem(item);
    }

    private void UseItem(Item item) {
        switch (item.itemScriptableObject.itemType)
        {
            /* case Item.ItemType.HealthPotion:
            {
                Debug.Log("su dung HealthPotion");
                inventory.RemoveItem(new Item {itemScriptableObject = item.itemScriptableObject, amount =1});
                break;
            }
            case Item.ItemType.ManaPotion:
            {
                Debug.Log("su dung ManaPotion");
                inventory.RemoveItem(new Item {itemScriptableObject = item.itemScriptableObject, amount =1});
                break;
            }
            case Item.ItemType.Medkit:
            {
                Debug.Log("su dung Medkit");
                inventory.RemoveItem(new Item {itemScriptableObject = item.itemScriptableObject, amount =1});
                break;
            }
            case Item.ItemType.Coin:
            {
                Debug.Log("su dung Coin");
                inventory.RemoveItem(new Item {itemScriptableObject = item.itemScriptableObject, amount =1});
                break;
            } */
            case Item.ItemType.IMagPistol3D_01:
            {
                Debug.Log("su dung IMagPistol3D_01");
                var weapon = activeGun.GetActiveWeapon(); // phai co sung tren tay thi moi su dung
                if(activeGun && weapon) {
                    activeGun.RefillAmmo(item.itemScriptableObject.clipAmount);
                    inventory.RemoveItem(new Item {itemScriptableObject = item.itemScriptableObject, amount = 1});
                }
                break;
            }
            case Item.ItemType.IHealthPickup3D_01:
            {
                Debug.Log("su dung IHealthPickupPrefab3D_01");
                if(playerHealth && playerHealth.IsLowHealth()) {
                    playerHealth.Heal(item.itemScriptableObject.healthAmout);
                    inventory.RemoveItem(new Item {itemScriptableObject = item.itemScriptableObject, amount = 1});
                }
                break;
            }
        }
    }

    private void UseItemEquipment(Item item) {
        switch (item.itemScriptableObject.itemType) {
            case Item.ItemType.Coin:
            {
                Debug.Log("su dung Coin");
                inventoryEquipment.RemoveItemEquip(new Item {itemScriptableObject = item.itemScriptableObject, amount =1});
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        //! pickup kieu Item nhung ko phai la vu khi
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();

        //itemWorld.GetItem().itemType != Item.ItemType.Sword_01
        if(itemWorld != null && itemWorld.GetItem().IsStackable() && 
            itemWorld.GetItem().itemScriptableObject.itemType != Item.ItemType.Gold && 
            itemWorld.GetItem().itemScriptableObject.itemType != Item.ItemType.Iron)
        {
            // lay ve doi tuong item Item.cs ( game object = vat pham pfItemWord vua louch)
            inventory.AddItem(itemWorld.GetItem()); //todo add item vat pham vao trong itemsList => tang them 1 vat pham
            itemWorld.DestroySelf();
        }


        //! pickup kieu Item nhung neu la sword thi add vao ban cam ung
        ItemWorld itemWorlEquipment = other.GetComponent<ItemWorld>();

        // && itemWorlEquip.GetItem().itemType == Item.ItemType.Sword_01
        if(itemWorlEquipment != null  && !itemWorld.GetItem().IsStackable() || 
            itemWorld.GetItem().itemScriptableObject.itemType == Item.ItemType.Gold || 
            itemWorld.GetItem().itemScriptableObject.itemType == Item.ItemType.Iron) // && itemWorld.GetItem().IsStackable()
        {
            inventoryEquipment.AddItemEquipment(itemWorlEquipment.GetItem());
            itemWorlEquipment.DestroySelf();
        }

    }
    
    private void OnTriggerEnter(Collider other) {
        //! pickup kieu ItemWorld3D chi lay vu khi vao ban cam ung
        ItemWorld3D itemWorld3DEquipment = other.GetComponent<ItemWorld3D>();
        if(itemWorld3DEquipment != null) {
            Debug.Log("co cham item3D");
            
            //? add itemWorld3D vao trong scrollViewInventory pickup
            //inventory_scroll.AddItem(itemWorld3DEquipment.GetItem());

            //? add thang truc tiep vao trong weaponEquipment_UI
            //inventoryEquipment.AddItemEquipment(itemWorld3DEquipment.GetItem());

            //? xoa itemworld3d sau khi bo vao inventory
            //itemWorld3DEquipment.DestroySelf();

            if(itemWorld3DEquipment.GetItem().IsStackable()) {
                inventory.AddItem(itemWorld3DEquipment.GetItem());
                itemWorld3DEquipment.DestroySelf();
            }
            else {
                //EquipOrAddToInventoryEquipmentList(itemWorld3DEquipment); //! KHI PICKIP DO VAT GAN LIEN TUC BO OVER ANIMATION => 1 TAY 2 VU KHI
                
                if(!isPicked) {
                    isPicked = true;
                    StartCoroutine(PickupDelay_Countine(0.7f, itemWorld3DEquipment));
                } else {
                    inventoryEquipment.AddItemEquipment(itemWorld3DEquipment.GetItem()); // trong thoi gian cho isPickup set false -> add thang vao kho
                }
                
            }
        }
    }

    IEnumerator PickupDelay_Countine(float delayTime, ItemWorld3D itemWorld3DEquipment) {
        EquipOrAddToInventoryEquipmentList(itemWorld3DEquipment);
        yield return new WaitForSeconds(delayTime);
        isPicked = false; // cho phep lay tiep
    }

    private void EquipOrAddToInventoryEquipmentList(ItemWorld3D itemWorld3DEquipment) {
        // todo neu cham && tren tay ko co PREFAB loai vua cham => equip -- else ko lam gi het
        // todo neu cham && loai vua cham khac loai tren tay => add vao balo
        var itemWorld3DType = itemWorld3DEquipment.GetItem().itemScriptableObject.itemType;
        CharacterEquipment characterEquipment = GetComponent<CharacterEquipment>();

        if((itemWorld3DType == Item.ItemType.GunSMG3D_01 && !characterEquipment.GetPrefab_RifleTemp) || 
            (itemWorld3DType == Item.ItemType.GunPistol3D_01 && !characterEquipment.GetPrefab_PistolTemp) || 
            (itemWorld3DType == Item.ItemType.ISword_Red_01 ||itemWorld3DType == Item.ItemType.ISword_Green_02) && 
            !characterEquipment.GetI_SwordPrefabTemp) 
        {
            characterEquipment.EquipItem(itemWorld3DEquipment.GetItem());
            itemWorld3DEquipment.DestroySelf();
        }
        else {
            inventoryEquipment.AddItemEquipment(itemWorld3DEquipment.GetItem());
            itemWorld3DEquipment.DestroySelf();
        }
    }

    //? test thu neu tao i inventory pickup trung gian - inventoryPickup -> inventory Equipment - slot equipment
    private void OnTriggerExit(Collider other) {
        ItemWorld3D itemWorld3DEquipment = other.GetComponent<ItemWorld3D>();
        if(itemWorld3DEquipment != null) {
            //? tra lai itemWorld3d tu scrollViewInventory
            //? clear scrollViewInventory khi ??
            /* foreach (var item in inventory_scroll.GetItemList()) {
                Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount };
                inventory_scroll.RemoveItem(item);
                ItemWorld3D.DropItem(GetPosition(),duplicateItem);
            }
            inventory_scroll.ClearInventory_Scroll(inventory_scroll.GetItemList());  */
        }
    }

    #region Idata_InventoryPersistence
    public void Load_InventoryData(InventoryJson inventoryJson)
    {
        foreach (var item in inventoryJson.itemsListJson)
        {
            // set SO in item
            item.itemScriptableObject = item.GetScriptableObject(); // using itemType -> get SO from ItemAsset.cs

            if(item.IsStackable()) this.inventory.AddItem(item);
            if(!item.IsStackable()) this.inventoryEquipment.AddItemEquipment(item);
        }
    }

    // ham chuyen doi item trong inventory -> thanh item (chi co itemType va amount) nhu khi sign up
    Item Check(Item _item) {
        return new Item {itemType = _item.itemScriptableObject.itemType, amount = _item.amount};
    }

    public void Save_InventoryData(ref InventoryJson inventoryJson) {
        inventoryJson.itemsListJson.Clear();    // xoa list itemsListJson REASON dang co gia tri khi khoi tao luc SignUp
        //? save inventory item IsStackable().
        foreach (var item in this.inventory.GetItemList()) {
            bool isUnique = true;
            foreach (var uniqueItem in inventoryJson.itemsListJson) {
                if(ArePropertiesEqual(item, uniqueItem)) {
                    isUnique = false;
                    break;
                }
            }

            // chi lau item type va amount de luu vao itemsListJson
            if(isUnique) inventoryJson.itemsListJson.Add(Check(item));
        }

        //? save inventory equip !IsStackable() - dragdrop.
        foreach (var item in this.inventoryEquipment.GetItemList()) {
            inventoryJson.itemsListJson.Add(Check(item));
        }

        foreach (var item in characterEquipment.GetEquippedItemsList)
        {
            inventoryJson.itemsListJson.Add(Check(item));
        }
    }
    #endregion Idata_InventoryPersistence

    private bool ArePropertiesEqual(Item item1, Item item2) {
        // Compare properties here, return true if they are equal, otherwise false
        return item1.itemType == item2.itemType &&
                item1.amount == item2.amount &&
                item1.IsStackable(item1.itemType) && item2.IsStackable(item2.itemType);
    }

    //todo
}
