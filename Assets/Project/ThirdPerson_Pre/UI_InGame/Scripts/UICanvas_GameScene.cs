using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//? game object = UI_canvas_Ingame in testing_third person
// nut back button -> save data
public class UICanvas_GameScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenu; // nut back button and save
    bool isDataHandler;
    bool isSceneHandler;

    private void Awake() {
        BackToMainMenu.onClick.AddListener(BackButtonToMainMenu_OnClick);

        isDataHandler = false;
        isSceneHandler = false;
    }

    private void Start() {
        isDataHandler = TryGetComponent<LoadDataTo_IDataPersistence>(out LoadDataTo_IDataPersistence loadDataTo_IDataPersistence);

        isSceneHandler = TryGetComponent<TestLoadingScene>(out TestLoadingScene testLoading);
    }

    private void BackButtonToMainMenu_OnClick()
    {
        //if(!TryGetComponent<LoadDataTo_IDataPersistence>(out LoadDataTo_IDataPersistence loadDataTo_IDataPersistence)) return;
        StartCoroutine(DelayTimeSave_ToExitGame(0.3f));
    }

    IEnumerator DelayTimeSave_ToExitGame(float time) {
        //? goi cac cs dang ke thua LoadDataTo_IDataPersistence save ve cho PlayerJson + InventoryJson
        LoadDataTo_IDataPersistence.Instance.SaveData_BeforeOutOfGame();
        
        yield return new WaitForSeconds(time);
        //Time.timeScale = 0f; //todo free game
        //SceneManager.LoadSceneAsync("MainMenu"); // kieu nao cung phai quay ve main menu

        TestLoadingScene.Instance.SetCurrentScene(); // set currentSceneIndex in TestLoadingScene.cs
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu); // quay ve MainMenu Scene
        Time.timeScale = 0; //todo DUNG GAME KHI QUAY VE MAN HINH MAIN MENE
    }

    //todo
}
