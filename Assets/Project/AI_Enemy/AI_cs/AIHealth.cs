using UnityEngine;

public class AiHealth : Health
{
    [SerializeField] private float delayTimeToDestroyAfterDeath = 5f; 
    AiAgent aiAgent;
    AiUIHealthBar uiHealthBar; // thanh mau cua Ai agen dat tai day

    protected override void OnStart() {
        Debug.Log("OnStart() AiHealth.cs run");
        //isGetKilledPoint = false;
        aiAgent = GetComponent<AiAgent>();
        
        SetCurrentHealth = MaxHealth; // gan currentHealth
        lowHealthLimit = 100f;
        isReadyToTakeDamage = true; // xet true de san sang bi take damage
        uiHealthBar = GetComponentInChildren<AiUIHealthBar>();

    }
    protected override void OnDeath(Vector3 direction) {
        AiDeathState deathState = aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState; //cha as con
        deathState.direction = direction;
        aiAgent.stateMachine.ChangeState(AiStateID.Death);
        Destroy(this.gameObject, delayTimeToDestroyAfterDeath);

        if(IsDead && isReadyToTakeDamage) {
            isReadyToTakeDamage = !isReadyToTakeDamage;

            GameManger.Instance.SetKilledCountCurrInGame(1);
            PlayerDataJson.Instance.PlayerJson.killed += 1; // tang gi tri giet duoc ai agent cho playerDataJson
        }
        
    }
    protected override void OnDamage(Vector3 direction) {
        Ai_UpdateHealthBar();
    }
    protected override void OnHeal(float amount) {
        Ai_UpdateHealthBar();
    }

    private void Ai_UpdateHealthBar() {
        if(uiHealthBar) {
            uiHealthBar.SetHealthBarEnemyPercent((float)CurrentHealth / MaxHealth);
        }
    }

}
