using UnityEngine;

//todo gameObject = doi tuong ben trong player
//todo active and deactive death vitual camera
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCameraBase deathCam;
    public void ActiveDeathCam() => deathCam.Priority = 30;

    public void DeActiveDeathCam() => deathCam.Priority = 10;
}
