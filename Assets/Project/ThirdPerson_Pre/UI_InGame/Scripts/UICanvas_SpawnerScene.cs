using System;
using UnityEngine;
using UnityEngine.UI;

//? gameobject = UI_canvas in game in spanwerscene

public class UICanvas_SpawnerScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenuBtton;
    [SerializeField] Button JoinGameButton_1; // nut join khi da xem trang bi
    [SerializeField] Button JoinGameButton_2; // nut join khi da xem trang bi

    [SerializeField] Button ChangeNextSkinsButton;
    [SerializeField] Button ChangePreviousSkinsButton;


    private void Awake() {
        JoinGameButton_1.onClick.AddListener(JoinGameButton_1_OnClick);
        JoinGameButton_2.onClick.AddListener(JoinGameButton_2_OnClick);

        BackToMainMenuBtton.onClick.AddListener(BackToMainMenuBtton_OnClick);
        ChangeNextSkinsButton.onClick.AddListener(ChangeNextSkinsButton_OnClick);
        ChangePreviousSkinsButton.onClick.AddListener(ChangePreviousSkinsButton_OnClick);
    }


    void JoinGameButton_1_OnClick() {
        Time.timeScale = 1; //todo BAT DAU GAME
        //TestLoadingScene.Instance.LoadScene(TestLoadingScene.TestingThirdPerson_Scene);
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson);
    }
    
    private void JoinGameButton_2_OnClick()
    {
        Time.timeScale = 1; //todo BAT DAU GAME
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale);
    }

    void BackToMainMenuBtton_OnClick() {
        //TestLoadingScene.Instance.LoadScene(TestLoadingScene.MainMenu_Scene);
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }

    void ChangeNextSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkin();
    }

    void ChangePreviousSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkinPrevious();
    }

}
