using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;

    public AiStateID GetId() {
        return AiStateID.Death; //? no se tra ve kieu ten nam trong enum
    }
    
    public void Enter(AiAgent agent) {
        Debug.Log("chay Enter() aiDeathState");
        agent.ragdoll.ActiveRag();
        direction.y = 1f;
        agent.ragdoll.ApplyForceLying(direction * agent.config.dieForece);
        agent.aiUIHealthBar.gameObject.SetActive(false);

        agent.weapons.DropWeapon(); // chet vang sung ra
    }
    public void Update(AiAgent agent) {
        Debug.Log("chay Update() aiDeathState");
        // chet la het ko co update gi tiep theo
    }

    public void Exit(AiAgent agent) {
        Debug.Log("chay Exit() aiDeathState");
        agent.weapons.SetTarget(null); // khi nhan vat chet thi ko cho nham vao player nua
    }
}
