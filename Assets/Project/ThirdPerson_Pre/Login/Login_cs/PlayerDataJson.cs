using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System.Collections;
using System;


public class PlayerJson {
    public string mail; // phai load duoc ve khi vao player infomation
    public string name;
    public int level;
    public int health;
    public int killed;
    public int died;

    public PlayerJson(string mail, string name, int level,int health, int killed, int died) {
        this.mail = mail;
        this.name = name;
        this.level = level;
        this.health = health;
        this.killed = killed;
        this.died = died;
    }
    public PlayerJson() {

    }
}

public class PlayerDataJson : Singleton<PlayerDataJson>
{
    public event EventHandler<PlayerData> OnPlayerDataLocalChanged;
    public class PlayerData : EventArgs {
        public string mail;
        public string userName;
        public int level;
        public int health;
        public int killed;
        public int died;
    }

    private PlayerJson playerJson;
    public PlayerJson PlayerJson {get => playerJson;}
    private PlayerJson ReturnClassPlayerJson_ToSave() {
        return new PlayerJson(playerJson.mail, playerJson.name, playerJson.level,
                                PlayerDataLocal_Temp.Instance.health,
                                PlayerDataLocal_Temp.Instance.killed,
                                PlayerDataLocal_Temp.Instance.died);
    }
    protected override void Awake() {
        base.Awake();
    }

#region SAVE
    //? Save doi tuong khoi tao ko tham so
    public void Save_PlayerJson_ToResiger(PlayerJson playerJson) {
        Debug.Log("Save_PlayerJson_ToResiger");
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"Json", JsonConvert.SerializeObject(playerJson)},
            }
        },
        result => { Debug.Log("Player DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    //? save doi tuong duoc khoi tao co tham so
    public void Save_PlayerDataJason_RealTime() {
        Debug.Log("co SAVE jsonnnn");
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"Json", JsonConvert.SerializeObject(ReturnClassPlayerJson_ToSave())},
            }
        },
        result => { Debug.Log("Player DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
#endregion SAVE

#region LOAD
    //?LOAD DATA
    public void Load_PlayerDataJason_RealTime() {
        Debug.Log("co LOAD jsonnnn");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            OnGetPlayerDataJson,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetPlayerDataJson(GetUserDataResult result) {
        Debug.Log("Received the following Player Data Json:");

        foreach (var eachData in result.Data) {
            switch (eachData.Key)
            {
                case "Json":
                    playerJson = JsonConvert.DeserializeObject<PlayerJson>((result.Data[eachData.Key].Value)); break;
            }
        }
        StartCoroutine(SetPlayerDataLocalTemp_Countine(0f)); //load() - time delay - set playerDataLocal
    }
#endregion LOAD

    //? set player data local coll 30 playerdatalocal.cs
    IEnumerator SetPlayerDataLocalTemp_Countine(float time) {
        yield return new WaitForSeconds(time);
        Debug.Log("set player data Loacal");
        OnPlayerDataLocalChanged?.Invoke(this, new PlayerData{mail = playerJson.mail,
                                                            userName = playerJson.name,
                                                            level = playerJson.level,
                                                            health = playerJson.health,
                                                            killed = playerJson.killed,
                                                            died = playerJson.died});
    }

//todo login - 3s - load - 3s - set
}
