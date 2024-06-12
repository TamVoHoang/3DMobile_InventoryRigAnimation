using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//* gameobject = UI_cnvas_PlayerInfo ben trong (Player)
//* doi tuong chua canvas hien thi thong tin name, level, health, healthSlider, mini map

public class PlayerInfo_UI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] Slider healthSlider;
    [SerializeField] PlayerDataJson playerDataJson;
    [SerializeField] LoadDataTo_IDataPersistence loadDataTo_IDataPersistence;

    // [SerializeField] private PlayerDataLocal_Temp playerDataLocal_Temp;
    /* private List<IDataPersistence> dataPersistenceObjects_InGame;
    private List<IData_InventoryPersistence> inventoryPersistenceObjects_InGame; */

    
    private void Awake() {
        Time.timeScale = 1; //todo UnFree game when resume game
        
        playerDataJson = FindObjectOfType<PlayerDataJson>();
        loadDataTo_IDataPersistence = FindObjectOfType<LoadDataTo_IDataPersistence>();
        healthSlider = GetComponentInChildren<Slider>();

        // playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
        /* this.dataPersistenceObjects_InGame = FindAllDataPersistenceObjects(); //! tim object dang chua IData
        this.inventoryPersistenceObjects_InGame = FindAllInventoryData_PersistenceObjects(); */

    }

    private void Start() {
        if(playerDataJson == null) return; //! TESTING

        StartCoroutine(ShowPlayerInfo_GameUI_Countine(0.2f));
        
        /* PlayerDataJson.Instance.LoadData_ToObjectsContainIDataPer(dataPersistenceObjects_InGame);
        InventoryDataJson.Instance.LoadData_ToObjectsContainIInventoryPer(inventoryPersistenceObjects_InGame); */
    }

    IEnumerator ShowPlayerInfo_GameUI_Countine(float time) {
        yield return new WaitForSeconds(time);
        LoadPlayerInfo_GameUI(playerDataJson.PlayerJson.name,
                            playerDataJson.PlayerJson.level,
                            playerDataJson.PlayerJson.health);
        
    }

    private void LoadPlayerInfo_GameUI(string userName, int level, int health) {
        this.userName.text =""+ userName;
        this.level.text ="Lv: "+ level.ToString();
        this.health.text =""+ health.ToString();
        healthSlider.value = health;
    }

    /* private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private List<IData_InventoryPersistence> FindAllInventoryData_PersistenceObjects() {
        IEnumerable<IData_InventoryPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IData_InventoryPersistence>();
        
        return new List<IData_InventoryPersistence>(dataPersistenceObjects);
    } */

    //?BUTTONS IN MAIN GAME
    public void LoadMainMenuScene_BackButtonInGame() {
        StartCoroutine(DelayTimeSave_ToExitGame(0.1f));
    }
    IEnumerator DelayTimeSave_ToExitGame(float time) {
        /* PlayerDataJson.Instance.SaveData_FromObjectsContainIDataPer(dataPersistenceObjects_InGame);
        InventoryDataJson.Instance.SaveInventoryData_FromObjectsContainIInventoryDataPer(inventoryPersistenceObjects_InGame);
        
        PlayerDataJson.Instance.Save_PlayerDataJason_RealTime();
        InventoryDataJson.Instance.Save_InventoryDataJason_RealTime(); */
        loadDataTo_IDataPersistence.SaveData_BeforeOutOfGame();
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f; //todo free game
        SceneManager.LoadSceneAsync("MainMenu");
    }

}
