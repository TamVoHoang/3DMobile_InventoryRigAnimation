public interface IData_InventoryPersistence
{
    void Load_InventoryData(InventoryJson inventoryJson);
    void Save_InventoryData(ref InventoryJson inventoryJson);
}
