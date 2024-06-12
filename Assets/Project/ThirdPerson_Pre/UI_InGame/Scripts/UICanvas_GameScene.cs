using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//? game object = UI canvas in testing_third person
// nut back button -> save data
public class UICanvas_GameScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenu; // nut back button and save

    private void Awake() {
        BackToMainMenu.onClick.AddListener(BackButtonToMainMenu_OnClick);
    }

    private void BackButtonToMainMenu_OnClick()
    {
        StartCoroutine(DelayTimeSave_ToExitGame(0.1f));
    }

    IEnumerator DelayTimeSave_ToExitGame(float time) {
        LoadDataTo_IDataPersistence.Instance.SaveData_BeforeOutOfGame();
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f; //todo free game
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
