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
        // Vector3 agentDirection1 = agent.transform.right;
        // Vector3 agentDirection2 = - agent.transform.right;

        playerDirection.Normalize(); //? vector do dai

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        // float dotProduct1 = Vector3.Dot(playerDirection, agentDirection1);
        // float dotProduct2 = Vector3.Dot(playerDirection, agentDirection2);


        if(dotProduct > 0.0f ) {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent) {
        
    }

}
