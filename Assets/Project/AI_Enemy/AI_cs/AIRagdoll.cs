using UnityEngine;

public class AIRagdoll : MonoBehaviour
{
    private Rigidbody[] rbs;
    private Animator animator;
    private void Start() {
        rbs = GetComponentsInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        DeactiveRag();
    }

    public void DeactiveRag() {
        foreach (var rigidBody in rbs) {
            rigidBody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActiveRag() {
        foreach (var rigidBody in rbs) {
            rigidBody.isKinematic = false;
        }
        animator.enabled = false;
    }
}
