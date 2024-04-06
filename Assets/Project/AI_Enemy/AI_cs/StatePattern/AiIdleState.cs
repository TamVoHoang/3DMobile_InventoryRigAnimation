using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateID GetId() {
        return AiStateID.Idle; //? no se tra ve kieu ten nam trong enum
    }

    public void Enter(AiAgent agent) {
        
    }

    public void Update(AiAgent agent) {
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if(playerDirection.magnitude > agent.config.maxSightDistance) {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize(); //? vector do dai
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);

        if(dotProduct > 0.0f ) {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent) {
        
    }

}
