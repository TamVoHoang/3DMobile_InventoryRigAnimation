using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateID GetId() {
        return AiStateID.Idle; //? no se tra ve kieu ten nam trong enum
    }

    public void Enter(AiAgent agent) {
        
    }

    public void Update(AiAgent agent) {
        Debug.Log("ai dang Idle");
        if(agent.playerTransform.GetComponent<PlayerHealth>().IsDead) {
            /* agent.weapons.SetTarget(null);  //! set null target player | quan trong co trong DeActive()
            agent.weapons.DeActiveWeapon(); */
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
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent) {
        Debug.Log("Exit() IdleState");
        agent.navMeshAgent.speed = 0f;
        agent.navMeshAgent.stoppingDistance = 0f;
    }

}
