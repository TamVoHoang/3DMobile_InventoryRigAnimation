using UnityEngine;

public class IHand : MonoBehaviour, IWeapon
{
    [SerializeField] private ItemScriptableObject weaponScriptableObject;
    public void Attack()
    {
        Debug.Log("Attack() IHand.cs called by ActiveWeapon.cs through IWeapon.cs");
        
    }

    public ItemScriptableObject GetWeaponInfo()
    {
        Debug.Log("GetWeaponInfo() IHand.cs called by ActiveWeapon.cs through IWeapon.cs");
        return weaponScriptableObject;
    }
}
