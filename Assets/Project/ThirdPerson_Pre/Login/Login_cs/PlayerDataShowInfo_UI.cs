using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerDataShowInfo_UI : MonoBehaviour
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
        StartCoroutine(LoadDataJson_Countine(delayTimeToLoad));//loginPress PREVIOUS SCENE - 3s - load() - 0s - setlocal()
        // StartCoroutine(ShowPlayerInfo_LoginScreen_Countine(delayTimeToLoad));
    }

    //?  RAT OK CO THE DUNG HAM NAY KET HOP VOI NUT NHAN INFO DE IN DATA TU MAIN GAME TRO LAI SCENE NAY
    public void InfoButton() {
        Debug.Log("co nhan nut tai thong tin nguoi choi");
        StartCoroutine(LoadDataJson_Countine(0f)); // c the bi cham lan dau ko load duoc se lay gia tri cu
        StartCoroutine(ShowPlayerInfo_LoginScreen_Countine(0f));    //loginPress -              10s             - print()
    }

    private void ShowPlayerInfo_LoginScreen(string mail, string userName, int level, int health, int killed, int died) {
        this.mail.text ="Mail: "+  mail;
        this.userName.text ="UserName: "+ userName;
        this.level.text ="Level: "+ level.ToString();
        this.health.text ="Health: "+ health.ToString();
        this.killed.text = "Killed: " + killed.ToString();
        this.died.text = "Died: " + died.ToString();
    }

    IEnumerator LoadDataJson_Countine(float time) {
        // hien hieu ung loading...
        yield return new WaitForSeconds(time); // cho login PREVIOUS SCENE xong -> load()
        playerDataJson.Load_PlayerDataJason_RealTime();
        StartCoroutine(ShowPlayerInfo_LoginScreen_Countine(2)); 

    }
    IEnumerator ShowPlayerInfo_LoginScreen_Countine(float time) {
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
        /* ShowPlayerInfo_LoginScreen(PlayerDataJson.Instance.PlayerJson.mail,
                                PlayerDataJson.Instance.PlayerJson.name,
                                PlayerDataJson.Instance.PlayerJson.level,
                                PlayerDataJson.Instance.PlayerJson.health,
                                PlayerDataJson.Instance.PlayerJson.killed,
                                PlayerDataJson.Instance.PlayerJson.died); */
    }

    //BUTTONS IN PLAYER INFO OVERVIEW
    public void LoadGame_Scene02_StartGameButton() {
        TestLoadingScene.Instance.LoadGame_Scene02();
    }
    public void Load_MainMenuSence_OnMainMenuButton() {
        TestLoadingScene.Instance.Load_MainMenu_Scene();
    }
    //todo
}
