using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton_<InventoryUI>
{
    //todo gameobject = canvas_UI inventory
    //todo quan ly bang UI hien item len tren bang

    [SerializeField] GameObject inventoryPanel; // on of cai bang
    [SerializeField] private Transform itemsParent; // quan ly button(select + remove)
    [SerializeField] private InventorySlot[] slots; //? mang chua (itemsParent.getChild) chua scrips inventory.cs
    protected override void Awake() {
        base.Awake();

        //? mang chua cac phan tu kieu InventorySlot.cs ( slots[i]. phuong thuc cua InventorySlot.cs )
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        
    }

    private void Start() {
        Inventory.Instance.onItemChangedCallBack += UpdateUI;
    }

    void Update()
    {
        if(Input.GetButtonDown("Inventory")) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
    void UpdateUI() // goi tu class khac
    {
        Debug.Log("updating ui from delegate Inventory.cs after add() and clear()");
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < Inventory.Instance.items.Count) {
                slots[i].AddItem(Inventory.Instance.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}