using UnityEngine;

public class AiAttackTargetState : AiState
{
    public AiStateID GetId() {
        return AiStateID.AttackTarget;
    }

    public void Enter(AiAgent agent) {
        Debug.Log("Enter() attackState");
        agent.weapons.ActiveWeapon();                   //! khi bat dau tan cong  thi active sung

        //agent.weapons.SetTarget(agent.playerTransform); // transform player
        
        agent.navMeshAgent.stoppingDistance = agent.config.attackStopingDestination; //cach 5
        agent.navMeshAgent.speed = agent.config.attackSpeed;

    }

    public void Update(AiAgent agent) {
        Debug.Log("Attack Update()");

        //? khi player die thi chuyen qua idleState
        if(!agent.aiTargetingSystem.HasTarget)              // neu ko co target (player death)
        {
            agent.weapons.SetTarget(null);                  // set null target player
            agent.weapons.DeActiveWeapon();                 // cat sung

            agent.stateMachine.ChangeState(AiStateID.FindTarget);
            return;
        }
        
        agent.weapons.SetTarget(agent.aiTargetingSystem.Target.transform); //set target transform
        agent.navMeshAgent.destination = agent.aiTargetingSystem.TargetPosition; // chay theo target object
        
        ReloadWeapon(agent);
        SelectWeapon(agent);
        UpdateFiring(agent); // ban hay ko dua vai IsInsight() layerMask co Scan() duoc hay khong
        
    }

    public void Exit(AiAgent agent) {
        Debug.Log("Exit() AttackState");
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    //? quyet dinh ban dua theo IsInsigh() -> ai co nhin thay player ko co vat can thi ko ban
    private void UpdateFiring(AiAgent aiAgent)
    {
        if(aiAgent.aiTargetingSystem.TargetInSight) {
            Debug.Log("ai CO thay player IsInsight");
            aiAgent.weapons.SetFiring(true);
        } else {
            Debug.Log("ai KO thay player IsInsight");
            aiAgent.weapons.SetFiring(false);
        }
    }

    //? quyet dinh ban dua theo distance ai and player
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
        if(weapon && weapon.ammoCount <= 0) {
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
        if(distance > agent.config.rangeToChangeWeapon) {
            return AiWeapons.WeaponSlot.Primary; // sung dai over 7 - 20
        }
        else {
            return AiWeapons.WeaponSlot.Secondary; // sung ngan under 7
        } 
    }

}
