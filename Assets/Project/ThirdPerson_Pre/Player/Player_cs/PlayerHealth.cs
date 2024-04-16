using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    private const string SLIDER_HEALTH =  "Slider Health";
    private const string HEALTH_TEXT =  "Health Text";

    private AiRagdoll aiRagdoll;
    private ActiveGun activeGun;
    private ChracterAim characterAim;
    private Animator animator;
    private CameraManager cameraManager;
    private PlayerGun playerMovement;

    Slider sliderHealth;
    TMPro.TMP_Text healthText;
    protected override void OnStart() {
        aiRagdoll = GetComponent<AiRagdoll>();
        activeGun = GetComponent<ActiveGun>();
        characterAim = GetComponent<ChracterAim>();
        playerMovement = GetComponent<PlayerGun>();
        animator = GetComponent<Animator>();
        cameraManager = FindObjectOfType<CameraManager>();
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

    }
    protected override void OnDamage(Vector3 direction) {
        Update_Virtual();                   // hieu ung man hinh khi get damage || animation get damage.
        UpdateSliderHealth();               // tang giam slider health
    }

    protected override void OnHeal(float amount) {
        
    }

    private void Update_Virtual() {
        // nhung thay doi hieu ung khi get damage or increase health
        animator.SetBool("GetDamage", true);
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
}
