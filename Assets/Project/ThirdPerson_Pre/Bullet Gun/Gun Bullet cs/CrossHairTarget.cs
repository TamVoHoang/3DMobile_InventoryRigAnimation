using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hitInfo;

    private void Start() {
        mainCamera = Camera.main;    
        
    }

    private void Update() {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
    }


}
