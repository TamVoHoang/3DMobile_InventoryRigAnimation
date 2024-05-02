using UnityEngine.AI;
using UnityEngine;
using Unity.VisualScripting;

public class AiChasePlayerState_zom : AiState_Zom
{
    private float timer = 0.0f;
    public AiStateID_Zom GetId() {
        return AiStateID_Zom.ChasePlayer;
    }

    public void Enter(AiAgent_zom agent) {
        agent.navMeshAgent.speed = 5;
        agent.navMeshAgent.stoppingDistance = 0f;
    }

    public void Update(AiAgent_zom agent) {
        Debug.Log("co vao chase");
        if(!agent.enabled) return;
        
        timer -= Time.deltaTime;
        if(!agent.navMeshAgent.hasPath) {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }
        if(timer < 0.0f) {
            Vector3 direction = agent.playerTransform.position - agent.navMeshAgent.destination;
            direction.y = 0;
            if(direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance) {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial) {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;

            //? dang chase neu khoang cach bo xa qu lon thi agen chuyen qua idle or Attack
            var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
            if(distance < 3) agent.stateMachine_zom.ChangeState(AiStateID_Zom.AttackTarget);        //todo ATTACK
            if(distance > 7) agent.stateMachine_zom.ChangeState(AiStateID_Zom.FindTarget); 

            
        }

    }
    public void Exit(AiAgent_zom agent) {
        Debug.Log("Exit() ChaseState");
        agent.navMeshAgent.speed = 0f;
        agent.navMeshAgent.stoppingDistance = 0f;
    }
    
}
