using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using CodeMonkey.Utils;

public class UI_Item : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler 
{
    //todo gameObject la doi tuong item nam trong itemSlot pfUI_item prefabs

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI itemsNameText;
    [SerializeField] private TextMeshProUGUI powerText;


    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Item item;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        //! neu de tu dong tim kiem khi doi tuong pfUI_item duoc sinh ra thi se ko the tim thay 2 bien image va amountText
        //image = transform.Find("image").GetComponent<Image>();//! testing
        //amountText = transform.Find("amountText").GetComponent<TextMeshProUGUI>(); //! testing
    }
    private void Update() {
        //todo dung khi col 162 UI_inventory su dung click de Use || nen o day phai dung MouseMidleDunc
        /* SpitItem();
        RemoveAndDrop(); */
    }

    public void OnBeginDrag(PointerEventData eventData) {
        canvasGroup.alpha = .5f; // lam mo hinh anh
        canvasGroup.blocksRaycasts = false;
        UI_ItemDrag.Instance.Show(item);
    }

    public void OnDrag(PointerEventData eventData) {
        ////rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f; // lam hinh anh sang tro lai
        canvasGroup.blocksRaycasts = true; // todo true o day de con co the drag sau khi trigger
        UI_ItemDrag.Instance.Hide();

        //? keo tha item out and drop world position
        if(EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        
        DropItemOutWorld();
    }

    //? KHI NHAP CHUOT phai VAO O VAT PHAM SE CHIA DOI
    public void OnPointerDown(PointerEventData eventData) {
        /* if (eventData.button == PointerEventData.InputButton.Left) 
        {
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
        } */
    }

    public void SetSprite(Sprite sprite) {
        Debug.Log(sprite);
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

    void SetItemName(string itemName) {
        if (itemName != null) {
            this.itemsNameText.text = itemName.ToString();
        }
    }

    void SetPowerText(float power) {
        if(power > 0)
            powerText.text = "Power: " + power.ToString();
        else
            powerText.text = "";
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void SetItem(Item item) {
        this.item = item;
        //SetSprite(item.GetSprite()); //! lay sprite ben ngoai ItemAsset - co the dung ok
        SetSprite(item.itemScriptableObject.itemSprite); // lay sprite ben trong ItemScriptableObject

        SetAmountText(item.amount);

        SetItemName(item.itemScriptableObject.itemName);

        SetPowerText(item.itemScriptableObject.damage);
    }

    public void SetItem_OnCraftingUI(Item item) {
        this.item = item;
        //SetSprite(item.GetSprite()); //! lay sprite ben ngoai ItemAsset - co the dung ok
        SetSprite(item.itemScriptableObject.itemSprite); // lay sprite ben trong ItemScriptableObject

        SetAmountText(-1);

        SetItemName("");

        SetPowerText(-1);
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
        Debug.Log("midleClick Split ben UI_item.cs");
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

    //? rightmouse click de remove thay vi run 177 ui_inventory.cs
    private void RemoveAndDrop(){
        rectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
            Debug.Log("rightClick ben UI_item.cs");
            Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount};
            item.GetItemHolder().RemoveItemEquipment(item);
            ItemWorld3D.DropItem(PlayerController.Instance.GetPosition(),duplicateItem);
        };
    }
    
    //? detect tin hieu de dropitem UI_item.cs
    private void DropItemOutWorld() {
        Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = item.amount};
            item.GetItemHolder().RemoveItemEquipment(item);
            ItemWorld3D.DropItem(PlayerController.Instance.GetPosition(),duplicateItem);
    }

}
