using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    //gameobject = vat pham healthPickup bom mau cho ai

    public float amount = 50f;
    private void OnTriggerEnter(Collider other) {
        Health health = other.GetComponent<Health>();                   //co helath.cs
        AiHealth aiHealth = other.GetComponent<AiHealth>();             //co aiHealth.cs Enemy cham vao
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); // player cham vao

        if((health && aiHealth.IsLowHealth()) || playerHealth) {
            health.Heal(amount);
            Destroy(this.gameObject, 0.2f);
        }
        
    }

}
