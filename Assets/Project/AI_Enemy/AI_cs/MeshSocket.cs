using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId socketId;
    private Transform attachPoint;
    public HumanBodyBones bone;
    Animator animator;
    public Vector3 offset;
    public Vector3 rotation;

    void Start()
    {
        Animator animator = GetComponentInParent<Animator>();
        attachPoint = new GameObject("socket" + socketId).transform;
        attachPoint.SetParent(animator.GetBoneTransform(bone));

        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);
    }

    public void Attach(Transform objectTranform) {
        objectTranform.SetParent(attachPoint, false);
    }
}
