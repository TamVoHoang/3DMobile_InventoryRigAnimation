using UnityEngine;

public class AiHealth_zom : Health
{
    AiAgent_zom aiAgent_zom;
    AiUIHealthBar uiHealthBar; // thanh mau cua Ai agen dat tai day
    //private bool isGetKilledPoint; // lay diem len bang
    protected override void OnStart() {
        Debug.Log("OnStart() AiHealth.cs run");
        //isGetKilledPoint = false;
        aiAgent_zom = GetComponent<AiAgent_zom>();
        
        SetCurrentHealth = MaxHealth; // gan currentHealth
        lowHealthLimit = 100f;
        isReadyToTakeDamage = true; // xet true de san sang bi take damage
        uiHealthBar = GetComponentInChildren<AiUIHealthBar>();

    }
    protected override void OnDeath(Vector3 direction) {
        AiDeathState_zom deathState = aiAgent_zom.stateMachine_zom.GetState(AiStateID_Zom.Death) as AiDeathState_zom; //cha as con
        deathState.direction = direction;
        aiAgent_zom.stateMachine_zom.ChangeState(AiStateID_Zom.Death);
        //Destroy(this.gameObject, delayTimeToDestroy);

        if(IsDead && isReadyToTakeDamage) {
            isReadyToTakeDamage = !isReadyToTakeDamage;

            GameManger.Instance.SetKilledCountCurrInGame(1);
            PlayerDataJson.Instance.PlayerJson.killed += 1;
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
