using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CharacterEquipmentSlot : MonoBehaviour, IDropHandler
{
    //? gameObject = weaponSlot (nhan item tu duoi weaponContainer) -> tiep theo se Instantiate va trang bi
    
    //? mot su kien ke lam cau noi khi OnDrop se truyen item vua drop -> trigger sang UI_characterequipment.cs
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
    public class OnItemDroppedEventArgs : EventArgs {
        public Item item;
    }

    //todo
    //public event EventHandler<OnItemPointerClick> OnItemPointerRightClicked;
    // public class OnItemPointerClick :EventArgs {
    //     public Item item;
    // }
    //todo

    public void OnDrop(PointerEventData eventData)
    {
        UI_ItemDrag.Instance.Hide();
        
        // keo item tu duoi weaponContainer1 -> tha vao o weaponSlot -> kiem tra la loai gi
        Item item = UI_ItemDrag.Instance.GetItem();

        //todo item duoc dat vao o weaponSlot thi duoc isEquiped = true TU THEM
        //item.SetIsEquipedItem(true);

        Debug.Log("weaponslot vua nhan loai Item:" + item);

        //! thuc hien action col 20 21 22 UI_CharacterEquipment.cs
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item });
    }


    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     if (eventData.clickCount == 2) {
    //         Debug.Log("Double click detected!");
    //     }
    //     Debug.Log("co nhan chuot phai tai equiptSlot");
    //     OnItemPointerRightClicked?.Invoke(this, new OnItemPointerClick {item = null});
        
    // }

    


}
