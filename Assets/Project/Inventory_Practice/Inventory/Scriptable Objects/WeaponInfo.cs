using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public enum ItemType
    {
        Sword1,
        HealthPotion,
        ManaPotion,
        Coin,
        Medkit
    }

    public ItemType itemType;

    public int amount = 0;
    public Sprite sprite;

    //todo loai vat pham nao co the duoc cong don
    public bool IsStackable() {
        switch (itemType) {
        default:
        case ItemType.Coin:
        case ItemType.HealthPotion:
        case ItemType.ManaPotion:
            return true;

        case ItemType.Sword1:
        case ItemType.Medkit:
            return false;
        }
    }

}
