using System;
using System.Collections;
using UnityEngine;

public class AiAttackTargetState : AiState
{
    public AiStateID GetId() {
        return AiStateID.AttackTarget;
    }

    public void Enter(AiAgent agent) {
        Debug.Log("Enter() attackState");
        agent.weapons.ActiveWeapon();                       //khi bat dau tan cong  thi active sung

        //agent.weapons.SetTarget(agent.playerTransform);   //attack ma ko can phai ai sensor detect
        
        agent.navMeshAgent.speed = agent.config.speed_Attack;
        agent.navMeshAgent.stoppingDistance = agent.config.stoppingDis_Attack;

    }

    public void Update(AiAgent agent) {
        Debug.Log("Attack Update()");

        //? khi player die thi chuyen qua idleState
        if(!agent.aiTargetingSystem.HasTarget)           // neu ko co target (player death)
        {
            agent.weapons.SetTarget(null);            //! set null target player | quan trong co trong DeActive()
            agent.weapons.DeActiveWeapon();             // cat sung
            agent.stateMachine.ChangeState(AiStateID.FindTarget);
            return;
        }
        
        agent.weapons.SetTarget(agent.aiTargetingSystem.Target.transform);          //set target transform lien tuc
        agent.navMeshAgent.destination = agent.aiTargetingSystem.TargetPosition;    //chay theo target object
        
        ReloadWeapon(agent);
        SelectWeapon(agent);
        UpdateFiring(agent); //ban hay ko dua vai IsInsight() layerMask co Scan() duoc hay khong
        UpdateLowhHealth(agent); // khi dang tan cong, neu heal < lowHealth thi di tim mau
        UpdateLowAmmo(agent);
    }


    public void Exit(AiAgent agent) {
        Debug.Log("Exit() AttackState");
        //agent.weapons.DeActiveWeapon(); //! them o day, thi khi dung qua gan, vua trang bi khong doi sung kip
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    //? quyet dinh ban dua theo IsInsigh() -> ai co nhin thay player ko co vat can thi ko ban
    private void UpdateFiring(AiAgent agent)
    {
        if(agent.aiTargetingSystem.TargetInSight) {
            Debug.Log("ai CO thay player IsInsight");
            agent.weapons.SetFiring(true);
        } else {
            Debug.Log("ai KO thay player IsInsight");
            agent.weapons.SetFiring(false);
        }
    }

    //? attack player based on ONLY distance between (player and aiagen)
    private void CanAttackPlayer_Distance(AiAgent agent, float attackedDistance) {
        var distance = Vector3.Distance(agent.aiTargetingSystem.Target.transform.position, agent.transform.position);
        if(distance <= attackedDistance) {
            agent.weapons.SetFiring(true);
        } else {
            agent.weapons.SetFiring(false);
        }
    }

    private void ReloadWeapon(AiAgent agent) {
        var weapon = agent.weapons.currentWeapon;
        if(weapon && weapon.ShouldReload()) {
            agent.weapons.ReloadWeapon();
        }
    }

    private void SelectWeapon(AiAgent agent) {
        var bestWeapon = ChooseWeapon(agent);
        Debug.Log("best weapon = "+bestWeapon);

        if(bestWeapon != agent.weapons.currentWeaponSlot) {
            Debug.Log("KHAC");
            agent.weapons.SwitchWeapon(bestWeapon);
            
            Debug.Log("Switch to best weapon = " + bestWeapon);
        }
        //CanAttackPlayer_Distance(agent, agent.config.canAttackDistance); //? chon best gun -> tan cong theo vector3.Distance

    }
    private AiWeapons.WeaponSlot ChooseWeapon(AiAgent agent) {
        float distance = agent.aiTargetingSystem.TargetDistance;
        if(distance > agent.config.changeBestWeapon_Range) {
            return AiWeapons.WeaponSlot.Primary; // sung dai over 7 - 20
        }
        else {
            return AiWeapons.WeaponSlot.Secondary; // sung ngan under 7
        }
    }

    private void UpdateLowhHealth(AiAgent agent) {
        if(agent.health.IsLowHealth()) {
            agent.stateMachine.ChangeState(AiStateID.FindHealth);
        }
    }

    private void UpdateLowAmmo(AiAgent agent)
    {
        if(agent.weapons.IsLowAmmo()) {
            agent.stateMachine.ChangeState(AiStateID.FindAmmo);
        }
    }

}
