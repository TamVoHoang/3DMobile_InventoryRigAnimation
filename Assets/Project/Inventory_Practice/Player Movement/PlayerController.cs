
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private int itemSlotAmount = 10;
    //[SerializeField] private int itemSlotAmount_scroll = 10;

    [SerializeField] Inventory inventory; // se duoc Awake() goi de khoi tao new inventory
    [SerializeField] Inventory inventoryEquipment;
    [SerializeField] Inventory inventory_scroll;
    [SerializeField] UI_Inventory ui_Inventory; // dung de goi ham SetInventoy()
    private Rigidbody2D rb;
    protected override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

        inventory_scroll = new Inventory(UseItemScroll);
        inventory = new Inventory(UseItem); // => khoi tao Inventory() => itemList
        inventoryEquipment = new Inventory(UseItemEquipment, itemSlotAmount);
        //ui_Inventory.SetPlayerPos(this); // uiInventory lay vi tri player //todo-> tesing.cs chy ham nay
    }
    private void Start() {

        //ui_Inventory.SetInventory(inventory); // bien inventory in doi tuong UI_Inventory duoi canvas da duoc gan gia tri
        // ui_Inventory.SetInventoryEquip(inventoryEquip); //todo-> tesing.cs chy ham nay

        //? dung static itemWorld goi phuong thuc Spawnworld ra vat phan world
        //ItemWorld.SpawnItemWorld(new Vector3(3,3), new Item {itemType = Item.ItemType.HealthPotion, amount =1});
        // ItemWorld.SpawnItemWorld(new Vector3(-3,3), new Item {itemType = Item.ItemType.ManaPotion, amount =1});
        // ItemWorld.SpawnItemWorld(new Vector3(0,-3), new Item {itemType = Item.ItemType.Sword, amount =1});
        // ItemWorld.SpawnItemWorld(new Vector3(0,3), new Item {itemType = Item.ItemType.Medkit, amount =1});
    }

    private void Update() {
        
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
            case Item.ItemType.HealthPotion:
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
            }
        }
    }

    private void UseItemEquipment(Item item) {
        switch (item.itemScriptableObject.itemType)
        {
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
            itemWorld.GetItem().itemScriptableObject.itemType != Item.ItemType.Gold && itemWorld.GetItem().itemScriptableObject.itemType != Item.ItemType.Iron)
        {
            // lay ve doi tuong item Item.cs ( game object = vat pham pfItemWord vua louch)
            inventory.AddItem(itemWorld.GetItem()); //todo add item vat pham vao trong itemsList => tang them 1 vat pham
            itemWorld.DestroySelf();
        }


        //! pickup kieu Item nhung neu la sword thi add vao ban cam ung
        ItemWorld itemWorlEquipment = other.GetComponent<ItemWorld>();

        // && itemWorlEquip.GetItem().itemType == Item.ItemType.Sword_01
        if(itemWorlEquipment != null  && !itemWorld.GetItem().IsStackable() || 
            itemWorld.GetItem().itemScriptableObject.itemType == Item.ItemType.Gold || itemWorld.GetItem().itemScriptableObject.itemType == Item.ItemType.Iron) // && itemWorld.GetItem().IsStackable()
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

            EquipOrAddToInventoryEquipmentList(itemWorld3DEquipment);
        }
    }
    private void EquipOrAddToInventoryEquipmentList(ItemWorld3D itemWorld3DEquipment) {
        // todo neu cham && tren tay ko co PREFAB loai vua cham => equip -- else ko lam gi het
        // todo neu cham && loai vua cham khac loai tren tay => add vao balo
        var itemWorld3DType = itemWorld3DEquipment.GetItem().itemScriptableObject.itemType;
        CharacterEquipment characterEquipment = GetComponent<CharacterEquipment>();

        if((itemWorld3DType == Item.ItemType.GunSMG3D_01 && !characterEquipment.GetPrefab_RifleTemp) 
            || itemWorld3DType == Item.ItemType.GunPistol3D_01 && !characterEquipment.GetPrefab_PistolTemp) {
            characterEquipment.EquipItem(itemWorld3DEquipment.GetItem());
            itemWorld3DEquipment.DestroySelf();
        }
        else {
            inventoryEquipment.AddItemEquipment(itemWorld3DEquipment.GetItem());
            itemWorld3DEquipment.DestroySelf();
        }
    }

    private void OnTriggerExit(Collider other) {
        ItemWorld3D itemWorld3DEquipment = other.GetComponent<ItemWorld3D>();
        if(itemWorld3DEquipment != null) {
            Debug.Log("KO cham item3D");

            //? tra lai itemWorld3d tu scrollViewInventory
            // foreach (var item in inventory_scroll.GetItemList()) {
            //     Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount };
            //     inventory_scroll.RemoveItem(item);
            //     ItemWorld3D.DropItem(GetPosition(),duplicateItem);
            // }

            //? clear scrollViewInventory khi ??
            //inventory_scroll.ClearInventory_Scroll(inventory_scroll.GetItemList()); 
        }
    }

    //todo
}
