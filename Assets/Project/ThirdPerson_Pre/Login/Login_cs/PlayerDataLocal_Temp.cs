using UnityEngine;

public class PlayerDataLocal_Temp :Singleton<PlayerDataLocal_Temp>
{
    //noi chua data player local_temp tu playerDatajonServer
    public string mail, userName;
    public int level, health, killed, died;
    
    //public string userName;
    // public int health;
    // public int killed;
    // public int died;

    public Vector3 position_Temp, rotation_Temp;

    private PlayerDataJson playerDataJson;
    
    protected override void Awake() {
        base.Awake();
        playerDataJson = GetComponent<PlayerDataJson>();
    }
    private void Start() {
        playerDataJson.OnPlayerDataLocalChanged += PlayerDataJson_OnPlayerDataLocalChanged;
    }

    private void PlayerDataJson_OnPlayerDataLocalChanged(object sender, PlayerDataJson.PlayerData e) {
        SetPlayerDataLocalTemp_FromPlayerDataJson(e.mail, e.userName, e.level, e.health, e.killed, e.died,
                                                    e.Position);
    }

    private void SetPlayerDataLocalTemp_FromPlayerDataJson(string mail,
        string userName, int level, int health, int killed, int died, Vector3 Position) {
        this.mail = mail;
        this.userName = userName;
        this.level = level;
        this.health = health;
        this.killed = killed;
        this.died = died;

        this.position_Temp = Position;
    }

    // Back mainMenu Button in Game Press
    public void BackButtonPress_SavePlayerDataJson() {
        playerDataJson.Save_PlayerDataJason_RealTime();
    }

//todo
}
