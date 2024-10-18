using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CodeMonkey.Utils;


//todo Gameobject = duoi tuong nam duoi Canvas Inventory
public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Transform pfUI_Item; // loai co info va loai ko co info
    
    [SerializeField] private Inventory inventory; // se duoc gan vao khi SetInventory is called
    [SerializeField] private Inventory inventoryEquipment;
    [SerializeField] private Inventory inventory_scroll; //!tesitng pickup
    
    //? 3 transform container and templet
    [SerializeField] private Transform itemSlotContainer; // parent folder - noi chua cac item Slot - notEquippedItems
    [SerializeField] private Transform itemSlotContainer1; // parent folder - noi chua cac item Slot - equippedItems - drag drop
    [SerializeField] private Transform itemSlotContainerPickup_scroll; //!tesitng pickup

    [SerializeField] private Transform itemSlotTemplate; // vi tri cua item se hien len (o vat pham) nam torng bang vat pham
    [SerializeField] private Transform itemSlotTemplate1; // vi tri cua item se hien len (o vat pham) nam torng bang vat pham
    [SerializeField] private Transform itemSlotTemplatePickup_scroll; //!tesitng pickup


    private PlayerController playerController;

    // kich thuoc moi slot chua item image | tang giam tuy vao inventory UI
    [SerializeField] float itemSlotCellSize = 100f;
    [SerializeField] private int ItemsAmountOnRow = 5;
    [SerializeField] private int ItemEquipedAmountOnRow = 1;


    // Not Using -> dung cho inventory pickup list
    private int ItemAmountOnRow_scroll = 2;
    float itemSlotCellSize_scroll = 100f;
    
    private void Awake() {
        //! ITEMSLOTCONTAINER PHAI GOI DAU TIEN NEU KO SE KO CO CHO DE ITEMSLOTTEMPLET INSTANTIATE COL 85 AND 147
        /* itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotContainer1 = transform.Find("itemSlotContainer1");

        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        itemSlotTemplate1 = itemSlotContainer1.Find("itemSlotTemplate1"); */

        itemSlotTemplate.gameObject.SetActive(false);
        itemSlotTemplate1.gameObject.SetActive(false);
        itemSlotTemplatePickup_scroll.gameObject.SetActive(false);
    }

    public void SetPlayerPos(PlayerController playerController) {
        this.playerController = playerController;
    }

    //? gan inventory khi player Awake() -> this.inventory
    // inventory show before deciding pickup or not _ not Using at now
    public void SetInventoryScroll(Inventory inventory_scroll) {
        this.inventory_scroll = inventory_scroll;
        inventory_scroll.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItemPickup_scroll();
    }

    // inventory hold items isStackable - not equip on player
    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshIventoryItems();
    }

    // inventory hold equipped items - equip on Player - drag drop
    public void SetInventoryEquipment(Inventory inventoryEquipment) {
        this.inventoryEquipment = inventoryEquipment;
        inventoryEquipment.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshIventoryEquipment();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItemPickup_scroll();//! testing hien bang khi chan item vat ki
        RefreshIventoryItems();
        RefreshIventoryEquipment();
    }

    //? duyet itemList ben trong list itemList | thong qua GetItemList() Inventory.cs
    //todo tao ra transform itemSlotTemplate (o vat pham) trong folder cha itemSlotContainer
    //? sap xep vi tri UI o vat pham ben trong bang vat pham
    private void RefreshInventoryItemPickup_scroll() {
        Debug.Log("Start run inventory scroll");
        //! tranh tao ra tranform khi duyet Inventory || itemList se tao ra qua nhiu child in itemSlotContainer
        foreach (Transform child in itemSlotContainerPickup_scroll) {
            if (child == itemSlotTemplatePickup_scroll) continue;
            Destroy(child.gameObject);
        }
        
        //? toa do itemSlot ben trong bang vat pham
        int x = 0;
        int y = 0;

        foreach (Item item in inventory_scroll.GetItemList()) // cu moi vat pham ben trong listItems trong Item.cs
        {
            //? tao ra transform itemSlotTemplate trong hierachy
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplatePickup_scroll, itemSlotContainerPickup_scroll).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            //? sau khi add Button_UI codeMonkey su dung mouse left right de se dung or return
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                inventory_scroll.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                // Drop item
                Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount }; // khi bi mat item khi drop and add
                inventory_scroll.RemoveItem(item);
                ItemWorld3D.DropItem(playerController.GetPosition(),duplicateItem);
            };

            //? xet vi tri cho o vat pham UI
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize_scroll, -y * itemSlotCellSize_scroll);


            //? thay doi hinh anh hien thi cua tung o vat pham 
            //Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            Image image = itemSlotRectTransform.GetChild(2).GetComponent<Image>();
            image.sprite = item.GetSprite();

            //? hien amount cua item tren bang UIsau khi add them vao > 1
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1) {
                uiText.SetText(item.amount.ToString());
            } else {
                uiText.SetText(""); // neu la cai dau tien lum vao thi ko hien so 1
            }


            // offset x, y vi tri o vat pham tren bang vat pham
            x++;
            if (x >= ItemAmountOnRow_scroll) {
                x = 0;
                y++;
            }
        }
        Debug.Log(" so luong vat pham trong itemList_scroll = " + inventory_scroll.GetItemList().Count);
    }
    
    private void RefreshIventoryItems() {
        Debug.Log("Start run inventory items");
        //! tranh tao ra tranform khi duyet Inventory || itemList se tao ra qua nhiu child in itemSlotContainer
        foreach (Transform child in itemSlotContainer) {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        
        //? toa do itemSlot ben trong bang vat pham
        int x = 0;
        int y = 0;

        foreach (Item item in inventory.GetItemList()) // cu moi vat pham ben trong listItems trong Item.cs
        {
            //Debug.Log("index_ = " + (int)item.itemType + " type_: " + item.itemType );

            //? tao ra transform itemSlotTemplate trong hierachy
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            //? sau khi add Button_UI codeMonkey su dung mouse left right de se dung or return

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                // Drop item
                Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount }; // khi bi mat item khi drop and add
                inventory.RemoveItem(item);
                ItemWorld.DropItem(playerController.GetPosition(),duplicateItem);
            };

            //? xet vi tri cho o vat pham UI
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);


            //? thay doi hinh anh hien thi cua tung o vat pham NEU > 0
            ////Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            
            Image image = itemSlotRectTransform.GetChild(2).GetComponent<Image>();
            //image.sprite = item.GetSprite();

            //! testing not using sprite from ItemAsset in scene - using Sprite in ScriptableObject
            image.sprite = item.itemScriptableObject.itemSprite;

            //? hien amount cua item tren bang UIsau khi add them vao > 1
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1) {
                uiText.SetText(item.amount.ToString());
            } else {
                uiText.SetText(""); // neu la cai dau tien lum vao thi ko hien so 1
            }


            // offset x, y vi tri o vat pham tren bang vat pham
            x++;
            if (x >= 5) {
                x = 0;
                y++;
            }
        }
        Debug.Log(" so luong vat pham trong itemList item = " + inventory.GetItemList().Count);
    }
    
    private void RefreshIventoryEquipment() {
        Debug.Log("Start run inventory equipment");
        //! tranh tao ra tranform khi duyet Inventory || itemList se tao ra qua nhiu child in itemSlotContainer
        foreach (Transform child in itemSlotContainer1) {
            if (child == itemSlotTemplate1) continue;
            Destroy(child.gameObject);
        }
        
        //? toa do itemSlot ben trong bang vat pham
        int x = 0;
        int y = 0;
        ////itemSlotCellSize = 80f;
        foreach (Inventory.InventorySlot inventorySlot in inventoryEquipment.GetInventorySlotArray()) // cu moi vat pham ben trong listItems trong Item.cs
        {
            Item item = inventorySlot.GetItem(); //? kiem tra item tai tung slot trong foreach

            if(item != null) Debug.Log("item just add to UI= " + item.itemScriptableObject.itemType);
            
            //? tao ra transform itemSlotTemplate trong hierachy
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate1, itemSlotContainer1).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true); // hien cac o chua
            
            //? xet vi tri cho o vat pham UI
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize * 0.9f);

            //? equip chen ngang khi co tin hieu mouse click
            if (!inventorySlot.IsEmpty()) {
                // Not Empty, has Item => thi hien thi item do thong qua UI_Item
                Transform uiItemTransform = Instantiate(pfUI_Item, itemSlotContainer1);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                uiItem.SetItem(item);
                
                //? REMOVE VU KHI BANG BUTTON_UI.cs DUOC GAN TORNG DOI TUONG pfUI_Item
                //  todo neu o day DUNG MouseClick de use() -> ui_item phai dung MouseMidle de split()
                //  todo neu o day KO DUNG MouseClick de use() -> ui_Item co the dung DoubleClick de slit()
                // todo mobile hieu click = touch = doublick

                //! todo ko dung duoc doubleclik ben ui_item de Split() khi tai day su dung MouseClick 
                //! Ly do: khi tai day co tin hieu mouseClick se use() va khong cho tin hieu doubleClick ben UI_item de split()
                uiItemTransform.GetComponent<Button_UI>().ClickFunc= () => {
                    // Use item
                    Debug.Log("single click use amount");
                    inventoryEquipment.UseItemEquipment(item);
                };

                uiItemTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                    Debug.Log("co nhan chuot phai tai o slot co item");
                    Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount};

                    inventoryEquipment.RemoveItemEquipment(item);
                    ItemWorld3D.DropItem(playerController.GetPosition(),duplicateItem);

                };
            }

            Inventory.InventorySlot tmpInventorySlot = inventorySlot;
            UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();

            uiItemSlot.SetOnDropAction(() => {
                Debug.Log("Start run OnDropAction khi da drop xuong duoc slot");
                // Dropped on this UI Item Slot
                Item draggedItem = UI_ItemDrag.Instance.GetItem();
                //todo neu KO dung ScrollView thi chay doan nay
                /* draggedItem.RemoveFromItemHolder();
                inventoryEquipment.AddItemEquipment(draggedItem, tmpInventorySlot); */

                //todo neu dung ScrollView thi chay doan nay
                Debug.Log(draggedItem);
                if(draggedItem != null) {
                    UI_ItemDrag.Instance.SetItemNull();
                    Debug.Log("dragItem1 = " + draggedItem);
                    draggedItem.RemoveFromItemHolder();
                    inventoryEquipment.AddItemEquipment(draggedItem, tmpInventorySlot);
                }
                });

            // offset x, y vi tri o vat pham tren bang vat pham
            x++;
            if (x >= ItemEquipedAmountOnRow) {
                x = 0;
                y++;
            }
        }
        Debug.Log(" so luong vat pham trong itemList equip = " + inventoryEquipment.GetItemList().Count);
    }

    //todo
}