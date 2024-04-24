using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//todo gameobject = PlayerInfo_UI ben trong (UI_Canvas_InGame)
//todo doi tuong chua canvas hien thi thong tin name, level, health, healthSlider
public class PlayerInfo_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private PlayerDataLocal_Temp playerDataLocal_Temp;
    [SerializeField] private PlayerDataJson playerDataJson;

    PlayerHealth playerHealth;

    private void Awake() {
        healthSlider = FindObjectOfType<Slider>();
        playerDataLocal_Temp = FindObjectOfType<PlayerDataLocal_Temp>();
        playerDataJson = FindObjectOfType<PlayerDataJson>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Start() {
        StartCoroutine(ShowPlayerInfo_GameUI_Countine(0.5f));
    }

    private void ShowPlayerInfo_GameUI(string userName, int level, int health) {
        this.userName.text =""+ userName;
        this.level.text ="Lv: "+ level.ToString();
        this.health.text =""+ health.ToString();

        healthSlider.value = health;
        playerHealth.SetCurrentHealth = health;
    }
    IEnumerator ShowPlayerInfo_GameUI_Countine(float time) {
        yield return new WaitForSeconds(time);
        ShowPlayerInfo_GameUI(playerDataLocal_Temp.userName,
                            playerDataLocal_Temp.level,
                            playerDataLocal_Temp.health);
    }

    public void LoadMainMenuScene_BackButtonInGame() {
        Debug.Log("co nhan back to main menu");
        Time.timeScale = 0f; //free
        SceneManager.LoadSceneAsync("MainMenu");
        playerDataLocal_Temp.BackButtonPress_SavePlayerDataJson();
    }

}
