using UnityEngine;

public class AiHealth : MonoBehaviour
{
    private AiUIHealthBar uiHealthBar;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    //[SerializeField] private float dieForece = 10.0f;
    //private AiRagdoll aIRagdoll;
    private AiAgent aiAgent;

    void Start()
    {
        uiHealthBar = GetComponentInChildren<AiUIHealthBar>();
        currentHealth = maxHealth;
        //aIRagdoll = GetComponent<AiRagdoll>();
        aiAgent = GetComponent<AiAgent>();
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidBodies)
        {
            HitBox hitBox =  rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.aIHealth = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction) {
        currentHealth -= amount;
        uiHealthBar.SetHealthBarEnemyPercent((float)currentHealth / maxHealth);
        if (currentHealth <= 0) {
            Die(direction);
        }
    }

    private void Die(Vector3 direction) {
        AiDeathState deathState = aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState;
        deathState.direction = direction;
        aiAgent.stateMachine.ChangeState(AiStateID.Death);
        // aIRagdoll.ActiveRag();
        // direction.y = 1f;
        // aIRagdoll.ApplyForceLying(direction * dieForece);
        // uiHealthBar.gameObject.SetActive(false);
    }
}
