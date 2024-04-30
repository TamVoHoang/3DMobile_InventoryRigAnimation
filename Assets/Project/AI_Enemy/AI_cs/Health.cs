using UnityEngine;

public class Health : MonoBehaviour
{
    
    [SerializeField] private bool isDead = false;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    //private AiUIHealthBar uiHealthBar; // thanh mau cua Ai agen dat tai day

    protected bool isReadyToTakeDamage = false; //todo neu true - se bi tru mau, neu false - ko bi tru mau| player khi respawn se co 5s de xet TRUE
    protected float lowHealthLimit;   // player.obj override loewHealth coll 19 PlayerHealth.cs

    public bool IsDead {get => isDead;}
    public float MaxHealth{get => maxHealth;}
    public float SetCurrentHealth {set => currentHealth = value;}
    public float CurrentHealth{get => currentHealth;}
    public bool IsReadyToTakeDamage{get => isReadyToTakeDamage;}
    
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
        //currentHealth = maxHealth; //! ok van co the dung xet tai day
        //uiHealthBar = GetComponentInChildren<AiUIHealthBar>();
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
    public void Heal(float amount) {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);    // dam vao may chi nam trong khoang

        //OK co the dung cho ai tai day
        /* if(uiHealthBar) {
            uiHealthBar.SetHealthBarEnemyPercent((float)currentHealth / maxHealth);
        } */

        OnHeal(amount);
    }

    //? hitbox.cs call TakeDamage()
    public void TakeDamage(float amount, Vector3 direction) {
        if(currentHealth == 0) return;
        if(currentHealth > 0 && isReadyToTakeDamage) {
            currentHealth -= amount;
        }

        //OK co the dung cho ai tai day
        /* if(uiHealthBar) {
            uiHealthBar.SetHealthBarEnemyPercent((float)currentHealth / maxHealth);
        } */
        Debug.Log("direction = " + direction);
        OnDamage(direction); // tai sao dat o day => tao animation + hieu ung man hinh khi trung dan

        if (currentHealth <= 0) {
            isDead = true;
            Die(direction);
        }
    }
    public bool IsLowHealth() => currentHealth < lowHealthLimit;
    private void Die(Vector3 direction) {
        //? giai thich ve cach gan c=gia tri cho doi duong ke thua interface
        /* AiState aiState = aiAgent.stateMachine.GetState(AiStateID.Death);
        AiDeathState aiDeathState = aiState as AiDeathState;
        (aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState).direction = direction; */
        Debug.Log("direction = " + direction);
        OnDeath(direction);
    }

    public void ResetCurrentHealth() {
        this.currentHealth = maxHealth;
        this.isDead = false;
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
