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
        agent.navMeshAgent.destination = agent.playerTransform.position;
    }

    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}
