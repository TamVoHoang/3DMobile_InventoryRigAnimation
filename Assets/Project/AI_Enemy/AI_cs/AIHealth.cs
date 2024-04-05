using UnityEngine;

public class AiHealth : Health
{
    private AiAgent aiAgent;
    protected override void OnStart() {
        Debug.Log("con chay");
        aiAgent = GetComponent<AiAgent>();
    }
    protected override void OnDeath(Vector3 direction) {
        AiDeathState deathState = aiAgent.stateMachine.GetState(AiStateID.Death) as AiDeathState; //cha as con
        deathState.direction = direction;
        aiAgent.stateMachine.ChangeState(AiStateID.Death);
    }
    protected override void OnDamage(Vector3 direction) {
        
    }
}
