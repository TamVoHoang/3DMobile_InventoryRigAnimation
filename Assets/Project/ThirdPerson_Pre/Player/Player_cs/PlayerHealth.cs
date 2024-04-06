using UnityEngine;

public class PlayerHealth : Health
{
    private AiRagdoll aiRagdoll;
    private ActiveGun activeGun;
    private ChracterAim chracterAim;
    private Animator animator;
    private CameraManager cameraManager;
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
        cameraManager.ActiveDeathCam();

    }
    protected override void OnDamage(Vector3 direction) {
        animator.SetBool("GetDamage", true);
    }
}
