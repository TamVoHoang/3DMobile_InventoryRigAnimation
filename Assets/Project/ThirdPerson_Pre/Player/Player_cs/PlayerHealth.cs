using UnityEngine;

public class PlayerHealth : Health
{
    private AiRagdoll aiRagdoll;
    Animator animator;
    protected override void OnStart() {
        aiRagdoll = GetComponent<AiRagdoll>();
    }
    protected override void OnDeath(Vector3 direction) {
        aiRagdoll.ActiveRag();
    }
    protected override void OnDamage(Vector3 direction) {

    }
}
