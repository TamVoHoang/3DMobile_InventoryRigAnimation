using Cinemachine;
using UnityEngine;

//todo gameObject = player dieu khien uu tien 2 camera
//todo camfollow and cam look around
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualAimCamera; //cam 3rd aiming
    [SerializeField] private CinemachineVirtualCamera virtualLookAroundCamera;  // cam look around 360
    [SerializeField] private GameObject spawnerCam; // cam nhin thang vao player khi o spawner scene

    private InputManager inputManager;


    private void Awake() {
        inputManager = GetComponent<InputManager>();
        spawnerCam.SetActive(false);
    }

    private void Update() {
        if(CheckSpawnerScene.CheckScene(CheckSpawnerScene.SpawnerScene)) spawnerCam.SetActive(true);
        else spawnerCam.SetActive(false);
    }


    //? nhan intput se xoay quanh player 360
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