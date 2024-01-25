
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Inventory inventory; // se duoc Awake() goi de khoi tao new inventory
    [SerializeField] Inventory inven;
    [SerializeField] Inventory inventoryEquipment;
    [SerializeField] UI_Inventory ui_Inventory; // dung de goi ham SetInventoy()
    //[SerializeField] private float moveSpeed =1f;
    //private Vector2 movement;
    private Rigidbody2D rb;
    //private PlayerControls playerControls;


    private void Awake() {
        //playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();

        inventory = new Inventory(UseItem); // => khoi tao Inventory() => itemList
        inventoryEquipment = new Inventory(UseItemEquipment, 12);

        //ui_Inventory.SetPlayerPos(this); // uiInventory lay vi tri player //todo-> tesing.cs chy ham nay
    }
    private void Start() {

        ui_Inventory.SetInventory(inventory); // bien inventory in doi tuong UI_Inventory duoi canvas da duoc gan gia tri
        // ui_Inventory.SetInventoryEquip(inventoryEquip); //todo-> tesing.cs chy ham nay

        //? dung static itemWorld goi phuong thuc Spawnworld ra vat phan world
        //ItemWorld.SpawnItemWorld(new Vector3(3,3), new Item {itemType = Item.ItemType.HealthPotion, amount =1});
        // ItemWorld.SpawnItemWorld(new Vector3(-3,3), new Item {itemType = Item.ItemType.ManaPotion, amount =1});
        // ItemWorld.SpawnItemWorld(new Vector3(0,-3), new Item {itemType = Item.ItemType.Sword, amount =1});
        // ItemWorld.SpawnItemWorld(new Vector3(0,3), new Item {itemType = Item.ItemType.Medkit, amount =1});
    }

    private void OnEnable() {
        //playerControls.Enable();
    }

    void Update()
    {
        //PlayerInput();

    }

    private void FixedUpdate() {
        //Move();
    }
    public Vector3 GetPosition() {
        return transform.position;
    }

    //todo lay tinh hieu input de suy ra vector2 movement
    private void PlayerInput(){
        //movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move(){
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
    public Inventory GetInventory() {
        return inventoryEquipment;
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
            inventoryEquipment.AddItemEquipment(itemWorld3DEquipment.GetItem());
            itemWorld3DEquipment.DestroySelf();
        }
    }

    //todo
}
