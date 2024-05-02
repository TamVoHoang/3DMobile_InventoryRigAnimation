public enum AiStateID_Zom
{
    ChasePlayer,
    Death,
    Idle,
    AttackTarget,
    FindTarget,
}

public interface AiState_Zom
{
    AiStateID_Zom GetId();
    void Enter(AiAgent_zom agent);
    void Update(AiAgent_zom agent);
    void Exit(AiAgent_zom agent);
}
