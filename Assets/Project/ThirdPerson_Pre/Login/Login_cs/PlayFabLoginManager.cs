using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;

public class PlayFabLoginManager : MonoBehaviour
{
    const string LAST_EMAIL_KEY = "LAST_EMAIL", LAST_PASSWORD_KEY = "LAST_PASSWORD";
    [SerializeField] PlayerDataJson playerDataJson;


//todo Register
    #region Register
    [Header("Register UI: ")]
    [SerializeField] TMP_InputField registerEmail;
    [SerializeField] TMP_InputField registerUnsername;
    [SerializeField] TMP_InputField registerPassword;
    [SerializeField] TextMeshProUGUI ResultRegister_Text;

    public void OnRegisterPressed() {
        Register(registerEmail.text, registerUnsername.text, registerPassword.text);

    }
    private void Register(string email,string username, string password) {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest() {
            Email = email,
            DisplayName = username,
            Password = password,
            RequireBothUsernameAndEmail = false
        },
        successResult => {
            Login(email, password);
            ResultRegister_Text.text = "Register Success";

            //? khoi tao doi tuong PlayerJson ko tham so - save doi tuong nay
            /* string vector3ToString = JsonUtility.ToJson(playerDataJson.InitialVector3Player_ToRegister);
            PlayerJson playerJson_Register  = new PlayerJson() {
                mail = email,
                name = username,
                level = 1,
                health = 500,
                killed = 0,
                died = 0,

                position = vector3ToString //! dang kiem tra thu
            };
            playerDataJson.Save_PlayerJson_ToResiger(playerJson_Register); */

            //? khoi tao doi tuong InventoryJson ko tham so - luu doi tuong nay
            PlayerDataJson.Instance.Save_PlayerDataJason_SignUp(email,username);
            StartCoroutine(Delaytime(4));
        }, 
        PlayFabFailure);

    }
    #endregion Register
    IEnumerator Delaytime(float time) {
        yield return new WaitForSeconds(time);
        //InventoryDataJson.Instance.Save_InventoryDataJason_RealTime(InventoryDataJson.Instance.InventoryJson);
        InventoryDataJson.Instance.Save_InventoryDataJson_SignUp();
    }

//todo Login
    #region Login
    [Header("Login UI: ")]
    [SerializeField] TMP_InputField loginEmail;
    [SerializeField] TMP_InputField loginPassword;
    [SerializeField] TextMeshProUGUI ResultLogin_Text;
    
    public void OnLoginAutoPressed() => 
        Login(PlayerPrefs.GetString(LAST_EMAIL_KEY), PlayerPrefs.GetString(LAST_PASSWORD_KEY));
    public void OnLoginPressed() =>
        Login(loginEmail.text, loginPassword.text);

    private void Login(string mail, string password) {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest() {
            Email = mail,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams() {
                GetPlayerProfile = true
            }
        },
        successResult => {
            PlayerPrefs.SetString(LAST_EMAIL_KEY, mail);
            PlayerPrefs.SetString(LAST_PASSWORD_KEY, password);
            PlayerPrefs.SetString("Username", successResult.InfoResultPayload.PlayerProfile.DisplayName);

            Debug.Log("Successfully Logged In User: " + PlayerPrefs.GetString("Username"));
            ResultLogin_Text.text = "Successfully Logged In User: " + PlayerPrefs.GetString("Username"); // hien thi ket qua khi login thanh cong
        },
        PlayFabFailure);
    }

    #endregion Login

//todo Forgot password
[Header("Forgot password UI: ")]
    #region Forgot password
    [SerializeField] TMP_InputField resetEmailInput;
    [SerializeField] TextMeshProUGUI ResultRequest_Text;

    public void OnSendResetPressed() {
        SendResetMail(resetEmailInput.text);
    }
    private void SendResetMail(string mail) {
        PlayFabClientAPI.SendAccountRecoveryEmail(new SendAccountRecoveryEmailRequest() {
            Email = mail,
            TitleId = "C1AD6"
        },
        successResult => {
            Debug.Log("Successfully sent reset mail");
            ResultRequest_Text.text = "Successfully sent reset mail";
        },
        PlayFabFailure);
    }

    #endregion Forgot password

    private void PlayFabFailure(PlayFabError error) {
        Debug.Log(error.Error + " : " + error.GenerateErrorReport());
        ResultRegister_Text.text = error.Error + " : " + error.GenerateErrorReport();
        ResultLogin_Text.text = error.Error + " : " + error.GenerateErrorReport();
        ResultRequest_Text.text = error.Error + " : " + error.GenerateErrorReport();
    }

    //todo
}
