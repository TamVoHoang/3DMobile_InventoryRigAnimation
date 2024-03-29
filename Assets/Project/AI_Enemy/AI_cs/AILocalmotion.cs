using UnityEngine;
using UnityEngine.AI;

public class AILocalmotion : MonoBehaviour
{
    [SerializeField] private float maxTime = 1.0f;
    [SerializeField] private float maxDistance = 1.0f;
    private float timer = 0.0f;


    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update() {
        //agent.destination = playerTransform.position; // move lien tuc den player.transform.position
        AiMoveToPlayerStack();
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    private void AiMoveToPlayerStack() {
        timer -= Time.deltaTime;
        if(timer < 0.0f) {
            float spDistance = (playerTransform.position - agent.destination).sqrMagnitude;
            if(spDistance > maxDistance * maxDistance) {
                agent.destination = playerTransform.position;
            }
            timer = maxTime;
        }
    }
}
