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

    private void Awake() {
        LoginButton.onClick.AddListener(LoginButton_OnClick);
        BackMainMenuButton.onClick.AddListener(BackMainMenuButton_OnClicked);

        RequestResetButton.onClick.AddListener(SendResetPassWord_OnClick);
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

        StartCoroutine(DelayTimeLogin_ToLoad_CO(4f)); //sau 3s chuyen qua onverview
    }

    IEnumerator DelayTimeLogin_ToLoad_CO(float time) {
        playFabLoginManager.OnLoginPressed();

        //load "Json" playfab -> PlayerJson -> doi tuong tronng PlayerDataJson.cs
        yield return new WaitForSeconds(time);
        playerDataJson.Load_PlayerDataJason_RealTime();

        // load "InvneotryJson" -> InventoryJosn -> doi tuong trong IventoryDataJson.cs
        yield return new WaitForSeconds(time);
        InventoryDataJson.Instance.Load_InventoryDataJason_RealTime();
        
        // load next scene - player information overview (playerInfo + inventoryInfo)
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    //void BackMainMenuButton_OnClicked() => TestLoadingScene.Instance.Load_MainMenu_Scene();
    void BackMainMenuButton_OnClicked() {
        //TestLoadingScene.Instance.LoadScene(TestLoadingScene.MainMenu_Scene);
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }
    
    void SendResetPassWord_OnClick() => playFabLoginManager.OnSendResetPressed();
    //todo
}
