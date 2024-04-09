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

        agent.weapons.SetFiring(true);                // bat dau ban khi da trang bi xong
    }

    public void Update(AiAgent agent) {
        agent.navMeshAgent.destination = agent.playerTransform.position; // chay thao player
        ReloadWeapon(agent);
        //CanAttackPlayer(agent, 10);

        //? khi player die thi chuyen qua idleState
        if(agent.playerTransform.GetComponent<PlayerHealth>().IsDead) {
            agent.weapons.DeActiveWeapon();                 // cat sung
            agent.stateMachine.ChangeState(AiStateID.Idle);

        }
    }

    public void Exit(AiAgent agent) {
        Debug.Log("Exit() AttackState");
        agent.navMeshAgent.stoppingDistance = 0.0f;

    }

    private void CanAttackPlayer(AiAgent agent, float attackedDistance) {
        var distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);
        if(distance >= attackedDistance) {
            agent.weapons.SetFiring(false);
            agent.weapons.DeActiveWeapon();
        } else {
            agent.weapons.ActiveWeapon();
            agent.weapons.SetFiring(true);
        }
    }

    private void ReloadWeapon(AiAgent agent) {
        
        var weapon = agent.weapons.CurrentWeapon;
        if(weapon && weapon.ammoCount <= 0) {
            agent.weapons.ReloadWeapon();
        }
    }

}
