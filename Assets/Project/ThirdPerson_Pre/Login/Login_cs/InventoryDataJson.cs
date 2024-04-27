using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

[System.Serializable]
public class ItemJson {
    public string itemJsonName;
    public int itemJsonAmount;
    public Item.ItemType itemType;
    public ItemJson(string name, int amount, Item.ItemType itemType) {
        this.itemJsonName = name;
        this.itemJsonAmount = amount;
        this.itemType = itemType;
    }
}

[System.Serializable]
public class InventoryJson{
    public string inventoryName;
    public List<ItemJson> itemJsons = new List<ItemJson>();
    
    public InventoryJson(string name, List<ItemJson> itemJsons) {
        this.inventoryName = name;
        this.itemJsons = itemJsons;
    }
}

public class InventoryDataJson : Singleton<InventoryDataJson>
{
    public ItemJson itemJson;
    public ItemJson itemJson2;
    public InventoryJson inventoryJson;
    protected override void Awake()
    {
        base.Awake();

        itemJson = new ItemJson("ISword_Red01", 1, Item.ItemType.ISword_Red_01); // dang in ra theo ma so
        itemJson2 = new ItemJson("ISword02_Green", 1, Item.ItemType.ISword_Green_02);

        inventoryJson = new InventoryJson("itemInventory", inventoryJson.itemJsons);
        
    }
    private void Start() {
        inventoryJson.itemJsons.Add(itemJson);
        inventoryJson.itemJsons.Add(itemJson2);

        Debug.Log(inventoryJson.inventoryName + inventoryJson.itemJsons);
        Debug.Log(inventoryJson.itemJsons.Count);
    }

    public void Save_InventoryDataJason_RealTime(InventoryJson inventoryJson) {
        string inventoryJson_String = JsonUtility.ToJson(inventoryJson); // inventoryJson = JsonUtility.FromJson<InventoryJson>(inventoryJson_String);
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"InventoryJson", JsonConvert.SerializeObject(inventoryJson)}, //JsonConvert.SerializeObject(inventoryJson)
            }
        },
        result => { Debug.Log("Inventory DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport());});
    }


    
    
}
