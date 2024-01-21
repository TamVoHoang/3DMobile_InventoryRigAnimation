using UnityEngine;


[CreateAssetMenu(fileName ="New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Virable Item")]
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    //todo nut nhan chon vat pham inventorySlot.cs goi
    public virtual void Use()
    {
        Debug.Log("using at Use() SOb " + name);
    }

    //todo phuong thuc duoc goi khi Use() Equipment.cs
    //todo xoa equipment : Item ra hoi list Items ( Iventory.cs coll11)
    protected void RemoveFromInventory() {
        Inventory.Instance.RemoveItemOutList(this);
    }
}