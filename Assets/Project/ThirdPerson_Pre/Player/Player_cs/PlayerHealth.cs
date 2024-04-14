using UnityEngine;

public class PlayerHealth : Health
{
    private AiRagdoll aiRagdoll;
    private ActiveGun activeGun;
    private ChracterAim chracterAim;
    private Animator animator;
    private CameraManager cameraManager;
    private PlayerGun playerMovement;
    protected override void OnStart() {
        aiRagdoll = GetComponent<AiRagdoll>();
        activeGun = GetComponent<ActiveGun>();
        chracterAim = GetComponent<ChracterAim>();
        animator = GetComponent<Animator>();
        cameraManager = FindObjectOfType<CameraManager>();
    }
    protected override void OnDeath(Vector3 direction) {
        aiRagdoll.ActiveRag();
        direction.y = 1;
        aiRagdoll.ApplyForceLying(direction);
        activeGun.DropWeapon();
        chracterAim.enabled = false; // tat chatacterAim.cs
        ////playerMovement.enabled = false;
        cameraManager.ActiveDeathCam();

    }
    protected override void OnDamage(Vector3 direction) {
        Update_Virtual();
    }

    protected override void OnHeal(float amount) {
        
    }

    private void Update_Virtual() {
        // nhung thay doi hieu ung khi get damage or increase health
        animator.SetBool("GetDamage", true);
    }
}
