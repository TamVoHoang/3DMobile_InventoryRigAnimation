using UnityEngine;

public class AiFindTargetState : AiState
{
    public AiStateID GetId() {
        return AiStateID.FindTarget;
    }
    public void Enter(AiAgent agent) {
        Debug.Log("Enter() AiFindTarget State");
        agent.navMeshAgent.speed = agent.config.speed_Target;
        agent.navMeshAgent.stoppingDistance = agent.config.stoppingDis_FindWeapon; //khi find thi stopDis = 0. dam bao trigger
    }
    public void Update(AiAgent agent) {
        //? WANDER
        Debug.Log(agent.navMeshAgent.hasPath);
        if(!agent.navMeshAgent.hasPath) {
            Debug.Log("Dang generate min max AiFindWeapon State");
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }
        if(agent.aiTargetingSystem.HasTarget) {
            agent.stateMachine.ChangeState(AiStateID.AttackTarget); //? khi sensor detect thay hasTarget | thay cham vang
        }
    }
    public void Exit(AiAgent agent) {
        
    }
}
