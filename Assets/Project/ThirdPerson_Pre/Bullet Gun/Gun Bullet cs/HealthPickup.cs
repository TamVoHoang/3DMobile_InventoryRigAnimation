using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    //gameobject = vat pham healthPickup bom mau cho ai

    [SerializeField] private float amount = 100f;
    private void OnTriggerEnter(Collider other) {
        Health health = other.GetComponent<Health>();                   //co helath.cs
        AiHealth aiHealth = other.GetComponent<AiHealth>();             //co aiHealth.cs Enemy cham vao
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); // player cham vao

        /* if(health && aiHealth.IsLowHealth()) {
            health.Heal(amount);
            Destroy(this.gameObject, 0.2f);
        } */

        if(aiHealth != null) {
            if(aiHealth.IsLowHealth()) {
                health.Heal(amount);
                Destroy(this.gameObject, 0.2f);
            }
        }

        if(playerHealth!= null) {
            if(playerHealth.IsLowHealth()) {
                health.Heal(amount);
                Destroy(this.gameObject, 0.2f);
            }
        }
        
    }

}
