using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftingItemSlot : MonoBehaviour, IDropHandler
{
    //todo gameobject = cac grid con cua grid container ||
    //todo slotTransformArray coll 20 UI_craftingSystem.cs

    //? dang ky su kien cho tung girid col 28 UI_CraftingSystem.cs
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
    public class OnItemDroppedEventArgs : EventArgs {
        public Item item;
        public int x;
        public int y;
    }

    //? nhan gia tri khi khoi tao slotStranformArray coll 26 UI_CraftingSystem.cs
    private int x;
    private int y;

    //todo khi grid[?,?] nhan tin hieu Drop loai item vao ben trong no se run o day
    public void OnDrop(PointerEventData eventData)
    {
        UI_ItemDrag.Instance.Hide(); //? doi tuong an hien loai item khi loai item duoc dragDrop
        
        // keo item tu duoi weaponContainer1 -> tha vao ma tran grid -> kiem tra la loai gi
        Item item = UI_ItemDrag.Instance.GetItem();

        Debug.Log($"grid_{x}_{y} vua nhan loai item: {item}");

        //! thuc hien action col 20 21 22 UI_CharacterEquipment.cs
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item,  x = x, y = y });
    }

    public void SetXY(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
