using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    private AIRagdoll aIRagdoll;
    void Start()
    {
        currentHealth = maxHealth;
        aIRagdoll = GetComponent<AIRagdoll>();

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidBodies)
        {
            HitBox hitBox =  rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.aIHealth = this;
        }
    }

    public void TakeDamage(float amount, Vector3 direction) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die()
    {
        aIRagdoll.ActiveRag();
    }
}
