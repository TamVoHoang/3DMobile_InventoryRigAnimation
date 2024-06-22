using UnityEngine;

public class AiFindTargetState : AiState
{
    public AiStateID GetId() {
        return AiStateID.FindTarget;
    }
    public void Enter(AiAgent agent) {
        Debug.Log("Enter() AiFindTarget State");
        agent.weapons.HolsterWeapon_FindAmmo();
        agent.navMeshAgent.speed = agent.config.speed_Target;
        agent.navMeshAgent.stoppingDistance = agent.config.stoppingDis_FindWeapon; //khi find thi stopDis = 0. dam bao trigger
    }
    public void Update(AiAgent agent) {
        Debug.Log("ai dang Find");
        //? WANDER
        if(!agent.navMeshAgent.hasPath) {
            Debug.Log("Dang generate min max AiFindWeapon State");
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();

            // random theo transform worldBound object _ Old version
            /* agent.navMeshAgent.destination = worldBounds.RandomPosition(); */

            // random theo vector min max co san tren aiagen duoc khai bao mac dinh
            agent.navMeshAgent.destination = worldBounds.RandomPosition_AroundAi(agent.Min, agent.Max);
        }
        if(agent.aiTargetingSystem.HasTarget) {
            agent.stateMachine.ChangeState(AiStateID.AttackTarget); //? khi sensor detect thay hasTarget | thay cham vang
        }
    }
    public void Exit(AiAgent agent) {
        
    }
}
