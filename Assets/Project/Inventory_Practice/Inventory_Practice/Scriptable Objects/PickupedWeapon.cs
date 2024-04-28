using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupedWeapon : MonoBehaviour
{
    [SerializeField] WeaponInfo weaponInfoPickuped;
    // public delegate void OnItemAdded(WeaponInfoScriptableObject newWeapon);
    // public OnItemAdded onItemAddedCallBack;
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if(playerController) {
            Debug.Log("co cham player");
            //playerController.inven.AddWeaponInfo(weaponInfoPickuped);
            
        }
    }
}