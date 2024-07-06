using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


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
    public InventoryJson inventoryJson; //! phai de public - do IventoryJson dang [Serializable]
    //public Item item;

    [Header("       Item.ScriptableObjects")]
    [SerializeField] ItemScriptableObject IHealthPickup_01;
    [SerializeField] ItemScriptableObject IMagPistol_01;

    [SerializeField] ItemScriptableObject Pistol01_3D_01;
    [SerializeField] ItemScriptableObject Pistol01_3D_02;
    [SerializeField] ItemScriptableObject Pistol01_3D_03;

    [SerializeField] ItemScriptableObject SMG01_3D_01;
    [SerializeField] ItemScriptableObject GunSMWEP_01;
    [SerializeField] ItemScriptableObject GunSMWEP_02;

    [SerializeField] ItemScriptableObject ISword_01;
    [SerializeField] ItemScriptableObject ISword_02;


    protected override void Awake() {
        base.Awake();
    }
    private void Start() {
        //? co the khoi tao doi tuong item - add vao itemListJson tai day.
        /* item = new Item {itemScriptableObject = ISword_01, amount = 1};
        inventoryJson.itemsListJson.Add(item); */
    }

    private void CreateNewItemListJson_ToSignUp(ItemScriptableObject ItemS, int amount) {
        var item = new Item {itemType = ItemS.itemType, amount = amount};
        inventoryJson.itemsListJson.Add(item);
    }

    //? khoi to InvenJson de signup() - cho vu khi khi signup
    private InventoryJson ReturnInventoryJson_ToSingnUp() {
        CreateNewItemListJson_ToSignUp(Pistol01_3D_01, 1);
        CreateNewItemListJson_ToSignUp(Pistol01_3D_02, 1);
        CreateNewItemListJson_ToSignUp(Pistol01_3D_03, 1);
        CreateNewItemListJson_ToSignUp(GunSMWEP_01, 1);
        CreateNewItemListJson_ToSignUp(GunSMWEP_02, 1);

        CreateNewItemListJson_ToSignUp(IHealthPickup_01, 3);
        CreateNewItemListJson_ToSignUp(IMagPistol_01, 3);
        return new InventoryJson("ItemInventory", inventoryJson.itemsListJson);
    }

    //? SAVE inventoryJson SIGNUP
    public void Save_InventoryDataJson_SignUp() {
        string inventoryJson_String = JsonUtility.ToJson(ReturnInventoryJson_ToSingnUp()); // inventoryJson = JsonUtility.FromJson<InventoryJson>(inventoryJson_String);
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"InventoryJson", JsonConvert.SerializeObject(inventoryJson_String)}, //JsonConvert.SerializeObject(ReturnClassInventoryJson_ToSingnUp())
            }
        },
        result => { Debug.Log("Inventory DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport());});
    }

    //? gameObjects :IData_InventoryPersistence run Save_InventoryData() | backButton torng game call
    public void SaveInventoryData_FromObjectsContainIInventoryDataPer(List<IData_InventoryPersistence> dataPersistenceObjects){
        foreach (IData_InventoryPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.Save_InventoryData(ref inventoryJson);
        }
    }

    public void Save_InventoryDataJson_RealTime() {
        Debug.Log("Save inventory Local to Inventoryjson_PlayFab");

        //? cach dang dung dung nhat
        string inventoryJson_String = JsonUtility.ToJson(inventoryJson); //inventoryJson = JsonUtility.FromJson<InventoryJson>(inventoryJson_String);
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                
                //? cach dang dung dung nhat
                {"InventoryJson", JsonConvert.SerializeObject(inventoryJson_String)},// {"InventoryJson", JsonConvert.SerializeObject(inventoryJson)},//TODO OK cach 1 _ testing
            }
        },
        result => { Debug.Log("Inventory DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport());});
    }

    //? LOAD
    public void Load_InventoryDataJason_RealTime() {
        inventoryJson.itemsListJson.Clear(); // test thu xoa het truoc khi nhan cai load moi

        Debug.Log("Load Inventory Playfab to inventoryjson Local");
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
                    //? cach dang dung dung nhat
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
