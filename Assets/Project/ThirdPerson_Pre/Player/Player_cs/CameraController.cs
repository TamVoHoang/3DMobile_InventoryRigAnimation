using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualAimCamera;
    [SerializeField] private CinemachineVirtualCamera virtualLookAroundCamera;
    private InputManager inputManager;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
    }
    private void Update() {
        PriorityCamera();
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