using UnityEngine;

public class PlayerHealth : Health
{
    private AiRagdoll aiRagdoll;
    private ActiveGun activeGun;
    private ChracterAim chracterAim;
    protected override void OnStart() {
        aiRagdoll = GetComponent<AiRagdoll>();
        activeGun = GetComponent<ActiveGun>();
        chracterAim = GetComponent<ChracterAim>();
    }
    protected override void OnDeath(Vector3 direction) {
        aiRagdoll.ActiveRag();
        activeGun.DropWeapon();
        chracterAim.enabled = false; // tat chatacterAim.cs

    }
    protected override void OnDamage(Vector3 direction) {

    }
}
