using UnityEngine;

public class AiAttackPlayerState : AiState
{
    public AiStateID GetId() {
        return AiStateID.AttackPlayer;
    }

    public void Enter(AiAgent agent) {
        Debug.Log("Enter() attackState");
        agent.weapons.ActiveWeapon();                   // khi bat dau tan cong  thi active sung

        agent.weapons.SetTarget(agent.playerTransform); // transform player
        agent.navMeshAgent.stoppingDistance = agent.config.attackStopingDestination;     // khi tan cong se dung cach 5
        agent.navMeshAgent.speed = agent.config.attackSpeed;

        //agent.weapons.SetFiring(true);                  // bat dau ban khi da trang bi xong
    }

    public void Update(AiAgent agent) {
        agent.navMeshAgent.destination = agent.playerTransform.position; // chay thao player
        ReloadWeapon(agent);
        SelectWeapon(agent);

        //? khi player die thi chuyen qua idleState
        if(agent.playerTransform.GetComponent<PlayerHealth>().IsDead) {
            agent.weapons.SetTarget(null);                  // set null target player
            agent.weapons.DeActiveWeapon();                 // cat sung
            agent.stateMachine.ChangeState(AiStateID.Idle);
        }

        Debug.Log("Attack Update()");
    }

    public void Exit(AiAgent agent) {
        Debug.Log("Exit() AttackState");
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    private void CanAttackPlayer(AiAgent agent, float attackedDistance) {
        var distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);
        if(distance <= attackedDistance) {
            agent.weapons.SetFiring(true);
        } else {
            agent.weapons.SetFiring(false);
        }
    }

    private void ReloadWeapon(AiAgent agent) {
        
        var weapon = agent.weapons.currentWeapon;
        if(weapon && weapon.ammoCount <= 0) {
            agent.weapons.ReloadWeapon();
        }
    }

    private void SelectWeapon(AiAgent agent) {
        var bestWeapon = ChooseWeapon(agent);
        Debug.Log("best weapon = "+bestWeapon);
        if(bestWeapon != agent.weapons.currentWeaponSlot) {
            agent.weapons.SwitchWeapon(bestWeapon);
            Debug.Log("Switch to best weapon = " + bestWeapon);
        }
        //agent.weapons.SetFiring(true); //! OK
        CanAttackPlayer(agent, agent.config.canAttackDistance); //20

    }
    private AiWeapons.WeaponSlot ChooseWeapon(AiAgent agent) {
        float distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);
        if(distance > agent.config.rangeToChangeWeapon) {
            return AiWeapons.WeaponSlot.Primary; // sung dai over 7 - 20
        }
        else {
            return AiWeapons.WeaponSlot.Secondary; // sung ngan under 7
        } 
    }

}
