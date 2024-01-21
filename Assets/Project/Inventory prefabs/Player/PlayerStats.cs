using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.Instance.onEquipmentChangedCallBack += OnEquipmentChanged;
    }


    void OnEquipmentChanged(Equipment newItem, Equipment oldItem) {
        if(newItem != null) {

            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier); 
        }

        if(oldItem != null) {

            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier); 
        }


    }
}
