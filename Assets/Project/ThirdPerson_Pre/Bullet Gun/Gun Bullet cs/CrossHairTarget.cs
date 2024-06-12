using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossHairTarget : MonoBehaviour
{
    //todo game object = crossHairTarget folder con cua mainCamera
    //todo gan position hitInfo cho vi tri crossHairTarget

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hitInfo;

    [SerializeField] Transform aimLookAt;
    private void Start() {
        mainCamera = Camera.main;
        
    }

    private void Update() {
        // khi xoay cam lookat vao nhan vat, xet aimLook gia tri am
        // ly do => toan bo rigBody Aim dang nhin ve huong AimlookAt
        // huong forward cua camera
        if(CheckSpawnerScene.CheckScene(CheckSpawnerScene.SpawnerScene)) {
            aimLookAt.localPosition = new Vector3 (0, 0, -20);
            return;
        } 
        else {
            aimLookAt.localPosition = new Vector3 (0, 0, 20);
        }

        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        Physics.Raycast(ray, out hitInfo);

        transform.position = hitInfo.point;

        Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 0.1f);
    }


    //todo
}
