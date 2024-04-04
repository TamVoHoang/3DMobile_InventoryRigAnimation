using UnityEditor.Animations;
using UnityEngine;

public class AiHealth : MonoBehaviour
{
    private AiUIHealthBar uiHealthBar;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    //[SerializeField] private float dieForece = 10.0f;
    //private AiRagdoll aIRagdoll;
    private AiAgent aiAgent;

    //!testing
    [SerializeField] private AvatarMask baseMask;
    [SerializeField] private AvatarMask weaponMask;
    private AnimatorController animatorController;
    private int baseLayerIndex;
    private int weaponLayerIndex;
    //! testing
    
    private void Awake() {
        animatorController = GetComponent<Animator>().runtimeAnimatorController as AnimatorController;
        weaponLayerIndex = GetComponent<Animator>().GetLayerIndex("Weapon Layer");
        baseLayerIndex = GetComponent<Animator>().GetLayerIndex("Base Layer");
    }
    void Start() {
        //SetLayerBegin(); //todo set gia tri weaponMask(chi co phan tren cho weapon Layer[1])

        uiHealthBar = GetComponentInChildren<AiUIHealthBar>();
        currentHealth = maxHealth;
        //aIRagdoll = GetComponent<AiRagdoll>(); //bo ko lay
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
        /* AiState aiState = aiAgent.stateMachine.GetState(AiStateID.Death);
        AiDeathState aiDeathState = aiState as AiDeathState;
        (aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState).direction = direction; */

        AiDeathState deathState = aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState; //cha as con
        deathState.direction = direction;

        //!testing
        //SetLayerDeath();
        aiAgent.stateMachine.ChangeState(AiStateID.Death);

        /* aIRagdoll.ActiveRag();
        direction.y = 1f;
        aIRagdoll.ApplyForceLying(direction * dieForece);
        uiHealthBar.gameObject.SetActive(false); */
    }

    private void SetLayerBegin() {
        var layers = animatorController.layers;
        layers[weaponLayerIndex].avatarMask = weaponMask;
        layers[baseLayerIndex].avatarMask = baseMask;
        animatorController.layers = layers;
        Debug.Log("set weapon layer mask for weapon Layer");
    }
    
    public void SetLayerDeath() {
        var layers = animatorController.layers;
        //layers[weaponLayerIndex].avatarMask = null;
        layers[baseLayerIndex].avatarMask = baseMask;

        animatorController.layers = layers;
        Debug.Log("set null avatar mask for weapon Layer");
    }
}
