using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButtons : MonoBehaviour
{
    public const string LAST_MAIL = "last_mail", PASS = "pass";
    [SerializeField] PlayFabLoginManager playFabLoginManager;
    [SerializeField] TMP_InputField loginEmail;
    [SerializeField] TMP_InputField loginPassword;
    [SerializeField] private int minPassLength =1;
    [SerializeField] private int maxPassLength =12;
    [SerializeField] Button LoginButton;
    PlayerDataJson playerDataJson;
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

    //? khi nhan nut Loggin
    public void OnLoginButton_Pressed() {
        PlayerPrefs.SetString(LAST_MAIL, loginEmail.text);
        PlayerPrefs.SetString(PASS, loginPassword.text);

        StartCoroutine(DelayTimeLogin_ToLoad(4f)); //sau 3s chuyen qua onverview
    }

    public void Load_MainMenuSence_OnMainMenuButton() => TestLoadingScene.Instance.Load_MainMenu_Scene();

    public void SendResetPassWord() {
        playFabLoginManager.OnSendResetPressed();
    }

    IEnumerator DelayTimeLogin_ToLoad(float time) {
        playFabLoginManager.OnLoginPressed();
        yield return new WaitForSeconds(time);
        playerDataJson.Load_PlayerDataJason_RealTime();
        yield return new WaitForSeconds(time);
        InventoryDataJson.Instance.Load_InventoryDataJason_RealTime();
        
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    //todo
}
