using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//? gameobject = canvas MainMenu ben trong scene MainMenu
public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] TMP_InputField loginPassword;
    [SerializeField] private int minPassLength =1;
    [SerializeField] private int maxPassLength =12;

    // Button in MainMenu scene
    [Header("MainMneu Screen")]
    [SerializeField] Button LoginButton; // load Login Scene - button at MainScreen (MainMenu Scene)
    [SerializeField] Button ResumeButton;

    // chi enable khi da load sau khi da thuc hien login thuc su bang tia khoan vua sang ky (co cai de account de vao xem)
    [SerializeField] Button AccountButton; 
    [SerializeField] Button QuitButton;

    // Button in Register Screen
    [Header("Register Screen")]
    [SerializeField] PlayFabLoginManager playFabLoginManager;
    [SerializeField] Button RegisterButton; // nut dang ky sau khi nhap mail va pass
    [SerializeField] Button LoadLoginScreen; // load Login_Scene - button at ResgisterScreen (mainMenuScene)

    [Header("Sound")]
    PlaySoundOnMenuScenes playSoundOnMainMenu;

    private void Awake() {
        LoginButton.onClick.AddListener(LoginButton_OnClicked);
        ResumeButton.onClick.AddListener(ResumeButton_OnClicked);
        AccountButton.onClick.AddListener(AccountButton_OnClicked);
        QuitButton.onClick.AddListener(QuitButton_OnClicked);

        RegisterButton.onClick.AddListener(RegisterButton_OnClicked); // registerScreen

        ////LoadLoginScreen.onClick.AddListener(TestLoadingScene.Instance.Load_Login_Scene); // sau khi dang ki chuyen scene de Login
        LoadLoginScreen.onClick.AddListener(LoginScreenButton_Onclicked);

        playFabLoginManager = FindObjectOfType<PlayFabLoginManager>();
    }

    private void Start() {
        HandlePassChanged();
    }

    private void Update() {
        EnableAccountButton();
    }

    //? kiem tra dieu kien nhap day du khi register => enable nut dang ky
    public void HandlePassChanged() {
        RegisterButton.interactable = 
            loginPassword.text.Length >= minPassLength &&
            loginPassword.text.Length <= maxPassLength;
    }

    //? dieu kien de nut Resume va Account cho phen interactable
    void EnableAccountButton() {
        if(PlayerDataJson.Instance.IsLoadedSuccessInLogin) AccountButton.enabled = true;
        else AccountButton.enabled = false;

        if(PlayerDataJson.Instance.IsLoadedSuccessInGame) ResumeButton.enabled = true;
        else ResumeButton.enabled = false;
    }

    //? Button OnCliked in Canvas MainMenu scene
    //void LoginButton_OnClicked() => TestLoadingScene.Instance.Load_Login_Scene();
    void LoginButton_OnClicked() {
        ////TestLoadingScene.Instance.LoadScene(TestLoadingScene.Login_Scene);
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Login);

    }

    void ResumeButton_OnClicked() {
        Time.timeScale = 1; //todo UnFree game when resume game
        TestLoadingScene.Instance.ResumeGame(); // can phai duoc xet gia tri currentSceneIndex ngay khi back to main menu

    }

    //void AccountButton_OnClicked() => TestLoadingScene.Instance.Load_AccountDataOverview_Scene();
    void AccountButton_OnClicked() {
        //TestLoadingScene.Instance.LoadScene(TestLoadingScene.AccountOverview_Scene); // den scene dang ky
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.AccountDataOverview);
    }
    
    void QuitButton_OnClicked() => Application.Quit();

    //? Button OnClicked in Register Screen 
    void RegisterButton_OnClicked() {
        playFabLoginManager.OnRegisterPressed();
        /* Register()_PlayfabLoginManager.cs cl 25
        if sucessed -> khoi tao new PlayerJson(email, username) AND new InventoryJson
        Save_PlayerDataJason_SignUp()_PlayerDataJson.cs -> chuyen new PlayerJson -> string -> luu len playfab "Json'
        Save_InventoryDataJson_SignUp()_PlayerInventory.cs -> chuyen new InventoryJson -> stirng -> save Playfab "InventoryJson" */
    }

    void LoginScreenButton_Onclicked() {
        ////TestLoadingScene.Instance.LoadScene(TestLoadingScene.Login_Scene); // di den login_Scene
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Login); // truyen vao tham so kieu enume

    }

    
    //todo
}
