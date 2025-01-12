using System;
using System.Collections;
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
    [SerializeField] Button CreateAccountButton;
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

    [SerializeField] GameObject LoadingAnimation_Image;
    public TextMeshProUGUI logTxt;
    public static Action<string> OnUpdateRegisterStatus;

    [SerializeField] GameObject MainMenu_Screen;
    [SerializeField] GameObject Register_Screeen;

    private void Awake() {
        CreateAccountButton.onClick.AddListener(CreateAccountButton_OnClicked);
        LoginButton.onClick.AddListener(LoginButton_OnClicked);
        ResumeButton.onClick.AddListener(ResumeButton_OnClicked);
        AccountButton.onClick.AddListener(AccountButton_OnClicked);
        QuitButton.onClick.AddListener(QuitButton_OnClicked);

        RegisterButton.onClick.AddListener(RegisterButton_OnClicked); // registerScreen

        LoadLoginScreen.onClick.AddListener(LoginScreenButton_Onclicked);

        playFabLoginManager = FindObjectOfType<PlayFabLoginManager>();

        LoadingAnimation_Image.SetActive(false);
    }

    private void Start() {
        HandlePassChanged();
        StopAllCoroutines();
    }

    private void OnEnable() {
        OnUpdateRegisterStatus += OnUpdateRegisterStatus_MainMenuButtons;
    }
    private void OnDisable() {
        OnUpdateRegisterStatus -= OnUpdateRegisterStatus_MainMenuButtons;
    }

    private void Update() {
        EnableAccountButton();
        if(Time.timeScale == 0) Debug.Log("dang frezzeeeeee");
        else Debug.Log("dang KO frezzeeeeee");
    }

    //? kiem tra dieu kien nhap day du khi register => enable nut dang ky
    public void HandlePassChanged() {
        RegisterButton.interactable = 
            loginPassword.text.Length >= minPassLength &&
            loginPassword.text.Length <= maxPassLength;
    }

    //? dieu kien de nut Resume va Account and Login Button cho phen interactable
    void EnableAccountButton() {
        if(PlayerDataJson.Instance.IsLoadedSuccessInLogin) AccountButton.enabled = true;
        else AccountButton.enabled = false;

        if(PlayerDataJson.Instance.IsLoadedSuccessInGame) ResumeButton.enabled = true;
        else ResumeButton.enabled = false;

        //old version active and deactive CreateAccountButton and LoginButton
        /* if(PlayerDataJson.Instance.IsLoadedSuccessInLogin) {
            LoginButton.interactable = false;
            CreateAccountButton.interactable = false;
        } 
        else {
            LoginButton.interactable = true;
            CreateAccountButton.interactable = true;
        } */ 
    }

    //? Button OnCliked in Canvas MainMenu scene
    //void LoginButton_OnClicked() => TestLoadingScene.Instance.Load_Login_Scene();
    void CreateAccountButton_OnClicked() {
        if(PlayerDataJson.Instance.IsLoadedSuccessInLogin || PlayerDataJson.Instance.IsLoadedSuccessInGame) {
            ShowLogMsg("Oops! Please quit game and create a new account.");
        } else {
            MainMenu_Screen.SetActive(false);
            Register_Screeen.SetActive(true);
        }
    }

    void LoginButton_OnClicked() {
        if(PlayerDataJson.Instance.IsLoadedSuccessInLogin || PlayerDataJson.Instance.IsLoadedSuccessInGame) {
            ShowLogMsg("You are logged in");

        } else {
            LoadingAnimation_Image.SetActive(true);
            TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Login);  //! co the dung
        }
    }

    void ResumeButton_OnClicked() {
        if(TestLoadingScene.Instance.GetCurrentSceneIndex == 0) return;
        LoadingAnimation_Image.SetActive(true);

        SetTimeScale.UnFrezzeGame();
        TestLoadingScene.Instance.ResumeGame(); // can phai duoc xet gia tri currentSceneIndex ngay khi back to main menu
    }

    //void AccountButton_OnClicked() => TestLoadingScene.Instance.Load_AccountDataOverview_Scene();
    void AccountButton_OnClicked() {
        LoadingAnimation_Image.SetActive(true);
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.AccountDataOverview);    //! co the dung

    }
    
    void QuitButton_OnClicked() => Application.Quit();

    //? Button OnClicked in Register Screen 
    void RegisterButton_OnClicked() {
        LoadingAnimation_Image.SetActive(true);
        playFabLoginManager.OnRegisterPressed();
        /* Register()_PlayfabLoginManager.cs cl 25
        if sucessed -> khoi tao new PlayerJson(email, username) AND new InventoryJson
        Save_PlayerDataJason_SignUp()_PlayerDataJson.cs -> chuyen new PlayerJson -> string -> luu len playfab "Json'
        Save_InventoryDataJson_SignUp()_PlayerInventory.cs -> chuyen new InventoryJson -> stirng -> save Playfab "InventoryJson" */
    }

    void LoginScreenButton_Onclicked() {
        LoadingAnimation_Image.SetActive(true);

        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Login); // truyen vao tham so kieu enume
    }

    void OnUpdateRegisterStatus_MainMenuButtons(string str) {
        ShowLogMsg(str);
    }

    void ShowLogMsg(string msg)
    {
        logTxt.text = msg;
        Debug.Log($"__________ co vao showlog");
        StartCoroutine(TextFadeOut(3f));
    }
    IEnumerator TextFadeOut(float time) {
        Debug.Log($"__________ co vao TextFadeOut");
        yield return new WaitForSeconds(time);
        logTxt.text = "";
        Debug.Log($"__________ co vao set string = ");
        if (LoadingAnimation_Image != null)
            LoadingAnimation_Image.SetActive(false);
    }

    //todo
}
