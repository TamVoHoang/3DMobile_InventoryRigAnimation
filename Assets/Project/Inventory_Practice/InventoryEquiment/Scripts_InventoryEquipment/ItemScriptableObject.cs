using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemScriptableObject")]
public class ItemScriptableObject : ScriptableObject
{
    public Item.ItemType itemType;
    public string itemName;
    public Sprite itemSprite; //? 2D

    //? doi tuong dung de chua sprites -> ItemWorld3DSpwaner prefab || this.gameObject se chay khi nam ngoai hierarchy -> 
    //? itemWordSpwaner.cs coll11 + coll 19 => SetItem3D() coll 39 ItemWorld3D.cs
    public GameObject pfItem;
    public CharacterEquipment.EquipSlot equipSlot;

    [Header("Gun")]
    public RaycastWeapon gunPrefabRaycast; // Gun Prefab with RaycastWeapon.cs
    [Header("Sword")]
    public HandSwordWeapon handSwordPrefab; // HandSword Prefab with HandSwordWeapon.cs
    
}