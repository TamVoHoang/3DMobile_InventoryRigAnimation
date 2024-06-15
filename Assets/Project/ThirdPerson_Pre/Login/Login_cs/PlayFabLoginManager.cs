using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;

//? game object = PlayFabLoginManger
//? input field -> register - save playerJson local to "Json"
//? save InventoryJson local to "InventoryJson"

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

    // Register Button in Register_Screen call this function - sau khi da nhap mail va pass
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

            //? email user name - gan vao PlayerJson - save len "Json" PlayFab
            PlayerDataJson.Instance.Save_PlayerDataJason_SignUp(email,username);
            
            //? khoi tao new inventory - gan vao InventoryJson - save len "InventoryJson" PlayFab
            StartCoroutine(SaveInventoryDataJson_ToSignUpContine(4)); //! neu luu ngay lap tuc inventoryJson se bi loi
        }, 
        PlayFabFailure);
    }

    IEnumerator SaveInventoryDataJson_ToSignUpContine(float time) {
        yield return new WaitForSeconds(time);
        InventoryDataJson.Instance.Save_InventoryDataJson_SignUp();
    }

    #endregion Register

    //todo Login
    #region Login
    [Header("Login UI: ")]
    [SerializeField] TMP_InputField loginEmail;
    [SerializeField] TMP_InputField loginPassword;
    [SerializeField] TextMeshProUGUI ResultLogin_Text;
    
    public void OnLoginAutoPressed() => 
        Login(PlayerPrefs.GetString(LAST_EMAIL_KEY), PlayerPrefs.GetString(LAST_PASSWORD_KEY));
    
    // Login Button in Login Scene call coll 47_LoginButton.cs
    public void OnLoginPressed() => Login(loginEmail.text, loginPassword.text);

    private void Login(string mail, string password) {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest() {
                Email = mail,
                Password = password,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams() {
                GetPlayerProfile = true,
            }
        },
        successResult => {
            // xet loggedPayfabID -> sent qua cho leader board hien thi mau text
            PlayerDataJson.Instance.loggedPayfabID = successResult.PlayFabId; //! vua them vao

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
