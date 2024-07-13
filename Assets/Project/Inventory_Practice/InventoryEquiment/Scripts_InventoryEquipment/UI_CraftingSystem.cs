using System;
using UnityEngine;

public class UI_CraftingSystem : MonoBehaviour
{
    [SerializeField] private Transform pfUI_Item;
    private Transform[,] slotTransformArray; //? quet doi tuong con cua gridContainer tao ma tran
    private Transform outputSlotTransform;
    private Transform itemContainer;
    private Transform gridContainer;
    private CraftingSystem craftingSystem; //? doi tuong se duoc khoi tao thong qua coll41 - duoc goi o Awake() testing.cs
    
    private void Awake() {
        gridContainer = transform.Find("gridContainer");
        itemContainer = transform.Find("itemContainer");

    }
    private void Start() {
        
        slotTransformArray = new Transform[CraftingSystem.GRID_SIZE, CraftingSystem.GRID_SIZE];

        for (int x = 0; x < CraftingSystem.GRID_SIZE; x++) {
            for (int y = 0; y < CraftingSystem.GRID_SIZE; y++) {
                slotTransformArray[x, y] = gridContainer.Find("grid_" + x + "_" + y);
                
                UI_CraftingItemSlot craftingItemSlot = slotTransformArray[x, y].GetComponent<UI_CraftingItemSlot>();// moi gridSLot da duoc gan UI_CraftingItemSlot.cs
                craftingItemSlot.SetXY(x, y); // gan gia tri x,y (vi tri hang cot cua grid) cho bien x , y UI_CraftingItemSlot.cs
                craftingItemSlot.OnItemDropped += UI_CraftingSystem_OnItemDropped;
            }
        }
        outputSlotTransform = transform.Find("outputSlot");


        //tao item hien thi grid truc tiep tai day de test ham Create
        // CreateItem(0, 0, new Item { itemType = Item.ItemType.Gold });
        // CreateItem(1, 2, new Item { itemType = Item.ItemType.Iron });
        // CreateItemOutput(new Item { itemType = Item.ItemType.Sword_iron });
    }

    //todo hien thi item len gridSLot khi dragdrop tu inventoryEquipment vao trong gridSlot cua this.gameobject (UI_CraftingSystem)
    public void SetCraftingSystem(CraftingSystem craftingSystem) //? coll 29 testing khoi tao va gan doi tuong lop CraftingSystem
    {
        this.craftingSystem = craftingSystem;
        craftingSystem.OnGridChanged += CraftingSystem_OnGridChanged;

        UpdateVisual(); // khi testing Awake() de kiem tra ma tran itemArray cua craftingSystem.cs
    }

    private void CraftingSystem_OnGridChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }


    //? col 29 this.cs dang ky su kien || khi co UI_ItemDrag tha vao grid_?_? col 23 UI_craftingItemSlot.cs || 
    //? -> col 33 UI_craftingItemSlot.cs -> coll 50 this.cs
    private void UI_CraftingSystem_OnItemDropped(object sender, UI_CraftingItemSlot.OnItemDroppedEventArgs e)
    {
        Debug.Log($"{e.item} col_{e.x} row_{e.y}"); // co vat pham tha vao grid_ cu the
        craftingSystem.TryAddItem(e.item, e.x, e.y); // kieu bool kiem tra xem co Set Get vao trong ma tran itemArray[]
    }

    private void UpdateVisual() {
        // Clear old items
        foreach (Transform child in itemContainer) {
            Destroy(child.gameObject);
        }

        // Cycle through grid and spawn items
        for (int x = 0; x < CraftingSystem.GRID_SIZE; x++) {
            for (int y = 0; y < CraftingSystem.GRID_SIZE; y++) {
                if (!craftingSystem.IsEmpty(x, y)) {
                    CreateItem(x, y, craftingSystem.GetItem(x, y));
                }
            }
        }

        if (craftingSystem.GetOutputItem() != null) {
            CreateItemOutput(craftingSystem.GetOutputItem());
        }
    }

    //todo nhan item, x,y -> hien thi len grid_ cu the this.gameobject (UI_CraftingSystem) hierachy
    private void CreateItem(int x, int y, Item item) {
        Transform itemTransform = Instantiate(pfUI_Item, itemContainer); //? transform cua hierachy itemContainer

        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>(); //? vi tri trong rect Transform
        itemRectTransform.anchoredPosition = slotTransformArray[x, y].GetComponent<RectTransform>().anchoredPosition;//? gan vi tri rectTrasform cua grid -> recTransform pfUI_item
        
        //itemTransform.GetComponent<UI_Item>().SetItem(item); //pfUI_item chua UI_Item.cs nen co the Getcomponent
        itemTransform.GetComponent<UI_Item>().SetItem_OnCraftingUI(item); // chi hien thi hinh anh len UI_crafting - ko quan tam pfUI || pfUI Info

    }

    private void CreateItemOutput(Item item) {
        Transform itemTransform = Instantiate(pfUI_Item, itemContainer);

        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        itemRectTransform.anchoredPosition = outputSlotTransform.GetComponent<RectTransform>().anchoredPosition;

        itemTransform.localScale = Vector3.one * 1.5f; // phong to anh len
        //itemTransform.GetComponent<UI_Item>().SetItem(item); // hien thi hinh anh tu trong .cs ra image transform
        itemTransform.GetComponent<UI_Item>().SetItem_OnCraftingUI(item); // chi hien thi hinh anh len UI_crafting - ko quan tam pfUI || pfUI Info
    }

}
