using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState_zom : AiState_Zom
{
    public Vector3 direction;
    public AiStateID_Zom GetId()
    {
        return AiStateID_Zom.Death;
    }
    public void Enter(AiAgent_zom agent)
    {
        Debug.Log("chay Enter() aiDeathState");
        agent.ragdoll.ActiveRag();
        direction.y = 1f;
        agent.ragdoll.ApplyForceLying(direction * agent.config.dieForece);
        agent.aiUIHealthBar.gameObject.SetActive(false);
    }
    public void Update(AiAgent_zom agent)
    {
        
    }
    public void Exit(AiAgent_zom agent)
    {
        
    }
}
