using UnityEngine;

public class AiIdleState_zom : AiState_Zom
{
    public AiStateID_Zom GetId() {
        return AiStateID_Zom.Idle;
    }
    public void Enter(AiAgent_zom agent) {
        
    }

    public void Update(AiAgent_zom agent) {
        Debug.Log("zombie Idle");
        if(agent.playerTransform.GetComponent<PlayerHealth>().IsDead) {
            return;
        }

        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if(playerDirection.magnitude > agent.config.maxSightDistance) {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize(); //? vector do dai
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);

        if(dotProduct > 0.0f) {
            Debug.Log("chase player");
            agent.stateMachine_zom.ChangeState(AiStateID_Zom.ChasePlayer);
        }
    }
    public void Exit(AiAgent_zom agent) {
        Debug.Log("Exit() IdleState");
        agent.navMeshAgent.speed = 0f;
        agent.navMeshAgent.stoppingDistance = 0f;
    }
    
}
