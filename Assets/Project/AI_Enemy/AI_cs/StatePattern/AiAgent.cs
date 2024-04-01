using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    //! gameobject = AI enemy
    public AiStateMachine stateMachine;
    public AiStateID intialState; // co the chon duoc ben trong unity
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config; //? sctiptable object chua dieu kien de chay update() tung state

    public AiRagdoll ragdoll;
    public AiUIHealthBar aiUIHealthBar;
    public Transform playerTransform;
    
    void Start()
    {
        ragdoll = GetComponent<AiRagdoll>();
        aiUIHealthBar = GetComponentInChildren<AiUIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        stateMachine = new AiStateMachine(this);                                    //todo 0

        stateMachine.RegisterState(new AiChasePlayerState()); // add vao array states
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());

        stateMachine.ChangeState(intialState); // AiStateID.intialState
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