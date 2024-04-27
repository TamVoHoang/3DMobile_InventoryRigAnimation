using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//todo gameobject = PlayerInfo_UI ben trong (UI_Canvas_InGame)
//todo doi tuong chua canvas hien thi thong tin name, level, health, healthSlider
public class PlayerInfo_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private PlayerDataLocal_Temp playerDataLocal_Temp;
    private List<IDataPersistence> dataPersistenceObjects_InGame;
    
    private void Awake() {
        playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
        healthSlider = FindObjectOfType<Slider>();
        this.dataPersistenceObjects_InGame = FindAllDataPersistenceObjects(); //! tim object dang chua IData
    }

    private void Start() {
        StartCoroutine(ShowPlayerInfo_GameUI_Countine(0.5f));
        
        PlayerDataJson.Instance.LoadData_ToObjectsContainIDataPer(dataPersistenceObjects_InGame);
    }

    IEnumerator ShowPlayerInfo_GameUI_Countine(float time) {
        yield return new WaitForSeconds(time);
        ShowPlayerInfo_GameUI(playerDataLocal_Temp.userName,
                            playerDataLocal_Temp.level,
                            playerDataLocal_Temp.health);
    }

    private void ShowPlayerInfo_GameUI(string userName, int level, int health) {
        this.userName.text =""+ userName;
        this.level.text ="Lv: "+ level.ToString();
        this.health.text =""+ health.ToString();
        healthSlider.value = health;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    //?BUTTONS IN MAIN GAME
    public void LoadMainMenuScene_BackButtonInGame() {
        StartCoroutine(DelayTimeSave_ToExitGame(0.1f));
    }
    IEnumerator DelayTimeSave_ToExitGame(float time) {
        PlayerDataJson.Instance.SaveData_FromObjectsContainIDataPer(dataPersistenceObjects_InGame);
        PlayerDataJson.Instance.Save_PlayerDataJason_RealTime();
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f; //free
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
