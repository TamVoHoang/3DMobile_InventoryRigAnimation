using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//? gameobject = UI_canvas in spanwer scene

public class UICanvas_SpawnerScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenuBtton;
    [SerializeField] Button JoinGameButton; // nut join khi da xem trang bi

    private void Awake() {
        JoinGameButton.onClick.AddListener(JoinGameButton_OnClick);
        BackToMainMenuBtton.onClick.AddListener(BackToMainMenuBtton_OnClick);
    }
    void JoinGameButton_OnClick() {
        TestLoadingScene.Instance.LoadScene(TestLoadingScene.TestingThirdPerson_Scene);
    }

    void BackToMainMenuBtton_OnClick() {
        TestLoadingScene.Instance.LoadScene(TestLoadingScene.MainMenu_Scene);
    }
}
