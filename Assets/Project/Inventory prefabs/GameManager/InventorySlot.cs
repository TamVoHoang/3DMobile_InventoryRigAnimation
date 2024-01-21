using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //todo gameobject = cai o nho chua item khi hien len
    //todo an hien icon cua itemPickup
    //todo o co chuc nang => button use item || Clear() || remove()
    [SerializeField] public Image icon; // component trong inventory slot
    Item item;

    [SerializeField] Button removeButton;

    public void AddItem(Item newItem) {
        item = newItem;

        icon.sprite = item.icon; // lay icon trong Scriptale gan vao image trong o inventory slot
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    //todo khi nhat nut x goi RemoveItem() inventory.cs => xoa khoi lít items
    public void OnRemoveButton() {
        Inventory.Instance.RemoveItemOutList(item);
    }
    public void UseItem() {
        if(item != null) {
            item.Use();
        }
    }
}
