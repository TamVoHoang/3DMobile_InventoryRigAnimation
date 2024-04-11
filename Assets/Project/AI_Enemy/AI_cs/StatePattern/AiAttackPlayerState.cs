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
        agent.navMeshAgent.stoppingDistance = agent.config.attackStopingDestination; //cach 5
        agent.navMeshAgent.speed = agent.config.attackSpeed;

    }

    public void Update(AiAgent agent) {
        agent.navMeshAgent.destination = agent.playerTransform.position; // chay thao player
        ReloadWeapon(agent);
        SelectWeapon(agent);
        UpdateFiring(agent); // ban hay ko dua vai IsInsight() layerMask co Scan() duoc hay khong

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

    //todo quyet dinh ban dua theo IsInsigh() -> ai co nhin thay player ko co vat can thi ko ban
    private void UpdateFiring(AiAgent aiAgent)
    {
        if(aiAgent.aiSensor.IsInSight(aiAgent.playerTransform.gameObject)) {
            aiAgent.weapons.SetFiring(true);
            Debug.Log("ai CO thay player IsInsight");
        } else {
            aiAgent.weapons.SetFiring(false);
            Debug.Log("ai KO thay player IsInsight");
        }
    }

    //todo quyet dinh ban dua theo distance ai and player
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
        CanAttackPlayer(agent, agent.config.canAttackDistance); // chon best gun -> tan cong theo vector3.Distance

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
