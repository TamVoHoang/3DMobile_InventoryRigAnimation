using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//? game object = UI_canvas_Ingame in testing_third person
// nut back button -> save data
public class UICanvas_GameScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenuButton; // nut back button and save
    [SerializeField] Button PauseButton; // nut dung game tam thoi
    [SerializeField] bool isPaused = false;
    [SerializeField] Transform pauseButton_Trasform;
    [SerializeField] GameObject[] pausePlayArray;

    bool isDataHandler;
    bool isSceneHandler;

    private void Awake() {
        BackToMainMenuButton.onClick.AddListener(BackButtonToMainMenu_OnClicked);
        PauseButton.onClick.AddListener(PauseButton_OnClicked);
        pausePlayArray = new GameObject[pauseButton_Trasform.childCount];

        isDataHandler = false;
        isSceneHandler = false;
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

    private void BackButtonToMainMenu_OnClicked()
    {
        /* if(!TryGetComponent<LoadDataTo_IDataPersistence>(out LoadDataTo_IDataPersistence loadDataTo_IDataPersistence)) return; */
        StartCoroutine(DelayTimeSave_ToExitGame(0.3f));
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
