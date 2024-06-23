using UnityEngine;

public class AiAttackTargetState_zom : AiState_Zom
{
    public AiStateID_Zom GetId() {
        return AiStateID_Zom.AttackTarget;
    }

    public void Enter(AiAgent_zom agent) {
        agent.navMeshAgent.speed = agent.configZombie.speed_Attack; //3f
        agent.navMeshAgent.stoppingDistance = agent.configZombie.stoppingDis_Attack; //! phai xet bang 1
    }
    public void Update(AiAgent_zom agent) {
        Debug.Log("zombie dang vao Attack");

        agent.PlaySound();

        if(agent.health.IsDead) return;

        if(agent.playerTransform.GetComponent<PlayerHealth>().IsDead) {

            //? neu player die va ko respawn - co the dung tai day
            //agent.stateMachine.ChangeState(AiStateID.Idle);

            //? neu player die - song lai - thi dung dong nay
            agent.stateMachine_zom.ChangeState(AiStateID_Zom.FindTarget);// move den vi tri player
            return;
        }

        //var distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        if(!agent.aiTargetingSystem.HasTarget) {
            agent.stateMachine_zom.ChangeState(AiStateID_Zom.FindTarget); //!NEU CHO NHIEU AI ATTACK() THI DUNG DONG NAY => TIM AI KHAC TAN CONG
            return;
        }

        if(agent.playerTransform.GetComponent<PlayerHealth>().IsDead) return; //! THEM VAO TEST THU
        agent.navMeshAgent.destination = agent.aiTargetingSystem.TargetPosition; // khi tan cong, ai chay ve huong muc tieu
        UpdateFighting(agent);
        
    }

    

    public void Exit(AiAgent_zom agent) {
        Debug.Log("Exit() AttackState");
        agent.navMeshAgent.stoppingDistance = 1f;
    }

    private void UpdateFighting(AiAgent_zom agent) {
        if(agent.aiTargetingSystem.TargetInSight) {
            Debug.Log("ai CO thay player IsInsight");
            agent.aiWeapons_zom.StartAttacking();
        } else {
            Debug.Log("ai KO thay player IsInsight");
            agent.aiWeapons_zom.StopAttacking();
        }

    }

    
    
    //todo
}
