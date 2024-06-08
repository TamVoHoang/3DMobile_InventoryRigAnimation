using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerController_Alls : MonoBehaviour, IData_InventoryPersistence
{
    [SerializeField] int itemSlotAmount = 10; // tao slot trong Inventory_Alls
    Inventory inventory_Alls;

    [SerializeField] InventoryDataJson inventoryDataJson; // dung bien nay de goi

    [SerializeField] Transform spawnedPointItems;
    private void Awake() {
        inventory_Alls = new Inventory(UseItemsAlls, itemSlotAmount);

        inventoryDataJson = FindObjectOfType<InventoryDataJson>();
    }
    private void Start() {
        Load_InventoryData(inventoryDataJson.inventoryJson);
    }


    public Inventory GetInventoryAlls() {
        return inventory_Alls;
    }


    // ham de khi click vao Slot trong UI_Inventory_Alls
    void UseItemsAlls(Item item) {
        // click vao item trong InventoryAll -> show thong tin items
        
    }

    #region IInventoryPersistence
    public void Load_InventoryData(InventoryJson inventoryJson)
    {
        //manul load data from inventoryJson -> this is loaded
        foreach (var item in inventoryJson.itemsListJson)
        {
            // if(item.IsStackable()) this.inventory_Alls.AddItem(item);
            // if(!item.IsStackable()) this.inventory_Alls.AddItemEquipment(item);

            this.inventory_Alls.AddItemEquipment(item);
        }
    }

    public void Save_InventoryData(ref InventoryJson inventoryJson)
    {
        
    }
    #endregion IDataPersistence

    //todo
}
