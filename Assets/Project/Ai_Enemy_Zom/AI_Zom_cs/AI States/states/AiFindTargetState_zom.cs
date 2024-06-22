using UnityEngine;

public class AiFindTargetState_zom : AiState_Zom
{
    public AiStateID_Zom GetId()  {
        return AiStateID_Zom.FindTarget;
    }

    public void Enter(AiAgent_zom agent) {
        agent.navMeshAgent.speed = agent.configZombie.speed_Target; // 3.5f
        agent.navMeshAgent.stoppingDistance = 0f; //0

        // dat gia tri timer = 0 khi bat dau vao findtarget satage
        agent.timer = 0f;
    }

    public void Update(AiAgent_zom agent) {
        Debug.Log("zombie dang vao Find");

        // neu tim ma ko thay target (ko thay player) -> huy doi tuong sau timer
        agent.timer += Time.deltaTime;
        if(agent.timer >= 20f) {
            agent.DesTroyOnSelfe();
            return;
        }
        
        // neu ko co path (ai ko biet tiep theo di dau sau khi di den destination)
        if(!agent.navMeshAgent.hasPath) {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();

            // random vi tri theo worldbound _ old version
            /* agent.navMeshAgent.destination = worldBounds.RandomPosition(); */ 

            // random vi tri theo 2 toa do co san tren ai _ new version
            agent.navMeshAgent.destination = worldBounds.RandomPosition_AroundAi(agent.Min, agent.Max);
        }

        var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        if(agent.aiTargetingSystem.HasTarget) {
            /* if(distance < 3f) agent.stateMachine_zom.ChangeState(AiStateID_Zom.AttackTarget); */ //? khi sensor detect thay hasTarget | thay cham vang
            agent.stateMachine_zom.ChangeState(AiStateID_Zom.ChasePlayer);
        }
    }

    public void Exit(AiAgent_zom agent) {
        agent.timer = 0f;
    }
    
}
