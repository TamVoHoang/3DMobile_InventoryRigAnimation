using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{
    public AiStateID GetId()
    {
        return AiStateID.AttackPlayer;
    }

    public void Enter(AiAgent agent)
    {
        agent.weapons.ActiveWeapon();// khi bat dau tan cong  thi active sung
        agent.weapons.SetTarget(agent.playerTransform); // transform player
        Debug.Log("ai enter attack");
    }

    public void Update(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {

    }
}
