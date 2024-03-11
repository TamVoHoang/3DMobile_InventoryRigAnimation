using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    //todo game object = crossHairTarget folder con cua mainCamera
    //todo gan position hitInfo cho vi tri crossHairTarget

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

        Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 0.1f);
    }


}
