using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateID intialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;

    public AiRagdoll ragdoll;
    public AiUIHealthBar aiUIHealthBar;
    public Transform playerTransform;
    
    void Start()
    {
        ragdoll = GetComponent<AiRagdoll>();
        aiUIHealthBar = GetComponentInChildren<AiUIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine = new AiStateMachine(this);

        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());

        stateMachine.ChangeState(intialState);
        Debug.Log("so states trong AiStateID enum = " 
                    + stateMachine.numStates_AiStateID);
        
        
    }

    void Update()
    {
            // Debug.Log(aiStateMachine.aiAgent.name);
            // Debug.Log(aiStateMachine.states.Length);
            // Debug.Log(aiStateMachine.states);
            
            // Debug.Log(aiStateMachine.states[0]);
            // Debug.Log(System.Enum.GetValues(typeof(AiStateID)).Length);

            stateMachine.Update();
            
            
    }
}