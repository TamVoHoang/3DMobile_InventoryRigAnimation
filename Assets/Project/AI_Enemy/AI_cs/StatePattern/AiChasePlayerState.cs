using UnityEngine.AI;
using UnityEngine;

public class AiChasePlayerState : AiState
{
    private float timer = 0.0f;

    public AiStateID GetId() {
        return AiStateID.ChasePlayer; //? no se tra ve kieu ten nam trong enum
    }
    public void Enter(AiAgent agent) {
        agent.navMeshAgent.speed = agent.config.speed_Chase;
        agent.navMeshAgent.stoppingDistance = agent.config.stoppingDis_Chase;
    }

    public void Update(AiAgent agent) {
        Debug.Log("ai dang Chase");
        if(!agent.enabled) return;

        timer -= Time.deltaTime;
        if(!agent.navMeshAgent.hasPath) {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if(timer < 0.0f) {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if(direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance) {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial) {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;

            //? dang chase neu khoang cach bo xa qu lon thi agen chuyen qua idle
            var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
            if(distance > 15) agent.stateMachine.ChangeState(AiStateID.Idle);
        }

    }
    public void Exit(AiAgent agent) {
        Debug.Log("Exit() ChaseState");
        agent.navMeshAgent.speed = 0f;
        agent.navMeshAgent.stoppingDistance = 0f;
    }

}
