
public enum AiStateID
{
    ChasePlayer,
    Death,
    Idle
}

public interface AiState
{
    AiStateID GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
