using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemScriptableObject")]
public class ItemScriptableObject : ScriptableObject
{
    public Item.ItemType itemType;
    public string itemName;
    public Sprite itemSprite; //? 2D
    public GameObject pfItem;
    public RaycastWeapon gunPrefabRaycast;
    public CharacterEquipment.EquipSlot equipSlot;
    
}