/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using CodeMonkey.Utils;

public class UI_Item : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler 
{

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image image;
    private Item item;
    private TextMeshProUGUI amountText;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        image = transform.Find("image").GetComponent<Image>();

        amountText = transform.Find("amountText").GetComponent<TextMeshProUGUI>();
    }
    private void Update() {
        //todo dung khi col 162 UI_inventory su dung click de Use || nen o day phai dung MouseMidleDunc
        //SpitItem();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
        UI_ItemDrag.Instance.Show(item);
    }

    public void OnDrag(PointerEventData eventData) {
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true; // todo true o day de con co the drag sau khi trigger
        UI_ItemDrag.Instance.Hide();
    }

    //? KHI NHAP CHUOT phai VAO O VAT PHAM SE CHIA DOI
    public void OnPointerDown(PointerEventData eventData) {
        // if (eventData.button == PointerEventData.InputButton.Left) 
        // {
        //     if (item != null) {
        //         // Has item
        //         if (item.IsStackable()) {
        //             // Is Stackable
        //             if (item.amount > 1) {
        //                 // More than 1
        //                 if (item.GetItemHolder().CanAddItemEquipment()) {
        //                     // Can split
        //                     int splitAmount = Mathf.FloorToInt(item.amount / 2f);
        //                     item.amount -= splitAmount;
        //                     Debug.Log(item.amount +"/"+ splitAmount);
        //                     Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = splitAmount };
        //                     //item.GetItemHolder().AddItemEquipment(duplicateItem);
        //                     duplicateItem.isSplited = true;
        //                     item.GetItemHolder().AddItemAfterSliting(duplicateItem);
        //                     if(duplicateItem != null) Debug.Log(item.amount +"/"+ splitAmount);
        //                 }
        //             }
        //         }
        //     }
        // }
    }

    public void SetSprite(Sprite sprite) {
        image.sprite = sprite;
    }

    public void SetAmountText(int amount) {
        if (amount <= 1) {
            amountText.text = "";
        } else {
            // More than 1
            amountText.text = amount.ToString();
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void SetItem(Item item) {
        this.item = item;
        SetSprite(item.GetSprite());
        SetAmountText(item.amount);
    }

    public Item TryGetType() {
        return item;
    }

    //todo double click dung IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) {
            Debug.Log("doubleClick Split ben UI_item.cs");
            if (item != null) {
                // Has item
                if (item.IsStackable()) {
                    // Is Stackable
                    if (item.amount > 1) {
                        // More than 1
                        if (item.GetItemHolder().CanAddItemEquipment()) {
                            // Can split
                            int splitAmount = Mathf.FloorToInt(item.amount / 2f);
                            item.amount -= splitAmount;
                            Debug.Log(item.amount +"/"+ splitAmount);
                            Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = splitAmount };
                            //item.GetItemHolder().AddItemEquipment(duplicateItem); //ko dung
                            duplicateItem.isSplited = true;
                            item.GetItemHolder().AddItemAfterSliting(duplicateItem);
                            if(duplicateItem != null) Debug.Log(item.amount +"/"+ splitAmount);
                        }
                    }
                }
            }
        }
    }

    //todo CodeMonkey.Utils; mouseClick split vat pham 
    //todo dung khi col 162 UI_inventory su dung click de Use || nen o day phai dung MouseMidleDunc || ko the dung doubleClick
    private void SpitItem() {
        rectTransform.GetComponent<Button_UI>().MouseMiddleClickFunc = () => {
        if (item != null) {
                // Has item
                if (item.IsStackable()) {
                    // Is Stackable
                    if (item.amount > 1) {
                        // More than 1
                        if (item.GetItemHolder().CanAddItemEquipment()) {
                            // Can split
                            int splitAmount = Mathf.FloorToInt(item.amount / 2f);
                            item.amount -= splitAmount;
                            Debug.Log(item.amount +"/"+ splitAmount);
                            Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = splitAmount };
                            //item.GetItemHolder().AddItemEquipment(duplicateItem);
                            duplicateItem.isSplited = true;
                            item.GetItemHolder().AddItemAfterSliting(duplicateItem);
                            if(duplicateItem != null) Debug.Log(item.amount +"/"+ splitAmount);
                        }
                    }
                }
            }
        };
    }

    
}
