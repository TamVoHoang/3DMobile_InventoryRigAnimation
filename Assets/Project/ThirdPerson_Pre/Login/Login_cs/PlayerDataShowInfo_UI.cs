using TMPro;
using UnityEngine;

public class PlayerDataShowInfo_UI : MonoBehaviour, IDataPersistence
{
    //todo game object = canvas hien thi thong sau khi nhan nut login dang nhap
    
    [SerializeField] private PlayerDataLocal_Temp playerDataLocal_Temp;
    [SerializeField] private PlayerDataJson playerDataJson;
    [SerializeField] private TextMeshProUGUI mail;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI killed;
    [SerializeField] private TextMeshProUGUI died;

    float delayTimeToLoad = 3f; // time de cho login Press coll 55 playfabloginManager.cs
    
    private void Awake() {
        playerDataJson = FindObjectOfType<PlayerDataJson>();
        playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
    }

    private void Start() {
        ////StartCoroutine(LoadDataJson_Countine(delayTimeToLoad));//loginPress PREVIOUS SCENE - 3s - load() - 0s - setlocal()
        //ShowInUI_FromDataLoacal_Temp(); //! RUN OK
        LoadData(playerDataJson.PlayerJson); //! test thu interface in ra
    }

    //?  RAT OK CO THE DUNG HAM NAY KET HOP VOI NUT NHAN INFO DE IN DATA TU MAIN GAME TRO LAI SCENE NAY
    public void InfoButton() {
        Debug.Log("co nhan nut tai thong tin nguoi choi");
        playerDataJson.Load_PlayerDataJason_RealTime();
        ShowInUI_FromDataLoacal_Temp();
        // StartCoroutine(LoadDataJson_Countine(0f)); // c the bi cham lan dau ko load duoc se lay gia tri cu
        // StartCoroutine(ShowPlayerInfo_LoginScreen_Countine(0f));    //loginPress -              10s             - print()
    }
    private void ShowInUI_FromDataLoacal_Temp() {
        this.mail.text ="Mail: "+  playerDataLocal_Temp.mail;
        this.userName.text ="UserName: "+ playerDataLocal_Temp.userName;
        this.level.text ="Level: "+ playerDataLocal_Temp.level.ToString();
        this.health.text ="Health: "+ playerDataLocal_Temp.health.ToString();
        this.killed.text = "Killed: " + playerDataLocal_Temp.killed.ToString();
        this.died.text = "Died: " + playerDataLocal_Temp.died.ToString();
    }

    //BUTTONS IN PLAYER INFO OVERVIEW
    public void LoadGame_Scene02_StartGameButton() {
        TestLoadingScene.Instance.LoadGame_Scene02();
    }
    public void Load_MainMenuSence_OnMainMenuButton() {
        TestLoadingScene.Instance.Load_MainMenu_Scene();
    }

    #region IDataPersistence
    public void LoadData(PlayerJson playerJsonData) {
        Debug.Log("interface co chay vao day");
        this.mail.text ="Mail: "+  playerJsonData.mail;
        this.userName.text ="UserName: "+ playerJsonData.name;
        this.level.text ="Level: "+ playerJsonData.level.ToString();
        this.health.text ="Health: "+ playerJsonData.health.ToString();
        this.killed.text = "Killed: " + playerJsonData.killed.ToString();
        this.died.text = "Died: " + playerJsonData.died.ToString();
    }

    public void SaveData(PlayerJson playerJsonData)
    {
        
    }
    #endregion IDataPersistence
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
        StartCoroutine(ShowPlayerInfo_LoginScreen_Countine(2)); 
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
