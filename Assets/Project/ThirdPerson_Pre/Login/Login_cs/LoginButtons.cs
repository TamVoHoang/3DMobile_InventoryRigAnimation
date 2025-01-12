using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//? game Object = canvas trong Login Scene

public class LoginButtons : MonoBehaviour
{
    public const string LAST_MAIL = "last_mail", PASS = "pass";
    [SerializeField] PlayFabLoginManager playFabLoginManager;
    [SerializeField] TMP_InputField loginEmail;
    [SerializeField] TMP_InputField loginPassword;
    [SerializeField] private int minPassLength =1;
    [SerializeField] private int maxPassLength =12;

    // Buttons
    [Header("Login Screen")]
    PlayerDataJson playerDataJson;
    [SerializeField] Button LoginButton; // loginButotn AT Login Screen (Login Scene)
    [SerializeField] Button BackMainMenuButton; // in login screen

    [Header("Reset Password Screen")]
    [SerializeField] Button RequestResetButton; // in reset password screen

    [SerializeField] GameObject LoadingAnimation_Image;

    [SerializeField] const float DELAYTIME_TO_OVERVIEW_SCENE = 2f;
    public TextMeshProUGUI logTxt;
    public static Action<string> OnUpdateLoginStatus;

    private void Awake() {
        LoginButton.onClick.AddListener(LoginButton_OnClick);
        BackMainMenuButton.onClick.AddListener(BackMainMenuButton_OnClicked);

        RequestResetButton.onClick.AddListener(SendResetPassWord_OnClick);
        LoadingAnimation_Image.SetActive(false);
        OnUpdateLoginStatus += OnUpdateLoginStatus_LoginButton;
    }

    void Start()
    {
        playFabLoginManager = FindObjectOfType<PlayFabLoginManager>();
        playerDataJson = FindObjectOfType<PlayerDataJson>();

        if(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            return;
        }
        loginEmail.text = PlayerPrefs.GetString(LAST_MAIL, string.Empty);
        loginPassword.text = PlayerPrefs.GetString(PASS, string.Empty);

        HandlePassChanged();
    }

    private void OnEnable() {
        OnUpdateLoginStatus += OnUpdateLoginStatus_LoginButton;
    }
    private void OnDisable() {
        OnUpdateLoginStatus -= OnUpdateLoginStatus_LoginButton;
    }

    //? add vao nut InputField - value changed
    public void HandlePassChanged() {
        LoginButton.interactable = 
            loginPassword.text.Length >= minPassLength &&
            loginPassword.text.Length <= maxPassLength;
    }

    //? khi nhan nut Loggin - load data 
    void LoginButton_OnClick() {
        PlayerPrefs.SetString(LAST_MAIL, loginEmail.text);
        PlayerPrefs.SetString(PASS, loginPassword.text);

        // hien thi loading animation
        LoadingAnimation_Image.SetActive(true);

        //sau 3s chuyen qua onverview
        
        StartCoroutine(DelayTimeLogin_ToLoad_CO(DELAYTIME_TO_OVERVIEW_SCENE));
        
    }

    IEnumerator DelayTimeLogin_ToLoad_CO(float time) {
        playFabLoginManager.OnLoginPressed(); // lay mail and pass loginPlayfab

        //load "Json" playfab -> PlayerJson -> doi tuong tronng PlayerDataJson.cs
        yield return new WaitForSeconds(time);
        playerDataJson.Load_PlayerDataJason_RealTime();

        // load "InvneotryJson" -> InventoryJosn -> doi tuong trong IventoryDataJson.cs
        yield return new WaitForSeconds(time);
        InventoryDataJson.Instance.Load_InventoryDataJason_RealTime();
        
        // load next scene - player information overview (playerInfo + inventoryInfo)
        yield return new WaitForSeconds(time);
        /* SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1); */

        SceneManager.LoadScene(TestLoadingScene.AccountOverview_Scene);

    }

    //void BackMainMenuButton_OnClicked() => TestLoadingScene.Instance.Load_MainMenu_Scene();
    void BackMainMenuButton_OnClicked() {
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }
    
    void SendResetPassWord_OnClick() => playFabLoginManager.OnSendResetPressed();

    void OnUpdateLoginStatus_LoginButton(string str) {
        ShowLogMsg(str);
    }

    void ShowLogMsg(string msg)
    {
        logTxt.text = msg;
        StartCoroutine(TextFadeOut(3f));
    }
    IEnumerator TextFadeOut(float time) {
        yield return new WaitForSeconds(time);
        logTxt.text = "";

        if (LoadingAnimation_Image != null)
            LoadingAnimation_Image.SetActive(false);
    }
    //todo
}
