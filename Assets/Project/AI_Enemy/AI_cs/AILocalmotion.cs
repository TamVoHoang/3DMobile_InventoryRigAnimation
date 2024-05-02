using UnityEngine;
using UnityEngine.AI;
public class AiLocalmotion : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private Transform playerTransfom;
    [SerializeField] private Transform headAimPlayer_SourceObjects;

    private void Awake() {
        playerTransfom = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update() {
        
        headAimPlayer_SourceObjects.position = playerTransfom.position;
        headAimPlayer_SourceObjects.transform.rotation = playerTransfom.rotation;
        
        if(agent.hasPath) {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        } else {
            animator.SetFloat("Speed", 0);
        }
    }

    //todo
}
