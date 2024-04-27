using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[System.Serializable]
public class InventoryJson{
    public string name;
    public Inventory inventory;

    public InventoryJson(){

    }
    
}

public class InventoryDataJson : Singleton<InventoryDataJson>
{
    private InventoryJson inventoryJson;
    public InventoryJson InventoryJson => inventoryJson;
    public List<string> testList = new List<string>() { "Ford", "BMW", "Benz" };

    protected override void Awake()
    {
        base.Awake();
    }

    public void Save_Inventory_DataJason_SignUp(InventoryJson inventoryJson) {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"InventoyJson", JsonConvert.SerializeObject(inventoryJson)},
            },
            Permission = UserDataPermission.Private
        },
        result => { Debug.Log("Inventory_DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
}
