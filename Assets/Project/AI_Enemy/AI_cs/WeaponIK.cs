using System;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight;

}
public class WeaponIK : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private Transform targetTransform;// vi tri cua target
    [SerializeField] private Transform aimTransform;// vi tri ray cast
    [SerializeField] private Vector3 targetOffset;
    public Vector3 TargetOffset{get => targetOffset;}
    //public Transform bone;
    private int interations = 10;
    [Range(0,1)]
    [SerializeField] private float weight = 1.0f;

    [SerializeField] private float angleLimit = 90f;
    [SerializeField] private float distanceLimit = 1.5f;


    [SerializeField] private HumanBone[] humanBones;
    [SerializeField] private Transform[] boneTransforms;


    private void Awake() {
    }
    private void Start() {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++) {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    //todo ham gioi han goc xoay ai agent
    private Vector3 GetTargetPosition() {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngel = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngel > angleLimit) {
            blendOut += (targetAngel - angleLimit) / 50f;
        }

        //khi target qua gan thi tia ray se ko nham voa no nua
        float targetDistance = targetDirection.magnitude; 
        if(targetDistance < distanceLimit) {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }
    private void Update() {
    }

    private void LateUpdate() {

        if(aimTransform == null) return;
        if(targetTransform == null) return;
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < interations; i++) {
            for (int b = 0; b < boneTransforms.Length; b++) {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weight;
                AimAtTarget(bone, targetPosition, boneWeight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight) {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection,targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTranform(Transform target) {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim) {
        aimTransform = aim;
    }
    public void SetTargetOffset_Aim(Vector3 targetOffset) {
        this.targetOffset = targetOffset;
    }
}
