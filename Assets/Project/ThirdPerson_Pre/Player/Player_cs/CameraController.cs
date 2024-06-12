using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    //gameObject = player dieu khien uu tien 2 camera
    // camfollow and cam look around
    [SerializeField] private CinemachineVirtualCamera virtualAimCamera;
    [SerializeField] private CinemachineVirtualCamera virtualLookAroundCamera;
    [SerializeField] private CinemachineVirtualCamera lookAtPlayerCam;

    private InputManager inputManager;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
    }
    private void Update() {
        if(SceneManager.GetActiveScene().name == "Testing_SpawnPlayer") lookAtPlayerCam.Priority = 50;
        else {
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