using UnityEngine.AI;
using UnityEngine;

public class AiChasePlayerState_zom : AiState_Zom
{
    private float timer = 0.0f;
    public AiStateID_Zom GetId() {
        return AiStateID_Zom.ChasePlayer;
    }

    public void Enter(AiAgent_zom agent_Zom) {
        agent_Zom.navMeshAgent.speed = agent_Zom.configZombie.speed_Chase;  //4
        agent_Zom.navMeshAgent.stoppingDistance = agent_Zom.configZombie.stoppingDis_Chase; //2
    }

    public void Update(AiAgent_zom agent) {
        Debug.Log("zombie dang vao chase");
        if(!agent.enabled) return;
        
        timer -= Time.deltaTime;
        if(!agent.navMeshAgent.hasPath) {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }
        if(timer < 0.0f) {
            Vector3 direction = agent.playerTransform.position - agent.navMeshAgent.destination;
            direction.y = 0;
            if(direction.sqrMagnitude > agent.configZombie.maxDistance * agent.configZombie.maxDistance) {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial) {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.configZombie.maxTime;

            //? dang chase neu khoang cach bo xa qu lon thi agen chuyen qua idle or Attack
            var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
            //if(distance < 6) agent.stateMachine_zom.ChangeState(AiStateID_Zom.FindTarget); //? OK
            if(distance < 4) agent.stateMachine_zom.ChangeState(AiStateID_Zom.AttackTarget); // chase -> attack
            else if(distance > 9) agent.stateMachine_zom.ChangeState(AiStateID_Zom.FindTarget); //! chase -> random find
        }

    }
    public void Exit(AiAgent_zom agent) {
        Debug.Log("Exit() ChaseState");
        agent.navMeshAgent.speed = 0f;
        agent.navMeshAgent.stoppingDistance = 2f;
    }
    
}
