using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//todo game object = canvas playerInfo in AccountPlayerOverview scene 
//todo after pressing loginButtonPressed in login Scene

public class PlayerDataShowInfo_UI : MonoBehaviour, IDataPersistence
{
    
    //[SerializeField] private PlayerDataLocal_Temp playerDataLocal_Temp;
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
    [SerializeField] Button StartGameButton;

    [SerializeField] Button EquipButton;

    [SerializeField] Button BackMainMenuButton;

    // others
    bool isLoaded = false;

    private void Awake() {
        isLoaded = false;

        playerDataJson = FindObjectOfType<PlayerDataJson>();
        inventoryDataJson = FindObjectOfType<InventoryDataJson>();

        StartGameButton.onClick.AddListener(StartGameButton_OnClicked);         // nut start At Overview Scene -> di thang vao scene game ThirdPerson
        BackMainMenuButton.onClick.AddListener(BackMainMenuButton_OnClicked);
        InfoButton.onClick.AddListener(InfoButton_OnClicked);

        EquipButton.onClick.AddListener(EquipButton_OnClicked); // button at Overdata Scene -> di den scen spawner player

        //playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
    }

    private void Start() {
        StartCoroutine(LoadData_ToShowPlayerInfo_Countine(3f));
    }

    //?  RAT OK CO THE DUNG HAM NAY KET HOP VOI NUT NHAN INFO DE IN DATA TU MAIN GAME TRO LAI SCENE NAY
    void InfoButton_OnClicked() {
        Debug.Log("co nhan nut tai thong tin nguoi choi");
        LoadPlayerData(playerDataJson.PlayerJson);
        isLoaded = true;
    }

    //BUTTONS IN PLAYER INFO OVERVIEW
    void EquipButton_OnClicked() {
        if(isLoaded) {
            isLoaded = false;
            //TestLoadingScene.Instance.LoadScene(TestLoadingScene.Spawner_Scene);
            TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_SpawnPlayer);

            //TestLoadingScene.Instance.LoadScene(TestLoadingScene.TestingThirdPerson_Scene);
            //TestLoadingScene.Instance.LoadScene(TestLoadingScene.ThirdPerson_Scene);

            //TestLoadingScene.Instance.Load_Testing_SpawnPlayer();
        }
    }

    void StartGameButton_OnClicked() {
        if(isLoaded) {
            isLoaded = false;
            //TestLoadingScene.Instance.LoadScene(TestLoadingScene.ThirdPerson_Scene);
            TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.ThirdPerson);
        }
    }
    void BackMainMenuButton_OnClicked() {
        if(isLoaded) {
            isLoaded = false;
            //TestLoadingScene.Instance.LoadScene(TestLoadingScene.MainMenu_Scene);
            TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
        }
    }

    #region IDataPersistence
    IEnumerator LoadData_ToShowPlayerInfo_Countine(float time) {
        yield return new WaitForSeconds(time);
        playerDataJson.Load_PlayerDataJason_RealTime();         // de lay playerJson
        inventoryDataJson.Load_InventoryDataJason_RealTime();   // lay inventoryJson = chi goi xuong chu ko dung inventoryJson thuc hien tiep
        
        yield return new WaitForSeconds(1f);
        LoadPlayerData(playerDataJson.PlayerJson); // lay data playerJson xet UI
        isLoaded = true;
    }

    public void LoadPlayerData(PlayerJson playerJsonData) {
        /* this.mail.text ="Mail: "+  playerJsonData.mail;
        this.userName.text ="UserName: "+ playerJsonData.name;
        this.level.text ="Level: "+ playerJsonData.level.ToString();
        this.health.text ="Health: "+ playerJsonData.health.ToString();
        this.killed.text = "Killed: " + playerJsonData.killed.ToString();
        this.died.text = "Died: " + playerJsonData.died.ToString(); */
        
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

    /* private void ShowInUI_FromDataLoacal_Temp() {
        this.mail.text ="Mail: "+  playerDataLocal_Temp.mail;
        this.userName.text ="UserName: "+ playerDataLocal_Temp.userName;
        this.level.text ="Level: "+ playerDataLocal_Temp.level.ToString();
        this.health.text ="Health: "+ playerDataLocal_Temp.health.ToString();
        this.killed.text = "Killed: " + playerDataLocal_Temp.killed.ToString();
        this.died.text = "Died: " + playerDataLocal_Temp.died.ToString();
    } */

    /* private void ShowPlayerInfo_LoginScreen(string mail, string userName, int level, int health, int killed, int died) {
        this.mail.text ="Mail: "+  mail;
        this.userName.text ="UserName: "+ userName;
        this.level.text ="Level: "+ level.ToString();
        this.health.text ="Health: "+ health.ToString();
        this.killed.text = "Killed: " + killed.ToString();
        this.died.text = "Died: " + died.ToString();
    } */
    /* IEnumerator LoadDataJson_Countine(float time) {
        // hien hieu ung loading...
        yield return new WaitForSeconds(time); // cho login PREVIOUS SCENE xong -> load()
        playerDataJson.Load_PlayerDataJason_RealTime();
    } */
    /* IEnumerator ShowPlayerInfo_LoginScreen_Countine(float time) {
        yield return new WaitForSeconds(time);
        Debug.Log("in player data local on ui");

        //todo In bang gi tri playerdata_temp trung gian
        ShowPlayerInfo_LoginScreen(playerDataLocal_Temp.mail,
                                    playerDataLocal_Temp.userName,
                                    playerDataLocal_Temp.level,
                                    playerDataLocal_Temp.health,
                                    playerDataLocal_Temp.killed,
                                    playerDataLocal_Temp.died);
        
        //todo in bang gia tri truc tiep playerDataJson cua server laod xuong | dang tre hon 1 nhip
        // ShowPlayerInfo_LoginScreen(PlayerDataJson.Instance.PlayerJson.mail,
        //                         PlayerDataJson.Instance.PlayerJson.name,
        //                         PlayerDataJson.Instance.PlayerJson.level,
        //                         PlayerDataJson.Instance.PlayerJson.health,
        //                         PlayerDataJson.Instance.PlayerJson.killed,
        //                         PlayerDataJson.Instance.PlayerJson.died);
    } */
    //todo
}
