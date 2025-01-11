using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CodeMonkey.Utils;

public class UI_Invnetory_Alls : MonoBehaviour
{
    [SerializeField] private Transform pfUI_Item_Alls; // prefab = cai ruot (hien thi hinh anh va thong so cua item) in cai slot array

    Inventory inventoryAlls; // testingAlls.cs se gan cai new InventoryAlls duoc khoi tao trong PlayerController.cs cho bien nay

    [SerializeField] private Transform itemSlotContainer1_Alls; // transform chua toan bo slot
    [SerializeField] Transform itemSlotTemplate1_Alls; // prefab = cai o slot trong se duoc array ra

    PlayerController_Alls playerController_Alls; // testing se xet cho doi tuong nay

    // so slot in row and distance between them
    [SerializeField] private int ItemAmountOnRow = 5;
    [SerializeField] float itemSlotCellSize = 100f;

    public void SetInventoryAlls(Inventory inventoryAlls) {
        this.inventoryAlls = inventoryAlls;
        inventoryAlls.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshIventoryAlls();
    }


    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshIventoryAlls();
    }

    private void RefreshIventoryAlls()
    {
        Debug.Log("Start run inventory equipment");
        //! tranh tao ra tranform khi duyet Inventory || itemList se tao ra qua nhiu child in itemSlotContainer
        foreach (Transform child in itemSlotContainer1_Alls) {
            if (child == itemSlotTemplate1_Alls) continue;
            Destroy(child.gameObject);
        }
        
        //? toa do itemSlot ben trong bang vat pham
        int x = 0;
        int y = 0;
        //itemSlotCellSize = 80f;
        foreach (Inventory.InventorySlot inventorySlot in inventoryAlls.GetInventorySlotArray()) // cu moi vat pham ben trong listItems trong Item.cs
        {
            Item item = inventorySlot.GetItem(); //? kiem tra item tai tung slot trong foreach

            if(item != null) Debug.Log("item just add to UI= " + item.itemScriptableObject.itemType);
            
            //? tao ra transform itemSlotTemplate trong hierachy
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate1_Alls, itemSlotContainer1_Alls).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true); // hien cac o chua
            
            //? xet vi tri cho o vat pham UI
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -0.5f * y * itemSlotCellSize);

            //? equip chen ngang khi co tin hieu mouse click
            if (!inventorySlot.IsEmpty()) {
                // Not Empty, has Item
                Transform uiItemTransform = Instantiate(pfUI_Item_Alls, itemSlotContainer1_Alls);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition + new Vector2(-160f, 0); //! move to left + new Vector2(-160f, 0)
                UI_Item_Alls uiItem_Alls = uiItemTransform.GetComponent<UI_Item_Alls>();
                uiItem_Alls.SetItem(item);
                
                //? REMOVE VU KHI BANG BUTTON_UI.cs DUOC GAN TORNG DOI TUONG pfUI_Item
                //todo neu o day DUNG MouseClick de use() -> ui_item phai dung MouseMidle de split()
                // todo neu o day KO DUNG MouseClick de use() -> ui_Item co the dung DoubleClick de slit()

                //! todo ko dung duoc doubleclik ben ui_item de Split() khi tai day su dung MouseClick 
                //! Ly do: khi tai day co tin hieu mouseClick se use() va khong cho tin hieu doubleClick ben UI_item de split()
                uiItemTransform.GetComponent<Button_UI>().ClickFunc= () => {
                    // Use item
                    Debug.Log("single click use amount");
                    inventoryAlls.UseItemEquipment(item);
                };

                uiItemTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                    Debug.Log("co nhan chuot phai tai o slot co item");
                    Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount};

                    ////inventoryAlls.RemoveItemEquipment(item);
                    ////ItemWorld3D.DropItem(playerController_Alls.GetPosition(),duplicateItem); // ko co drop nen ko can xet player's position

                };
            }

            Inventory.InventorySlot tmpInventorySlot = inventorySlot;
            UI_ItemSlot_Alls uiItemSlot_Alls = itemSlotRectTransform.GetComponent<UI_ItemSlot_Alls>();

            uiItemSlot_Alls.SetOnDropAction(() => {
                Debug.Log("Start run OnDropAction");
                // Dropped on this UI Item Slot
                Item draggedItem = UI_ItemDrag_Alls.Instance.GetItem();
                //todo neu KO dung ScrollView thi chay doan nay
                //// draggedItem.RemoveFromItemHolder();
                //// inventoryEquipment.AddItemEquipment(draggedItem, tmpInventorySlot);

                //todo neu dung ScrollView thi chay doan nay
                Debug.Log(draggedItem);
                if(draggedItem != null) {
                    UI_ItemDrag_Alls.Instance.SetItemNull();
                    Debug.Log("dragItem1 = " + draggedItem);
                    draggedItem.RemoveFromItemHolder();
                    inventoryAlls.AddItemEquipment(draggedItem, tmpInventorySlot);
                }
                });


            // offset x, y vi tri o vat pham tren bang vat pham
            x++;
            if (x >= ItemAmountOnRow) {
                x = 0;
                y++;
            }
        }
        Debug.Log(" so luong vat pham trong itemList Alls = " + inventoryAlls.GetItemList().Count);
    }


    //todo
}
