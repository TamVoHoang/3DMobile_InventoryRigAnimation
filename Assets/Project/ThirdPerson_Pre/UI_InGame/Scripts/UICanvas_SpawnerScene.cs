using UnityEngine;
using UnityEngine.UI;

//? gameobject = UI_canvas in game in spanwerscene

public class UICanvas_SpawnerScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenuBtton;
    [SerializeField] Button JoinGameButton; // nut join khi da xem trang bi
    [SerializeField] Button ChangeNextSkinsButton;
    [SerializeField] Button ChangePreviousSkinsButton;


    private void Awake() {
        JoinGameButton.onClick.AddListener(JoinGameButton_OnClick);
        BackToMainMenuBtton.onClick.AddListener(BackToMainMenuBtton_OnClick);
        ChangeNextSkinsButton.onClick.AddListener(ChangeNextSkinsButton_OnClick);
        ChangePreviousSkinsButton.onClick.AddListener(ChangePreviousSkinsButton_OnClick);
    }
    void JoinGameButton_OnClick() {
        Time.timeScale = 1; //todo BAT DAU GAME
        TestLoadingScene.Instance.LoadScene(TestLoadingScene.TestingThirdPerson_Scene);
    }

    void BackToMainMenuBtton_OnClick() {
        TestLoadingScene.Instance.LoadScene(TestLoadingScene.MainMenu_Scene);
    }

    void ChangeNextSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkin();
    }

    void ChangePreviousSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkinPrevious();
    }

}
