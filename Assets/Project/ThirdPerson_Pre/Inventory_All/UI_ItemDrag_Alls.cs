using TMPro;
using UnityEngine;
using UnityEngine.UI;

//? gameObject = pfUI_itemDrag se active khi keo pfUI_Item di chuyen tu slot nay sang slot khac

public class UI_ItemDrag_Alls : MonoBehaviour
{
    public static UI_ItemDrag_Alls Instance { get; private set; }

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    
    private Image image;
    private Item item;
    private TextMeshProUGUI amountText;

    private void Awake() {
        Instance = this;
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        image = transform.Find("image").GetComponent<Image>();
        amountText = transform.Find("amountText").GetComponent<TextMeshProUGUI>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();

        Hide();
    }

    private void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }

    public Item GetItem() {
        return item;
    }

    public void SetItem(Item item) {
        this.item = item;
    }
    public void SetItemNull() {
        this.item = null;
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

    public void Show(Item item) {
        gameObject.SetActive(true);

        SetItem(item);
        ////SetSprite(item.GetSprite());
        SetSprite(item.itemScriptableObject.itemSprite);
        SetAmountText(item.amount);
        UpdatePosition();
    }

}
