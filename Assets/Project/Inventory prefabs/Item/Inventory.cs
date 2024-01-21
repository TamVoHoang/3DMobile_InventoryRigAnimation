using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton_<Inventory>
{
    //todo gameobject = GameManager object

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    [SerializeField] public List<Item> items = new List<Item>();
    [SerializeField] public int spaceInventory = 20;

    protected override void Awake() {
        base.Awake();
    }

    public bool Add(Item item) //todo dieu kien de Add Item kieu true false
    {
        if(!item.isDefaultItem) 
        {
            if(items.Count >= spaceInventory) {
                Debug.Log("full space");
                return false;
            }

            items.Add(item);

            if(onItemChangedCallBack != null) 
                onItemChangedCallBack.Invoke();
        }
        return true;
    }

    //todo nut x trong slot remove
    //todo phuon gthuc remove trong item SOb
    public void RemoveItemOutList(Item item) {
        items.Remove(item);

        if(onItemChangedCallBack != null) 
            onItemChangedCallBack.Invoke();
    }
}
