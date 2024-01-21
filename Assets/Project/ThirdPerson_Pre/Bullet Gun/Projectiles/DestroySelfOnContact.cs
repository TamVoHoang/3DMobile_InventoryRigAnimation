using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroySelfOnContact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other)
        Destroy(gameObject);
        Debug.Log(other.name);
    }

}
