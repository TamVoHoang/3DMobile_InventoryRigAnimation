using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System;
using System.Linq;

[System.Serializable]
public class PlayerJson
{
    public string mail, name;
    public int level, health, killed, died;
    public Vector3 position, rotation;// se duoc Json ep kieu dau duoi ve vector3

    public PlayerJson() {}
    public PlayerJson(string mail, string name, int level,int health, int killed, int died, Vector3 position) {
        this.mail = mail;
        this.name = name;
        this.level = level;
        this.health = health;
        this.killed = killed;
        this.died = died;
        this.position = position;
    }
    //todo
}

public class PlayerDataJson : Singleton<PlayerDataJson>
{
    public event EventHandler<PlayerData> OnPlayerDataLocalChanged; // tao event de pass data sang PlayerDataLocal
    public class PlayerData : EventArgs {
        public string mail;
        public string userName;
        public int level;
        public int health;
        public int killed;
        public int died;

        public Vector3 Position;
    }
    //PlayerDataLocal_Temp playerDataLocal_Temp;
    
    private PlayerJson playerJson;
    public PlayerJson PlayerJson => playerJson;

    private Vector3 initialVector3Player_ToRegister = new Vector3(12,0.5f,20);
    public Vector3 InitialVector3Player_ToRegister => initialVector3Player_ToRegister;
    private List<IDataPersistence> dataPersistenceObjects; //! list chua IDataPersistence

    // dieu kien dung de enable or disable AccountButton trong mainMenu Screen
    private bool isLoadedSuccessInLogin = false;
    public bool IsLoadedSuccessInLogin => isLoadedSuccessInLogin;

    private bool isLoadedSuccessInGame = false;
    public bool IsLoadedSuccessInGame => isLoadedSuccessInGame; // coll 59 MainMenu.cs -> khi da co LoadData thi enable nut Account

    public string loggedPayfabID;

    protected override void Awake() {
        base.Awake();
        /* playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
        this.dataPersistenceObjects = FindAllDataPersistenceObjects(); //! tao list objects: IDataPersistence */

        isLoadedSuccessInLogin = false;
        isLoadedSuccessInGame = false;
    }

    //? Tao doi tuong lop playerJson local - gan gia tri -> save "Json" PlayFab.
    private PlayerJson ReturnClassPlayerJson_ToSignUp(string mail, string name) {
        ////string vector3ToString = JsonUtility.ToJson(initialVector3Player_ToRegister); // chi chuyen vector3 sang string luu theo hang doc
        return new PlayerJson(mail, name, 1, 500, 0, 0, initialVector3Player_ToRegister); // vector3ToString
    }

    //? SAVE KHI SIGN UP
    // save PlayJson local to "Json" PlayFab -> khi nhan nut Register trong Register Screen
    public void Save_PlayerDataJason_SignUp(string mail, string name) {
        string playerJsonString = JsonUtility.ToJson(ReturnClassPlayerJson_ToSignUp(mail, name));
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                ////{"Json", JsonConvert.SerializeObject(ReturnClassPlayerJson_ToSignUp(mail, name))}, //OK
                {"Json", JsonConvert.SerializeObject(playerJsonString)},
            }
        },
        result => { Debug.Log("Player DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

/*     // Save doi tuong khoi tao ko tham so coll 33 PlayFabManager.cs
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
 */

    //? SAVE data event in game - back button
    // doi tuong ke thua IDataPersistence -> run ham ke thua rieng le -> gan gia tri cho playerJson
    public void SaveData_FromObjectsContainIDataPer(List<IDataPersistence> dataPersistenceObjects){
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SavePlayerData(playerJson);
        }
    }

    //? SAVE PLAYER DATA REALTIME
    // save thong tin nguoi choi trong game - thong qua IDataPersistence
    public void Save_PlayerDataJason_RealTime() {
        string playerJsonString = JsonUtility.ToJson(playerJson);
        Debug.Log("co SAVE jsonnnn");
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"Json", JsonConvert.SerializeObject(playerJsonString)},
            }

        },
        result => { Debug.Log("Player DataJason Title updated");},
        error => { Debug.LogError(error.GenerateErrorReport());});
    }

    //? LOAD any saved data from data handler
    // login in loginScreen, ShowPlayerInfo in over player data scene
    public void Load_PlayerDataJason_RealTime() {
        Debug.Log("Load Json Playfab to playerJson Local");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            OnGetPlayerDataJson,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetPlayerDataJson(GetUserDataResult result) {
        foreach (var eachData in result.Data) {
            switch (eachData.Key) {
                case "Json":
                    ////playerJson = JsonConvert.DeserializeObject<PlayerJson>((result.Data[eachData.Key].Value)); // OK
                    var playerDataJsonString = JsonConvert.DeserializeObject<string>(result.Data[eachData.Key].Value);
                    playerJson = JsonUtility.FromJson<PlayerJson>(playerDataJsonString);
                    break;
            }
        }
        ////StartCoroutine(SetPlayerDataLocalTemp_Countine(0f)); //load() - time delay - set playerDataLocal
        Debug.Log("Received the following playerDataJson:");
        isLoadedSuccessInLogin = true;
    }

    //? LOAD playerJson local to object chua class ke thua IDataPersistence
    //! LOAD trong game voi list Interface| lay data ben trong playerJson load ben tren sau do load cho cac doi tuong : Idatapersistence trong list
    public void LoadData_ToObjectsContainIDataPer(List<IDataPersistence> dataPersistenceObjects){
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.LoadPlayerData(playerJson);
        }

        //? da load data tu playerJson vao mainGame
        isLoadedSuccessInGame = true;
    }


    //? set player datalocal_temp coll 30 playerdatalocal.cs
    /* IEnumerator SetPlayerDataLocalTemp_Countine(float time) {
        yield return new WaitForSeconds(time);
        Debug.Log("LOADED and SET playerDataLoacal from PlayerDataJson server");

        Vector3 vector3ToSetLocal = JsonUtility.FromJson<Vector3>(playerJson.position);
        
        OnPlayerDataLocalChanged?.Invoke(this, new PlayerData{mail = playerJson.mail,
                                                            userName = playerJson.name,
                                                            level = playerJson.level,
                                                            health = playerJson.health,
                                                            killed = playerJson.killed,
                                                            died = playerJson.died,
                                                            Position = vector3ToSetLocal});
    } */

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    #region  LEADERBOARD
    // send khi quay thoat khoi game scene, SaveData_BeforeOutOfGame() col 50 LoadDatato_IDataPersistence.cs
    public void SendLeaderBoard(int _killedCount) {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "KilledCount",
                    Value = _killedCount
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result) => Debug.Log("Sent leader board");
    
    // get nut nhan trong data overview scene
    public void GetLeaderBoard() {
        var request = new GetLeaderboardRequest {
            StatisticName = "KilledCount",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    private void OnLeaderBoardGet(GetLeaderboardResult result) {


        foreach (var item in result.Leaderboard) {
            Debug.Log($"{item.Position} {item.DisplayName} {item.StatValue}");
        }
    }



    private void OnError(PlayFabError error) => Debug.Log(error.GenerateErrorReport());
    #endregion LEADERBOARD

    //todo
}