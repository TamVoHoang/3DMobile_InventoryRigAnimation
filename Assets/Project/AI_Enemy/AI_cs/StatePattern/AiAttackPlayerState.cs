using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiAttackPlayerState : AiState
{
    public AiStateID GetId() {
        return AiStateID.AttackPlayer;
    }

    public void Enter(AiAgent agent) {
        Debug.Log("Enter() attackState");
        agent.weapons.ActiveWeapon();                   // khi bat dau tan cong  thi active sung
        agent.weapons.SetTarget(agent.playerTransform); // transform player
        agent.navMeshAgent.stoppingDistance = 7.0f;

        //agent.weapons.SetFiring(true);                  // bat dau ban khi da trang bi xong
    }

    public void Update(AiAgent agent) {
        
        agent.navMeshAgent.destination = agent.playerTransform.position; // chay thao player

        // dang tan cong dis > 10 stop attacking
        var distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);
        if(distance >= 10) {
            agent.weapons.SetFiring(false);
            agent.weapons.UnActiveWeapon();
        } else {
            agent.weapons.ActiveWeapon();
            agent.weapons.SetFiring(true);
        } 
        //

        //? khi player die thi chuyen qua idleState
        if(agent.playerTransform.GetComponent<PlayerHealth>().CurrentHealth <= 0) {
            agent.weapons.UnActiveWeapon();                 // cat sung
            agent.weapons.SetTarget(null);                  // ko xet target nua do player da chet
        }
    }

    public void Exit(AiAgent agent) {
        Debug.Log("Exit() AttackState");
        agent.navMeshAgent.stoppingDistance = 0.0f;

    }

}
