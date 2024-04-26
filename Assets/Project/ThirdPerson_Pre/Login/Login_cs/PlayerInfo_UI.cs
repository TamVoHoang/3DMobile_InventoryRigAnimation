using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    PlayerHealth playerHealth;

    [SerializeField] private PlayerDataLocal_Temp playerDataLocal_Temp;

    private List<IDataPersistence> dataPersistenceObjects;
    private void Awake() {
        playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
        healthSlider = FindObjectOfType<Slider>();
        playerHealth = FindObjectOfType<PlayerHealth>();
         this.dataPersistenceObjects = FindAllDataPersistenceObjects(); //! tim object dang chua IData
    }

    private void Start() {
        StartCoroutine(ShowPlayerInfo_GameUI_Countine(0.5f));

        Debug.Log(dataPersistenceObjects.Count);
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
        //playerHealth.SetCurrentHealth = health;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    

    //?BUTTONS IN MAIN GAME
    public void LoadMainMenuScene_BackButtonInGame() {
        /* Time.timeScale = 0f; //free
        SceneManager.LoadSceneAsync("MainMenu");
        ////playerDataLocal_Temp.position_Temp = PlayerGun.Instance.transform.position;
        ////PlayerGun.Instance.SaveData(PlayerDataJson.Instance.PlayerJson);
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(PlayerDataJson.Instance.PlayerJson);
        }
        ////playerDataLocal_Temp.BackButtonPress_SavePlayerDataJson();
        PlayerDataJson.Instance.Save_PlayerDataJason_RealTime(); */
        StartCoroutine(DelayTimeSave_ToExitGame(0.2f));
    }
    IEnumerator DelayTimeSave_ToExitGame(float time) {
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(PlayerDataJson.Instance.PlayerJson);
        }
        PlayerDataJson.Instance.PlayerJson.position = JsonUtility.ToJson(PlayerGun.Instance.transform.position);
        PlayerDataJson.Instance.Save_PlayerDataJason_RealTime();
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f; //free
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
