using UnityEngine;

public class AiAttackPlayerState : AiState
{
    public AiStateID GetId()
    {
        return AiStateID.AttackPlayer;
    }

    public void Enter(AiAgent agent) 
    {
        Debug.Log("ai enter attack");
        agent.weapons.ActiveWeapon();// khi bat dau tan cong  thi active sung
        agent.weapons.SetTarget(agent.playerTransform); // transform player
        agent.navMeshAgent.stoppingDistance = 5.0f;

        agent.weapons.SetFiring(true); // bat dau ban khi da trang bi xong
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position; // chay thao player

        if(agent.playerTransform.GetComponent<PlayerHealth>().CurrentHealth <= 0) {
            agent.weapons.UnActiveWeapon();                 // cat sung
            agent.weapons.SetTarget(null);                  // ko xet target nua do player da chet
            agent.stateMachine.ChangeState(AiStateID.Idle); // chuyen trang thai idle
        }
        
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}
