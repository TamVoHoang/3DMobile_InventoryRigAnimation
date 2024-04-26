using UnityEngine;

public class PlayerDataLocal_Temp :Singleton<PlayerDataLocal_Temp>, IDataPersistence
{
    //noi chua data player local_temp tu playerDatajonServer
    public string mail, userName;
    public int level;
    public int health;
    public int killed;
    public int died;

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

    public void SetPlayerDataLocalTemp_FromPlayerDataJson(string mail,
        string userName, int level, int health, int killed, int died, Vector3 position) {
        this.mail = mail;
        this.userName = userName;
        this.level = level;
        this.health = health;
        this.killed = killed;
        this.died = died;

        this.position_Temp = position;
    }

    // Back mainMenu Button in Game Press
    public void BackButtonPress_SavePlayerDataJson() {
        playerDataJson.Save_PlayerDataJason_RealTime();
    }

    public void LoadData(PlayerJson playerJsonData)
    {
        this.mail = playerJsonData.mail;
        this.userName = playerJsonData.name;
        this.level = playerJsonData.level;
        this.health = playerJsonData.health;
        this.killed = playerJsonData.killed;
        this.died = playerJsonData.died;

        this.position_Temp = JsonUtility.FromJson<Vector3>(playerJsonData.position);
    }

    public void SaveData(PlayerJson playerJsonData)
    {
        
    }

    //todo
}
