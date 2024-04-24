using UnityEngine;
using UnityEngine.AI;
public class AiLocalmotion : MonoBehaviour
{
    /* [SerializeField] private float maxTime = 1.0f;
    [SerializeField] private float maxDistance = 1.0f;
    private float timer = 0.0f;
    private Transform playerTransform; */

    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private Transform playerTransfom;
    [SerializeField] private Transform headAimPlayer_SourceObjects;

    private void Awake() {
        playerTransfom = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
    private void Start() {
        
        // if(playerTransform == null) {
        //     playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // }

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update() {
        /* if(!agent.enabled) return;

        timer -= Time.deltaTime;
        if(!agent.hasPath) {
            agent.destination = playerTransform.position;
        }

        if(timer < 0.0f) {
            Vector3 direction = (playerTransform.position - agent.destination);
            direction.y = 0;
            if(direction.sqrMagnitude > maxDistance * maxDistance) {
                if(agent.pathStatus != NavMeshPathStatus.PathPartial) {
                    agent.destination = playerTransform.position;
                }
            }
            timer = maxTime;
        } */
        
        headAimPlayer_SourceObjects.position = playerTransfom.position;
        headAimPlayer_SourceObjects.transform.rotation = playerTransfom.rotation;
        
        if(agent.hasPath) {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        } else {
            animator.SetFloat("Speed", 0);
        }
    }

}
