using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemHolder {

    void RemoveItemEquipment(Item item); //ok
    void AddItemEquipment(Item item);
    bool CanAddItemEquipment();
    void AddItemAfterSliting(Item item);

    //todo
}
