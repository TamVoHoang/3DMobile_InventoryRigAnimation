using System;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//todo game object = canvas playerInfo in AccountPlayerOverview scene 
//todo after pressing loginButtonPressed in login Scene

public class PlayerDataShowInfo_UI : MonoBehaviour, IDataPersistence
{
    
    [SerializeField] private PlayerDataJson playerDataJson;
    [SerializeField] private InventoryDataJson inventoryDataJson;

    [Header("Showing Data from playerJson")]
    [SerializeField] private TextMeshProUGUI mail;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI killed;
    [SerializeField] private TextMeshProUGUI died;

    [Header("Buttons")]
    [SerializeField] Button InfoButton;
    [SerializeField] Button RankingButton;
    [SerializeField] Button GetRankingAroundPlayer_Button;

    [SerializeField] Button EquipButton;
    [SerializeField] Button SettingButton;

    [SerializeField] Button StartGameButton;
    [SerializeField] Button BackMainMenuButton;

    // others
    bool isLoaded = false;

    // leader board
    [Header("Leader Board")]
    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform rowParent;
    private void Awake() {
        isLoaded = false;

        playerDataJson = FindObjectOfType<PlayerDataJson>();
        inventoryDataJson = FindObjectOfType<InventoryDataJson>();

        StartGameButton.onClick.AddListener(StartGameButton_OnClicked);         // nut start At Overview Scene -> di thang vao scene game ThirdPerson
        BackMainMenuButton.onClick.AddListener(BackMainMenuButton_OnClicked);
        
        InfoButton.onClick.AddListener(InfoButton_OnClicked);
        EquipButton.onClick.AddListener(EquipButton_OnClicked); // button at Overdata Scene -> di den scen spawner player
        SettingButton.onClick.AddListener(SettingButton_OnClicked); //button hien thi screen dieu chinh am thanh

    }

    private void Start() {
        //? khi vao scene nay -> se auton load data
        //StartCoroutine(LoadData_ToShowPlayerInfo_Countine(2f));
        UpdateUIVisual(playerDataJson.PlayerJson);  //? test auto load when back to main menu -> account

        // de awake bi null ko run duoc action
        RankingButton.onClick.AddListener(RankingButton_OnCliked);
        GetRankingAroundPlayer_Button.onClick.AddListener(GetRankingAroundPlayer_Button_OnClicked);
    }

    void InfoButton_OnClicked() {
        Debug.Log("co nhan nut tai thong tin nguoi choi");
        UpdateUIVisual(playerDataJson.PlayerJson);
        isLoaded = true;
    }

    void RankingButton_OnCliked(){
        GetLeaderBoard();
    }

    private void GetRankingAroundPlayer_Button_OnClicked() {
        GetLeaderBoardAroundPlayer();
    }

    //BUTTONS IN PLAYER INFO OVERVIEW
    void EquipButton_OnClicked() {
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_SpawnPlayer);

        SetTimeScale.UnFrezzeGame();
    }

    private void SettingButton_OnClicked() {
        
    }


    void StartGameButton_OnClicked() {
        /* TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.ThirdPerson); */
    }
    void BackMainMenuButton_OnClicked() {
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }

    //? GET LEADER BOARD KILLED COUNT
    public void GetLeaderBoard() {
        var request = new GetLeaderboardRequest {
            StatisticName = "KilledCount",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }

    private void OnLeaderBoardGet(GetLeaderboardResult result) {
        foreach (Transform item in rowParent) {
            Destroy(item.gameObject);
        }
        StartCoroutine(ShowingLeaderBoard(result));
    }

    IEnumerator ShowingLeaderBoard(GetLeaderboardResult result) {
        yield return new WaitForSeconds(0.5f);
        foreach (var item in result.Leaderboard) {
            GameObject newRow = Instantiate(rowPrefab, rowParent); // 1 row prefab - 3 texts (pos, name, killedCount)
            TextMeshProUGUI[] childs = newRow.GetComponentsInChildren<TextMeshProUGUI>(); // tao mang kieu Text chua text child
            Debug.Log(childs.Length);

            childs[0].GetComponent<TextMeshProUGUI>().text = (item.Position + 1).ToString();
            childs[1].GetComponent<TextMeshProUGUI>().text = item.DisplayName;
            childs[2].GetComponent<TextMeshProUGUI>().text = item.StatValue.ToString();

            Debug.Log($"{item.Position} {item.DisplayName} {item.StatValue}");
        }
    }

    //? GET LEADER AROUND PLAYER
    void GetLeaderBoardAroundPlayer() {
        var request = new GetLeaderboardAroundPlayerRequest {
            StatisticName = "KilledCount",
            MaxResultsCount = 9
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderBoardAroundPlayerGet, OnError);
    }

    private void OnLeaderBoardAroundPlayerGet(GetLeaderboardAroundPlayerResult result) {
        foreach (Transform item in rowParent) {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard) {
            GameObject newRow = Instantiate(rowPrefab, rowParent); // 1 row prefab - 3 texts (pos, name, killedCount)
            TextMeshProUGUI[] childs = newRow.GetComponentsInChildren<TextMeshProUGUI>(); // tao mang kieu Text chua text child
            Debug.Log(childs.Length);

            childs[0].GetComponent<TextMeshProUGUI>().text = (item.Position + 1).ToString();
            childs[1].GetComponent<TextMeshProUGUI>().text = item.DisplayName;
            childs[2].GetComponent<TextMeshProUGUI>().text = item.StatValue.ToString();

            if(item.PlayFabId == playerDataJson.loggedPayfabID) {
                childs[0].color = Color.cyan;
                childs[1].color = Color.cyan;
                childs[2].color = Color.cyan;
            }

            Debug.Log($"{item.Position} {item.DisplayName} {item.StatValue}");
        }
    }


    private void OnError(PlayFabError error) => Debug.Log(error.GenerateErrorReport());

    IEnumerator LoadData_ToShowPlayerInfo_Countine(float time) {
        yield return new WaitForSeconds(time);
        playerDataJson.Load_PlayerDataJason_RealTime();         // de lay playerJson
        inventoryDataJson.Load_InventoryDataJason_RealTime();   // lay inventoryJson = chi goi xuong chu ko dung inventoryJson thuc hien tiep
        
        yield return new WaitForSeconds(1f);
        UpdateUIVisual(playerDataJson.PlayerJson); // lay data playerJson xet UI
        isLoaded = true;
    }
    
    #region IDataPersistence
    public void UpdateUIVisual(PlayerJson playerJsonData) {
        this.mail.text = playerJsonData.mail;
        this.userName.text = playerJsonData.name;
        this.level.text = playerJsonData.level.ToString();
        this.health.text = playerJsonData.health.ToString();
        this.killed.text = playerJsonData.killed.ToString();
        this.died.text = playerJsonData.died.ToString();
    }

    public void SavePlayerData(PlayerJson playerJsonData) { 

    }

    #endregion IDataPersistence

    //todo
}
