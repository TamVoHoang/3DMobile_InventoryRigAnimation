using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataLocal_Temp :Singleton<PlayerDataLocal_Temp>
{
    //noi chua data player local_temp tu playerDatajonServer
    
    public string mail;
    public string userName;
    public int level;
    public int health;
    public int killed;
    public int died;

    private PlayerDataJson playerDataJson;
    
    protected override void Awake() {
        base.Awake();
        playerDataJson = GetComponent<PlayerDataJson>();
    }
    private void Start() {
        playerDataJson.OnPlayerDataLocalChanged += PlayerDataJson_OnPlayerDataLocalChanged;
    }

    private void PlayerDataJson_OnPlayerDataLocalChanged(object sender, PlayerDataJson.PlayerData e) {
        SetPlayerDataLocalTemp_FromPlayerDataJson(e.mail, e.userName, e.level, e.health, e.killed, e.died);
    }

    private void SetPlayerDataLocalTemp_FromPlayerDataJson(string mail, string userName, int level, int health, int killed, int died) {
        this.mail = mail;
        this.userName = userName;
        this.level = level;
        this.health = health;
        this.killed = killed;
        this.died = died;
    }

    // Back mainMenu Button in Game Press
    public void BackButtonPress_SavePlayerDataJson() {
        playerDataJson.Save_PlayerDataJason_RealTime();
    }

//todo
}
