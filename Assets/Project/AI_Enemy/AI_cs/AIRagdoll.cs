using System.Collections;
using Mono.Cecil;
using TreeEditor;
using UnityEditor.Sprites;
using UnityEngine;

public class AiRagdoll : MonoBehaviour
{
    private Rigidbody[] rbs;
    private Rigidbody rigidbody;
    private Animator animator;
    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rbs = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactiveRag();
    }

    public void DeactiveRag() {
        foreach (var rigidBody in rbs) {
            rigidBody.isKinematic = true;
        }
        animator.enabled = true;
        if(rigidbody) rigidbody.isKinematic = true;
    }

    public void ActiveRag() {
        foreach (var rigidBody in rbs) {
            rigidBody.isKinematic = false; //false
        }
        animator.enabled = false; //false
        if(rigidbody) rigidbody.isKinematic = true; // khi player die iskinemactic de ko vang lung tung
    }

    public void ApplyForceLying(Vector3 force) {
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidBody.AddForce(force, ForceMode.VelocityChange); // ForceMode.VelocityChange
    }

}
