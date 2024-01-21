using UnityEngine;

public enum EquipmentSlot
{
    Head,
    Face,
    ShoulderL,
    ShoulderR,
    Weapon,
    Sheild,
    Feet
}

[CreateAssetMenu(fileName ="New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    [Header("Virable Equipment")]
    public EquipmentSlot equipmentSlot; //?slot to store enquipment
    public MeshRenderer meshRenderer; // doi tuong prefab se trang bi player
    public int armorModifier; //? incre or des in armor
    public int damageModifier; //? incre or des in damage

    public override void Use() //? ke thua tu col 16 Item.cs
    {
        base.Use();
        //todo trang bi khi select
        EquipmentManager.Instance.Equip(this); //? Equimentmanager.cs

        //todo xoa equipment : Item ra hoi list Items ( Iventory.cs coll11)
        //todo xoa icon khoi UI 
        RemoveFromInventory(); //? xoa Item khoi list | phuong thuc ke thua tu :Item
    }
}

