using UnityEngine;
using UnityEngine.AI;

public class DebugNavMesh : MonoBehaviour
{
    private NavMeshAgent aiAgent;
    [SerializeField] private bool velocity;
    [SerializeField] private bool desireVelocity;
    [SerializeField] private bool path;

    private void Start() {
        aiAgent = GetComponent<NavMeshAgent>();
    }
    private void OnDrawGizmos() {
        //if (aiAgent == null) return;
        if (velocity) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + aiAgent.velocity);
        }
        if (desireVelocity) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + aiAgent.desiredVelocity);
        }
        if(path) {
            Gizmos.color = Color.black;
            var agenPath = aiAgent.path;
            Vector3 preVCorner = transform.position;
            foreach (var corner in agenPath.corners)
            {
                Gizmos.DrawLine(preVCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                preVCorner = corner;
            }
        }
    }
}
