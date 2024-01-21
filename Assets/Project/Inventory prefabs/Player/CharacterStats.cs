using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat armor;

    public int maxHealth = 100;
    public int currentHealth {get; private set;}

    private void Awake() {
        currentHealth = maxHealth;
    }
    protected void Update() {
        if(Input.GetKeyDown(KeyCode.T)) TakeDamage(20);
    }

    public void TakeDamage(int damage) {

        damage -= armor.GetValue(); // damage bi giam nho vao armor
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log("PlayerHealth - " + damage);
        if(currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        Debug.Log("Player died");
    }
}
