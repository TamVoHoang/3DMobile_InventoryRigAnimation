using UnityEngine;

public class ISword : MonoBehaviour, IWeapon
{
    //todo game object = cay kiem kieu interface
    public ActiveWeapon.SwordSlots swordSlot;
    [SerializeField] private ItemScriptableObject weaponScriptableObject;
    

    public void Attack()
    {
        Debug.Log("Attack() ISword.cs called by ActiveWeapon.cs through IWeapon.cs");
        
    }

    public ItemScriptableObject GetWeaponInfo()
    {
        Debug.Log("GetWeaponInfo() ISword.cs called by ActiveWeapon.cs through IWeapon.cs");
        return weaponScriptableObject;
    }
}
