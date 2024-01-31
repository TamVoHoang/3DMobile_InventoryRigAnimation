using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CharacterEquipmentSlot : MonoBehaviour, IDropHandler
{
    //! gameObject = weaponSlot Equipment (chua loai weaponItem tu duoi weaponInventory)
    
    //? mot su kien ke lam cau noi khi OnDrop se truyen item vua drop -> trigger sang UI_characterequipment.cs
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped; // run 36, 52 UI_CharacterEquipment.cs
    public class OnItemDroppedEventArgs : EventArgs {
        public Item item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        UI_ItemDrag.Instance.Hide();
        
        // keo item tu duoi weaponContainer1 -> tha vao o weaponSlot -> kiem tra la loai gi
        Item item = UI_ItemDrag.Instance.GetItem();

        Debug.Log("weaponslot vua nhan loai Item tu UI_ItemDrag: " + item);

        //! thuc hien action col 20 21 22 UI_CharacterEquipment.cs
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item });
    }

    //todo remove item ben trong o slot ma this.gameobject dang chua
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     if (eventData.clickCount == 2) {
    //         Debug.Log("Double click detected!");
    //     }
    //     Debug.Log("co nhan chuot phai tai equiptSlot");
    //     OnItemPointerRightClicked?.Invoke(this, new OnItemPointerClick {item = null});
        
    // }

    


}
