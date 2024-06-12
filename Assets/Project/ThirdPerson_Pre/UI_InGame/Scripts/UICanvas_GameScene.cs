using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//? game object = UI_canvas_Ingame in testing_third person
// nut back button -> save data
public class UICanvas_GameScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenu; // nut back button and save

    private void Awake() {
        BackToMainMenu.onClick.AddListener(BackButtonToMainMenu_OnClick);
    }

    private void BackButtonToMainMenu_OnClick()
    {
        if(PlayerDataJson.Instance.PlayerJson == null || InventoryDataJson.Instance.inventoryJson == null) return; //! TESTING

        StartCoroutine(DelayTimeSave_ToExitGame(0.1f));
    }

    IEnumerator DelayTimeSave_ToExitGame(float time) {
        LoadDataTo_IDataPersistence.Instance.SaveData_BeforeOutOfGame();
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f; //todo free game
        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 0; //todo DUNG GAME KHI QUAY VE MAN HINH MAIN MENE
    }
}
