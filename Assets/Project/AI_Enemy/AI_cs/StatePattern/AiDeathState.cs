using System.ComponentModel;
using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;

    public AiStateID GetId() {
        return AiStateID.Death; //? no se tra ve kieu ten nam trong enum
    }
    
    public void Enter(AiAgent agent) {
        agent.ragdoll.ActiveRag();
        direction.y = 1f;
        agent.ragdoll.ApplyForceLying(direction * agent.config.dieForece);
        agent.aiUIHealthBar.gameObject.SetActive(false);
        agent.weapons.DropWeapon(); // chet vang sung ra
    }
    public void Update(AiAgent agent) {
        
    }

    public void Exit(AiAgent agent) {
        
    }


    
}
