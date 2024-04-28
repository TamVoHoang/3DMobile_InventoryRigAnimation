using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//! gameobject = doi tuong chi awake 1 lan trong scene
// load data 1 lan khi vao scene game
// neu doi tuong nay ko xuat hien 1 lan dauy nhat nho singleton dontDestroy
// data se load lai khi player resume game, va data bi gan vo so lan

public class LoadDataTo_IDataPersistence : Singleton<LoadDataTo_IDataPersistence>
{
    private List<IDataPersistence> dataPersistenceObjects_InGame;
    private List<IData_InventoryPersistence> inventoryPersistenceObjects_InGame;
    private PlayerDataJson playerDataJson;
    private InventoryDataJson inventoryDataJson;
    protected override void Awake() {
        base.Awake();
        playerDataJson = FindObjectOfType<PlayerDataJson>();
        inventoryDataJson = FindObjectOfType<InventoryDataJson>();
        
        //? tim object dang chua IData | IDatainventory
        this.dataPersistenceObjects_InGame = FindAllDataPersistenceObjects();
        this.inventoryPersistenceObjects_InGame = FindAllInventoryData_PersistenceObjects();
    }
    private void Start() {
        //? goi cac phuong thuc dang ke thua interface IPlayerData va IInventoryData chay
        playerDataJson.LoadData_ToObjectsContainIDataPer(dataPersistenceObjects_InGame);
        inventoryDataJson.LoadData_ToObjectsContainIInventoryPer(inventoryPersistenceObjects_InGame);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private List<IData_InventoryPersistence> FindAllInventoryData_PersistenceObjects() {
        IEnumerable<IData_InventoryPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IData_InventoryPersistence>();
        
        return new List<IData_InventoryPersistence>(dataPersistenceObjects);
    }

    public void SaveData_BeforeOutOfGame() {
        // cac ham ke thus Interface chay
        playerDataJson.SaveData_FromObjectsContainIDataPer(dataPersistenceObjects_InGame);
        inventoryDataJson.SaveInventoryData_FromObjectsContainIInventoryDataPer(inventoryPersistenceObjects_InGame);

        // sau khi cac ham ke thua chay - data persisten (InventoryJson + PlayerDataJson) da co du lieu
        // push du lieu cua data persistence len server
        playerDataJson.Save_PlayerDataJason_RealTime();
        inventoryDataJson.Save_InventoryDataJson_RealTime();
    }

    
    //todo
}
