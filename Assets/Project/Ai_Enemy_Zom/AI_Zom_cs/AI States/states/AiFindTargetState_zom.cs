using UnityEngine;

public class AiFindTargetState_zom : AiState_Zom
{
    public AiStateID_Zom GetId()  {
        return AiStateID_Zom.FindTarget;
    }

    public void Enter(AiAgent_zom agent) {
        agent.navMeshAgent.speed = 3.5f;
        agent.navMeshAgent.stoppingDistance = 0f;
    }

    public void Update(AiAgent_zom agent) {
        if(!agent.navMeshAgent.hasPath) {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }

        //var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        if(agent.aiTargetingSystem.HasTarget) {
            agent.stateMachine_zom.ChangeState(AiStateID_Zom.AttackTarget); //? khi sensor detect thay hasTarget | thay cham vang
        }
    }

    public void Exit(AiAgent_zom agent) {
        
    }
    
}
