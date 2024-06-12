using UnityEngine;
using UnityEngine.AI;

public class AiAgent_zom : MonoBehaviour
{
    public AiStateMachine_zom stateMachine_zom;
    public AiStateID_Zom intialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig configZombie;

    public AiRagdoll ragdoll;
    public AiUIHealthBar aiUIHealthBar;
    public Transform playerTransform;

    public AiWeapons_zom aiWeapons_zom;
    
    public AiSensor aiSensor;
    public AiTargetingSystem aiTargetingSystem;
    public AiHealth_zom health;
    private void Start() {
        ragdoll = GetComponent<AiRagdoll>();
        aiUIHealthBar = GetComponentInChildren<AiUIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        aiWeapons_zom = GetComponent<AiWeapons_zom>();
        aiSensor = GetComponent<AiSensor>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        aiTargetingSystem = GetComponent<AiTargetingSystem>();
        health = GetComponent<AiHealth_zom>();

        stateMachine_zom = new AiStateMachine_zom(this);

        stateMachine_zom.RegisterState(new AiChasePlayerState_zom());
        stateMachine_zom.RegisterState(new AiDeathState_zom());
        stateMachine_zom.RegisterState(new AiIdleState_zom());
        stateMachine_zom.RegisterState(new AiAttackTargetState_zom());
        stateMachine_zom.RegisterState(new AiFindTargetState_zom());

        stateMachine_zom.ChangeState(intialState); // AiStateID.intialState
        Debug.Log("so states trong AiStateID enum = " 
            + stateMachine_zom.numStates_AiStateID_zom);
    }

    private void Update() {

        if (!CheckSpawnerScene.CheckScene(CheckSpawnerScene.MainMenuScene) && 
            !CheckSpawnerScene.CheckScene(CheckSpawnerScene.DataOverviewScene) && 
            !CheckSpawnerScene.CheckScene(CheckSpawnerScene.SpawnerScene)) 
        {
            stateMachine_zom.Update();        
        }
        
    }
}
