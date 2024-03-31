using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;
    public AiStateID GetId()
    {
        return AiStateID.Death;
    }
    public void Enter(AiAgent agent)
    {
        agent.ragdoll.ActiveRag();
        direction.y = 1f;
        agent.ragdoll.ApplyForceLying(direction * agent.config.dieForece);
        agent.aiUIHealthBar.gameObject.SetActive(false);
    }
    public void Update(AiAgent agent)
    {

    }

    public void Exit(AiAgent agent)
    {
        
    }


    
}
