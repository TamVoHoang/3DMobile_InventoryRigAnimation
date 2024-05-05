using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health, IDataPersistence
{
    private const string SLIDER_HEALTH =  "Slider Health";
    private const string HEALTH_TEXT =  "Health Text";
    [SerializeField] private float delayTimeToRespawn = 5f;
    [SerializeField] private float delayTimeToReadyGetDamage = 5f; // co 5s ko an dan sau khi respawn
    private AiRagdoll aiRagdoll;
    private ActiveGun activeGun;
    private ChracterAim characterAim;
    private Animator animator;
    private CameraManager cameraManager;
    private PlayerGun playerMovement;
    private int diedCount;
    public int GetDiedCount => diedCount;
    //private bool isGetDiedPoint = false; // die bi tinh diem tru

    Slider sliderHealth;
    TMPro.TMP_Text healthText;

    protected override void OnStart() {
        Debug.Log("OnStart() PlayerHealth.cs run");

        //SetCurrentHealth = MaxHealth; // neu ko tinh luong duoc luu, thi xet mac dinh maxHealth
        diedCount = 0;
        lowHealthLimit = MaxHealth;  //? xet rieng lowHealth cho player
        isReadyToTakeDamage = true;     // true - san sang bi tru mau
        //isGetDiedPoint = false;         // chau bi tru diem died
        aiRagdoll = GetComponent<AiRagdoll>();
        activeGun = GetComponent<ActiveGun>();
        characterAim = GetComponent<ChracterAim>();
        playerMovement = GetComponent<PlayerGun>();
        animator = GetComponent<Animator>();
        cameraManager = FindObjectOfType<CameraManager>();
        
        // LoadData(PlayerDataJson.Instance.PlayerJson);//! load bang interface
        UpdateSliderHealth();               // tang giam slider health
    }
    protected override void OnDeath(Vector3 direction) {

        aiRagdoll.ActiveRag();
        direction.y = 1;
        aiRagdoll.ApplyForceLying(direction);
        activeGun.DropWeapon();

        cameraManager.ActiveDeathCam();     //show cam nhin player va enemy
        characterAim.enabled = false;       //tat chatacterAim.cs
        playerMovement.enabled = false;     //tat khong cho player move

        // died point +
        if(IsDead && isReadyToTakeDamage) {
            isReadyToTakeDamage = false;
            ////PlayerDataLocal_Temp.Instance.died += 1; //todo tang so luong die
            diedCount ++;
        }

        //chua nbi respawn song lai
        StartCoroutine(RespawnPlayerCountine(delayTimeToRespawn));
    }
    protected override void OnDamage(Vector3 direction) {
        Update_Virtual();                   // hieu ung man hinh khi get damage || animation get damage.
        UpdateSliderHealth();               // tang giam slider health

        //PlayerDataLocal_Temp.Instance.health = (int)CurrentHealth;
    }

    protected override void OnHeal(float amount) {
        // Heal(amount) coll 48 Health.cs - co virtual - ovverride tai this.cs
        // co the dung de thay doi hieu ung tai day
        // cap nhat thanh mau tai day
        UpdateSliderHealth();
        //PlayerDataLocal_Temp.Instance.health = (int)CurrentHealth;
    }

    private void Update_Virtual() {
        // nhung thay doi hieu ung khi get damage or increase health
        //animator.SetBool("GetDamage", true);
        animator.SetTrigger("GetDamage");
    }
    
    // tang giam slider health cho player.
    private void UpdateSliderHealth() {
        if (sliderHealth == null && healthText == null) {
            sliderHealth = GameObject.Find(SLIDER_HEALTH).GetComponent<Slider>();
            healthText = GameObject.Find(HEALTH_TEXT).GetComponent<TMPro.TMP_Text>();
        }

        sliderHealth.maxValue = MaxHealth;
        sliderHealth.value = CurrentHealth;
        healthText.text = CurrentHealth.ToString();
    }

    // lam cho player song lai
    private void RespawnPlayer() {
        Debug.Log("Respawn player");
        isReadyToTakeDamage = false; // ko bi tru mau du trung dan
        aiRagdoll.DeactiveRag();
        cameraManager.DeActiveDeathCam();
        ResetCurrentHealth();
        UpdateSliderHealth();
        characterAim.enabled = true;
        playerMovement.enabled = true;
        StartCoroutine(ReadyGetDamageCountine(delayTimeToReadyGetDamage));
    }
    IEnumerator RespawnPlayerCountine(float time) {
        yield return new WaitForSeconds(time);
        RespawnPlayer();
    }
    IEnumerator ReadyGetDamageCountine(float time) {
        //doi mau bat tu tai day
        yield return new WaitForSeconds(time);
        isReadyToTakeDamage = true; //bat dau bi tru mau
    }

    public void LoadPlayerData(PlayerJson playerJsonData) {
        SetCurrentHealth = playerJsonData.health;
        //diedCount = playerJsonData.died; //? OK
    }

    public void SavePlayerData(PlayerJson playerJsonData) {
        playerJsonData.health = (int)CurrentHealth;
        playerJsonData.died += diedCount;
    }
}
