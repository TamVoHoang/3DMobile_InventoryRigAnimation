using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private bool isDead = false;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    private AiUIHealthBar uiHealthBar;
    public bool IsDead {get => isDead;}
    private float lowHealth = 100f;
    public float MaxHealth{get => maxHealth;}
    public float CurrentHealth{get => currentHealth;}
    public float LowHealth{get => lowHealth;}
    //private AiAgent aiAgent;//? ben ke thua se tu tao ra
    
/*     //testing Layer at animator override
    [SerializeField] private AvatarMask baseMask;
    [SerializeField] private AvatarMask weaponMask;
    private AnimatorController animatorController;
    private int baseLayerIndex;
    private int weaponLayerIndex;
 */    
    private void Awake() {
/*      animatorController = GetComponent<Animator>().runtimeAnimatorController as AnimatorController;
        weaponLayerIndex = GetComponent<Animator>().GetLayerIndex("Weapon Layer");
        baseLayerIndex = GetComponent<Animator>().GetLayerIndex("Base Layer");
 */    }
    void Start() {
        //SetLayerBegin(); //set gia tri weaponMask(chi co phan tren cho weapon Layer[1])
        Debug.Log("Health.cs run");
        uiHealthBar = GetComponentInChildren<AiUIHealthBar>();
        currentHealth = maxHealth;
        //aiAgent = GetComponent<AiAgent>(); //? ke thua tu tao ra
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidBodies) {
            HitBox hitBox =  rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.health = this; // aIHealth = GetComponentInParent<AiHealth>();

            //gan layer hitbox vao cho tung game object da duoc add hitbox.cs
            if(hitBox.gameObject != this.gameObject) {
                hitBox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }
        OnStart();
    }

    //?tang mau
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);    // dam vao may chi nam trong khoang

        if(uiHealthBar) {
            uiHealthBar.SetHealthBarEnemyPercent((float)currentHealth / maxHealth);
        }

        OnHeal(amount);

    }

    //? hitbox.cs call TakeDamage()
    public void TakeDamage(float amount, Vector3 direction) {
        currentHealth -= amount;
        if(uiHealthBar) {
            uiHealthBar.SetHealthBarEnemyPercent((float)currentHealth / maxHealth);
        }

        OnDamage(direction); // tai sao dat o day

        if (currentHealth <= 0) {
            isDead = true;
            Die(direction);
        }
    }
    public bool IsLowHealth() => currentHealth < lowHealth;
    private void Die(Vector3 direction) {
        /* AiState aiState = aiAgent.stateMachine.GetState(AiStateID.Death);
        AiDeathState aiDeathState = aiState as AiDeathState;
        (aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState).direction = direction; */
        
        OnDeath(direction);
    }

    protected virtual void OnStart() {

    }
    protected virtual void OnDeath(Vector3 direction) {

    }
    protected virtual void OnDamage(Vector3 direction) {

    }
    protected virtual void OnHeal(float amount) {

    }

    

    /*     private void SetLayerBegin() {
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
     */
}
