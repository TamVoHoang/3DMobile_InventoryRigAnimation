using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    // [SerializeField] private float maxTime = 1.0f;
    // [SerializeField] private float maxDistance = 1.0f;
    // private Transform playerTransform;
    private float timer = 0.0f;

    public AiStateID GetId()
    {
        return AiStateID.ChasePlayer;
    }
    public void Enter(AiAgent agent)
    {
        // if(playerTransform == null) {
        //     playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // }
    }

    public void Update(AiAgent agent)
    {
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
        }
    }
    public void Exit(AiAgent agent)
    {
    }


}
