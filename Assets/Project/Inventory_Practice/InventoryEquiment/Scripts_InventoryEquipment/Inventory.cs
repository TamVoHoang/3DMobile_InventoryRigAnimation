using System.Collections.Generic;
using UnityEngine;
using System;


public class Inventory : IItemHolder
{
    //? gameobject ko duoc gan vao Gameobject cu the
    //? duoc goi khi player Awake()

    public event EventHandler OnItemListChanged;//
    private List<Item> itemList; //? chau cac phan tu kieu Item(itemType, amount)
    private Action<Item> useItemAction; //? input vao la Item ko co tra ve khi use
    private Action<Item> useItemEquipmentAction; //? input vao la Item ko co tra ve khi use

    public InventorySlot[] inventorySlotArray; // int [] n || n = new int [5]

    // TODO HAM KHOI TAO CHO INVENTORY SIGNUP
    //todo HAM KHOI TAO CHO EQUIPMENT
    public Inventory(Action<Item> useItemEquipmentAction, int inventorySlotCount)
    {
        this.useItemEquipmentAction = useItemEquipmentAction;
        itemList = new List<Item>();

        inventorySlotArray = new InventorySlot[inventorySlotCount];
        for (int i = 0; i < inventorySlotCount; i++) {
            inventorySlotArray[i] = new InventorySlot(i);

        }
        //? tao moi item va add vao itemList kieu Item weapon
        /* AddItemEquipment(new Item {itemScriptableObject = new ItemScriptableObject() {
                itemType = Item.ItemType.Sword_01 },
                amount = 1}); */
    }

    //TODO HAM KHOI TOA CHO INVENTORY ITEMS + INVENTORY TEMP TRUOC KHI QUYET DINH CO ADD VAO KHONG
    //?player Awake() - khoi toa Inventoy() -> khoi toa itemList() kieu Item - KHI TEST 1
    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        Debug.Log("player -> khoi tao Inventory -> itemList -> add");
        itemList = new List<Item>();

        //? tao moi item va add vao itemList kieu Item item
        /* AddItem(new Item {itemScriptableObject = new ItemScriptableObject() {
            itemType = Item.ItemType.GunSMG3D_01 },
            amount = 1});

        PrinItemList(); */
    }

    //todo ham tra ve gia tri itemlist | RefreshIventoryItems() UI_Inventory.cs call
    public List<Item> GetItemList() {
        return itemList;
    }

    private void PrinItemList() {
        Debug.Log("itemListCount = "+ itemList.Count);
        foreach (var item in itemList)
        {
            Debug.Log($"name: {item.itemScriptableObject.itemType}_{item.IsStackable()} _ add vao ItemList");
        }
    }

    //todo duyet itemList de kiem tra loai vu khi ben trong

    internal void UseItem(Item item) => useItemAction(item);
    internal void UseItemEquipment(Item item) => useItemEquipmentAction(item);

    //todo ClearInventory_Scroll(List<Item> itemList)
    public void ClearInventory_Scroll(List<Item> itemList) {
        itemList.Clear();
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    //todo ADD ITEM VAO TRONG ITEMLIST CO THE + VAO NHAU col 11
    public void AddItem(Item item) {
        if(item.IsStackable()) {
            //? neu cung loai thi se tang amount
            //? so sanh Item chuan vi add voi cac item da co san xem xo cung loai hay khong
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if(inventoryItem.itemScriptableObject.itemType == item.itemScriptableObject.itemType) {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if(!itemAlreadyInInventory) {
                itemList.Add(item);
            }
        }else {
            itemList.Add(item);
        }
        if(item.amount > 0) // neu lon hon 0 thi update UI
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    //todo ADD ITEM VAO TRONG BANG EQUIPMENT || co the istack || co the split
    public void AddItemEquipment(Item item) //? ad interface ok
    {
        //! doan code dung nhung chi add vao ma khong stackable
        // itemList.Add(item);
        // item.SetItemHolder(this);
        // GetEmptyInventorySlot().SetItem(item);
        // OnItemListChanged?.Invoke(this, EventArgs.Empty);

        //! KHI PLAYER CHAM ITEMWORLD ADD VAO VA CO XET DIEU KIEU STACKABLE
        if(item.IsStackable()) {
            //? neu cung loai thi se tang amount
            //? so sanh Item chuan vi add voi cac item da co san xem xo cung loai hay khong
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if(inventoryItem.itemScriptableObject.itemType == item.itemScriptableObject.itemType && !inventoryItem.isSplited) {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if(!itemAlreadyInInventory) {
                itemList.Add(item);
                item.SetItemHolder(this);
                GetEmptyInventorySlot().SetItem(item);
            }
        }else {
            itemList.Add(item);
            item.SetItemHolder(this);
            GetEmptyInventorySlot().SetItem(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

    }
    //? SAU KHI SLIT COL 116 UI_ITEM.CS SE CO DUPLICATE VA LEN BANG CONTAINAR1 
    //? THEM 1 ITEM(ISPLITED = TRUE) TRANH ADD VAO CUNG LOAI CO SAN
    public void AddItemAfterSliting(Item item) {
        itemList.Add(item);
        item.SetItemHolder(this);
        GetEmptyInventorySlot().SetItem(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    //todo add vao col 262 UI_inventory.cs
    public void AddItemEquipment(Item item, InventorySlot inventorySlot) {
        
        itemList.Add(item);
        item.SetItemHolder(this);
        inventorySlot.SetItem(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItemEquipment(Item item)
    {
        GetInventorySlotWithItem(item).RemoveItem();
        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanAddItemEquipment()
    {
        return GetEmptyInventorySlot() != null;
    }

    internal void RemoveItem(Item item)
    {
        if (item.IsStackable()) {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemScriptableObject.itemType == item.itemScriptableObject.itemType) {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0) {
                itemList.Remove(itemInInventory);
            }
        } else {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        //Debug.Log(itemList.Count);
    }

    //todo DUNG DE REMOVE AMOUNT CUA ITEM KHI SU DUNG VAT PHAM WEAPONsLOT
    internal void RemoveItemEquip(Item item)
    {
        if (item.IsStackable()) {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemScriptableObject.itemType == item.itemScriptableObject.itemType) {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0) {
                GetInventorySlotWithItem(itemInInventory).RemoveItem();
                itemList.Remove(itemInInventory);
            }
        } else {
            GetInventorySlotWithItem(item).RemoveItem();
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

#region InventorySlot
    public InventorySlot[] GetInventorySlotArray() {
        return inventorySlotArray;
    }

    public class InventorySlot {

        private int index;
        private Item item;

        public int InventoryGetIndex(){
            return index;
        }
        public InventorySlot(int index) {
            this.index = index;
        }

        public Item GetItem() {
            return item;
        }

        public void SetItem(Item item) {
            this.item = item;
        }

        public void RemoveItem() {
            item = null;
        }

        public bool IsEmpty() {
            return item == null;
        }

    }
    public InventorySlot GetEmptyInventorySlot() {
        foreach (InventorySlot inventorySlot in inventorySlotArray) {
            if (inventorySlot.IsEmpty()) {
                return inventorySlot;
            }
        }

        Debug.LogError("Cannot find an empty InventorySlot!");
        return null;
    }

    public InventorySlot GetInventorySlotWithItem(Item item) {
        foreach (InventorySlot inventorySlot in inventorySlotArray) {
            if (inventorySlot.GetItem() == item) {
                return inventorySlot;
            }
        }

        Debug.LogError("Cannot find Item " + item + " in a InventorySlot!");
        return null;
    }
#endregion InventorySlot

}
