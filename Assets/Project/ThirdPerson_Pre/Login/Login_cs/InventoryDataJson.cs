using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor.Playables;
using UnityEngine;

/* [System.Serializable]
public class ItemJson {
    public string itemJsonName;
    public int itemJsonAmount;
    public Item.ItemType itemType;
    public ItemJson(string name, int amount, Item.ItemType itemType) {
        this.itemJsonName = name;
        this.itemJsonAmount = amount;
        this.itemType = itemType;
    }
} */

[System.Serializable]
public class InventoryJson
{
    public string inventoryName;
    public List<Item> itemsListJson = new List<Item>();
    
    public InventoryJson(string name, List<Item> itemsListJson) {
        this.inventoryName = name;
        this.itemsListJson = itemsListJson;
    }
}

public class InventoryDataJson : Singleton<InventoryDataJson>
{
    public Item item;
    public InventoryJson inventoryJson;
    public InventoryJson InventoryJson => inventoryJson;

    [Header("Item.ScriptableObjects")]
    [SerializeField] ItemScriptableObject IHealthPickup_01;
    [SerializeField] ItemScriptableObject IMagPistol_01;

    [SerializeField] ItemScriptableObject Pistol01_3D_01;
    [SerializeField] ItemScriptableObject SMG01_3D_01;
    [SerializeField] ItemScriptableObject ISword_01;
    [SerializeField] ItemScriptableObject ISword_02;


    protected override void Awake()
    {
        base.Awake();
    }
    private void Start() {
        /* inventoryJson = new InventoryJson("itemInventory", inventoryJson.itemsListJson);
        item_HealthPickup3D_01 = new Item {itemScriptableObject = new ItemScriptableObject() {
                                itemType = Item.ItemType.IHealthPickup3D_01 },
                                amount = 2};

        item_IMagPistol3D_01 = new Item {itemScriptableObject = new ItemScriptableObject() {
                                itemType = Item.ItemType.IMagPistol3D_01 },
                                amount = 2};
        
        inventoryJson.itemsListJson.Add(item_HealthPickup3D_01);
        inventoryJson.itemsListJson.Add(item_IMagPistol3D_01);

        Debug.Log(inventoryJson.inventoryName + inventoryJson.itemsListJson);
        Debug.Log(inventoryJson.itemsListJson.Count); */

        // item = new Item {itemScriptableObject = new ItemScriptableObject() {
        //                         itemType = Item.ItemType.IHealthPickup3D_01,
        //                         healthAmout = 100 },
        //                         amount = 2};
        // inventoryJson.itemsListJson.Add(item);

        // item = new Item {itemScriptableObject = new ItemScriptableObject() {
        //                         itemType = Item.ItemType.IMagPistol3D_01,
        //                         clipAmount = 1 },
        //                         amount = 2};
        // inventoryJson.itemsListJson.Add(item);

        item = new Item {itemScriptableObject = Pistol01_3D_01, amount = 1};
        inventoryJson.itemsListJson.Add(item);

        item = new Item {itemScriptableObject = ISword_01, amount = 1};
        inventoryJson.itemsListJson.Add(item);
    }

    private InventoryJson ReturnClassInventoryJson_ToSingnUp() {
        return new InventoryJson("itemInventory", inventoryJson.itemsListJson);
    }

    //SAVE
    public void SaveInventoryData_FromObjectsContainIInventoryDataPer(List<IData_InventoryPersistence> dataPersistenceObjects){
        foreach (IData_InventoryPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.Save_InventoryData(ref inventoryJson);
        }
    }

    public void Save_InventoryDataJson_RealTime() {
        string inventoryJson_String = JsonUtility.ToJson(inventoryJson); // inventoryJson = JsonUtility.FromJson<InventoryJson>(inventoryJson_String);
        
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"InventoryJson", JsonConvert.SerializeObject(inventoryJson_String)}, //JsonConvert.SerializeObject(inventoryJson)
            }
        },
        result => { Debug.Log("Inventory DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport());});
    }


    // SAVE inventoryJson SIGNUP
    public void Save_InventoryDataJson_SignUp() {
        string inventoryJson_String = JsonUtility.ToJson(ReturnClassInventoryJson_ToSingnUp()); // inventoryJson = JsonUtility.FromJson<InventoryJson>(inventoryJson_String);
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"InventoryJson", JsonConvert.SerializeObject(inventoryJson_String)}, //JsonConvert.SerializeObject(ReturnClassInventoryJson_ToSingnUp())
            }
        },
        result => { Debug.Log("Inventory DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport());});
    }

    //LOAD
    public void Load_InventoryDataJason_RealTime() {
        Debug.Log("co LOAD Inventory jsonnnn");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            OnGetinventoryDataJson,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetinventoryDataJson(GetUserDataResult result) {
        Debug.Log("Received the following Player Data Json:");
        foreach (var eachData in result.Data) {
            switch (eachData.Key) {
                case "InventoryJson":
                    //inventoryJson = JsonConvert.DeserializeObject<InventoryJson>((result.Data[eachData.Key].Value)); //TODO OK

                    var inventoryString = JsonConvert.DeserializeObject<string>((result.Data[eachData.Key].Value));
                    inventoryJson = JsonUtility.FromJson<InventoryJson>(inventoryString);
                    break;
            }
        }
    }

    //! LOAD trong game voi list Interface| lay data ben trong playerJson load ben tren sau do load cho cac doi tuong : Idatapersistence trong list
    public void LoadData_ToObjectsContainIInventoryPer(List<IData_InventoryPersistence> dataPersistenceObjects){
        foreach (IData_InventoryPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.Load_InventoryData(inventoryJson);
        }
    }




    
    //todo
}
