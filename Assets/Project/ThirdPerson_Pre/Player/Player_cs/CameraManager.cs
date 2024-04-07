using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCameraBase deathCam;
    public void ActiveDeathCam() {
        deathCam.Priority = 30;
    }
}
