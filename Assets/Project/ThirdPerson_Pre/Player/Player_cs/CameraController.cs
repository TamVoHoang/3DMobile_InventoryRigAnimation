using Cinemachine;
using UnityEngine;

//todo gameObject = player dieu khien uu tien 2 camera
//todo camfollow and cam look around
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualAimCamera;
    [SerializeField] private CinemachineVirtualCamera virtualLookAroundCamera;
    [SerializeField] private CinemachineVirtualCamera lookAtPlayerCam;

    private InputManager inputManager;

    CrossHairTarget crossHairTarget;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
        crossHairTarget = FindObjectOfType<CrossHairTarget>();
    }
    private void Update() {
        if(CheckSpawnerScene.CheckScene(CheckSpawnerScene.SpawnerScene)) {
            crossHairTarget.SetAimLookAt(new Vector3 (0, 0, -20));
            lookAtPlayerCam.Priority = 50;
        } 
        else {
            crossHairTarget.SetAimLookAt(new Vector3 (0, 0, 20));
            lookAtPlayerCam.Priority = -1;
            PriorityCamera();
        }
        
    }

    private void PriorityCamera() {
        if(inputManager.GetLook.normalized != Vector2.zero) {
            virtualAimCamera.Priority = 1;
            virtualLookAroundCamera.Priority = 20;
        } else {
            virtualAimCamera.Priority = 20;
            virtualLookAroundCamera.Priority = 1;
        }
    }

    //todo
}