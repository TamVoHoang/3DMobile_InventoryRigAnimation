using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

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

    //vector min max -> enemy se spawn random tren 2 diem nay 
    [SerializeField] Vector3 min, max;
    public Vector3 Min{get{return min;}}
    public Vector3 Max{get{return max;}}
    public float timer;

    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    private void Awake() {
        min = this.transform.position + new Vector3(-15f, 0, -15);
        max = this.transform.position + new Vector3(15f, 0, 15);
        timer = 0f;
    }

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

        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {

        /* if (!CheckSpawnerScene.CheckScene(CheckSpawnerScene.MainMenuScene) && 
            !CheckSpawnerScene.CheckScene(CheckSpawnerScene.DataOverviewScene) && 
            !CheckSpawnerScene.CheckScene(CheckSpawnerScene.SpawnerScene)) 
        {
            stateMachine_zom.Update();        
        } */

        if(CheckSpawnerScene.IsInMenuScene()) return;
        stateMachine_zom.Update();
    }
    
    public void DesTroyOnSelfe() => Destroy(this.gameObject);
    
    // sound
    public void PlaySound() {
        audioSource.PlayOneShot(audioClip, 1);
    }
}
