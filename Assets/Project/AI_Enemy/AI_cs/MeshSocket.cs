using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId socketId;
    [SerializeField] private Transform attachPoint;

    void Start()
    {
        attachPoint = transform.GetChild(0);
    }


    public void Attach(Transform objectTranform) {
        objectTranform.SetParent(attachPoint, false);
    }
}
