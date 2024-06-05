using UnityEngine;

public class AiFindTargetState_zom : AiState_Zom
{
    public AiStateID_Zom GetId()  {
        return AiStateID_Zom.FindTarget;
    }

    public void Enter(AiAgent_zom agent) {
        agent.navMeshAgent.speed = agent.configZombie.speed_Target; // 3.5f
        agent.navMeshAgent.stoppingDistance = 0f; //0
    }

    public void Update(AiAgent_zom agent) {
        Debug.Log("zombie dang vao Find");
        if(!agent.navMeshAgent.hasPath) {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }

        var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        if(agent.aiTargetingSystem.HasTarget) {
            //if(distance < 3f) agent.stateMachine_zom.ChangeState(AiStateID_Zom.AttackTarget); //? khi sensor detect thay hasTarget | thay cham vang
            agent.stateMachine_zom.ChangeState(AiStateID_Zom.ChasePlayer);
        }
    }

    public void Exit(AiAgent_zom agent) {
        
    }
    
}
