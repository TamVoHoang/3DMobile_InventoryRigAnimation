
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Transform targetTrans;
    private NavMeshAgent agent;
    [SerializeField] private float followTargetDis = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(targetTrans != null) {
            agent.SetDestination(targetTrans.position);
            FaceTaget();

        }
    }

    //todo di chuyen den vi tri vector3. PlayerController call when mouse(0) button
    public void MoveToPoint(Vector3 point) {
        agent.SetDestination(point);
    }

    //todo
    public void FollowTarget(Interactable newTarget) {

        agent.stoppingDistance = newTarget.Radius * followTargetDis;
        agent.updateRotation = false;

        targetTrans = newTarget.InteractionTransform; //todo iTem.position
    }

    //todo PlayerController call when RemoveFocus()
    public void StopFollowingTarget() {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        targetTrans = null;
    }

    //todo xoay mat ve huong target item => xoay doi tuong
    private void FaceTaget() {
        Vector3 dir = (targetTrans.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * 5f);
    }
}