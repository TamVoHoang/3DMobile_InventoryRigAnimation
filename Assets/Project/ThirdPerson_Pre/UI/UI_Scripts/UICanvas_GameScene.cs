using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//? game object = UI_canvas_Ingame in testing_third person
// nut back button -> save data
public class UICanvas_GameScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenuButton; // nut back button and save
    [SerializeField] Button PauseButton; // nut dung game tam thoi
    [SerializeField] Button SettingButton;
    [SerializeField] bool isPaused = false;
    [SerializeField] RectTransform directionImage; // show direction when spaceship is spawned
    [SerializeField] Transform pauseButton_Trasform;
    [SerializeField] GameObject[] pausePlayArray;

    [SerializeField] GameObject SettingPanel;
    bool isShowingSettingPanel = false;


    bool isDataHandler;
    bool isSceneHandler;

    [SerializeField] const float DELAYTIME_TO_LOAD_SCENE = 0.2f;    // default = 0

    private void Awake() {
        BackToMainMenuButton.onClick.AddListener(BackButtonToMainMenu_OnClicked);
        PauseButton.onClick.AddListener(PauseButton_OnClicked);
        SettingButton.onClick.AddListener(SettingButton_Onclicked);
        pausePlayArray = new GameObject[pauseButton_Trasform.childCount];

        isDataHandler = false;
        isSceneHandler = false;
        isShowingSettingPanel = false;
        SettingPanel.SetActive(false);
    }

    private void SettingButton_Onclicked()
    {
        isShowingSettingPanel = !isShowingSettingPanel;
        if(isShowingSettingPanel) SettingPanel.SetActive(true);
        else SettingPanel.SetActive(false);
    }

    private void Start() {
        isDataHandler = TryGetComponent<LoadDataTo_IDataPersistence>(out LoadDataTo_IDataPersistence loadDataTo_IDataPersistence);
        isSceneHandler = TryGetComponent<TestLoadingScene>(out TestLoadingScene testLoading);
        
        isPaused = false;
        for (int i = 0; i < pauseButton_Trasform.childCount; i++)
        {
            pausePlayArray[i] = pauseButton_Trasform.GetChild(i).gameObject;
        }

    }

    private void Update() {
        if(!CheckSpawnerScene.IsInGameScene()) return;
            DirectionToSpaceship();
    }

    void DirectionToSpaceship() {
        if(GameManger.Instance.IsSpaceShipSpawned) {
            directionImage.GetComponent<Image>().enabled = true;
            var playerGun = FindObjectOfType<PlayerGun>();
            var spaceShip01 = FindObjectOfType<SpaceShip01>();

            // xet vi tri tuong doi player and space ship
            Vector3 direction = spaceShip01.transform.position - playerGun.transform.position;

            // xoay directionImage facing spaceship
            float angle = Vector3.SignedAngle(playerGun.transform.forward, direction, Vector3.up);
            directionImage.transform.localRotation = Quaternion.Euler(0, 0, -angle);
        }
        else {
            directionImage.GetComponent<Image>().enabled = false;
        }
    }

    private void BackButtonToMainMenu_OnClicked()
    {
        /* if(!TryGetComponent<LoadDataTo_IDataPersistence>(out LoadDataTo_IDataPersistence loadDataTo_IDataPersistence)) return; */
        
        StartCoroutine(DelayTimeSave_ToExitGame(DELAYTIME_TO_LOAD_SCENE));
    }

    void PauseButton_OnClicked() {
        isPaused = !isPaused;
        if(isPaused) {
            pausePlayArray[0].gameObject.SetActive(false);
            pausePlayArray[1].gameObject.SetActive(true);
            pausePlayArray[2].gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1; 
            pausePlayArray[0].gameObject.SetActive(true);
            pausePlayArray[1].gameObject.SetActive(false);
            pausePlayArray[2].gameObject.SetActive(false);
        } 
    }

    IEnumerator DelayTimeSave_ToExitGame(float time) {
        //? goi cac cs dang ke thua LoadDataTo_IDataPersistence save ve cho PlayerJson + InventoryJson
        LoadDataTo_IDataPersistence.Instance.SaveData_BeforeOutOfGame();
        
        yield return new WaitForSeconds(time);
        /* Time.timeScale = 0f; //todo free game
        SceneManager.LoadSceneAsync("MainMenu"); // kieu nao cung phai quay ve main menu */

        TestLoadingScene.Instance.SetCurrentScene(); // set currentSceneIndex in TestLoadingScene.cs
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu); // quay ve MainMenu Scene
        //GameManger.Instance.FrezzGame(); //todo DUNG GAME KHI QUAY VE MAN HINH MAIN MENE
        SetTimeScale.FrezzGame();
    }

    //todo
}
