using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSockets : MonoBehaviour
{
    public enum SocketId
    {
        Spine,
        Righthand
    }
    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();

    void Start()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (var socket in sockets)
        {
            socketMap[socket.socketId] = socket;
        }
    }

    public void Attach(Transform objectTranform, SocketId socketId) {
        socketMap[socketId].Attach(objectTranform);
    }

}
