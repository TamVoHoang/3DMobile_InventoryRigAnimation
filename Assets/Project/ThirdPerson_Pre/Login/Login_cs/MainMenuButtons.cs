using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    // [SerializeField] TMP_InputField loginEmail; // tao bien tai day de nhat value changed
    // [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField loginPassword;
    [SerializeField] private int minPassLength =1;
    [SerializeField] private int maxPassLength =12;

    [SerializeField] Button AccountButton;
    [SerializeField] Button ResumeButton;
    [SerializeField] Button RegisterButton;

    private void Start() {
        HandlePassChanged();
    }
    //? kiem tra dieu kien nhap day du khi register => enable nut dang ky
    public void HandlePassChanged() {
        RegisterButton.interactable = 
            loginPassword.text.Length >= minPassLength &&
            loginPassword.text.Length <= maxPassLength;
    }

    private void Update() {
        EnableAccountButton();
    }

    //? dieu kien de nut Resume va Account cho phen interactable
    void EnableAccountButton() {
        if(PlayerDataJson.Instance.IsLoadedSuccessInLogin) AccountButton.enabled = true;
        else AccountButton.enabled = false;

        if(PlayerDataJson.Instance.IsLoadedSuccessInGame) ResumeButton.enabled = true;
        else ResumeButton.enabled = false;
    }

    public void ResumeGame_ResumeButton() {
        TestLoadingScene.Instance.ResumeGame();
    }

    public void Load_AccountDataOverviewScene_AccountButton() => 
        TestLoadingScene.Instance.Load_AccountDataOverview_Scene();

    public void QuitGame_OnQuitButton() => Application.Quit();

    public void Load_Login_Scene() => TestLoadingScene.Instance.Load_Login_Scene();

}
