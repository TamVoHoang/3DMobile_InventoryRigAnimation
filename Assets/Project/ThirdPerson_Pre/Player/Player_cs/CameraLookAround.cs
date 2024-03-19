
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CameraLookAround : MonoBehaviour
{
    [SerializeField] private Transform camFollowAround;
    [SerializeField] private Vector2 xyRotation; // float xRotation, yRotation;
    [SerializeField] float speedReturn = 3f;
    [SerializeField] private float delayTimeVcLookForward = 2f;
    [SerializeField] private float xRotationLimit = 30f;
    [SerializeField] Rig bodyAimLayer;
    [SerializeField] MultiAimConstraint weaponPoseLayer; // khong cho weapon holder nhin theo co ntro
    [SerializeField] private float aimDuration = 0.3f;

    private void Awake() {
        camFollowAround = GameObject.Find("CamFollowAround").transform;
    }
    private void Update() {
        if(InputManager.Instance.GetLook.normalized != Vector2.zero) {
            bodyAimLayer.weight = 0;
            weaponPoseLayer.weight = 0;
            // xRotation += InputManager.Instance.GetLook.y;
            // yRotation -= InputManager.Instance.GetLook.x;
            // xRotation = Mathf.Clamp(xRotation, -30f, 70f);

            xyRotation.x += InputManager.Instance.GetLook.y;
            xyRotation.y -= InputManager.Instance.GetLook.x;
            xyRotation.x = Mathf.Clamp(xyRotation.x, -xRotationLimit, xRotationLimit);
        } 
        else
        {
            weaponPoseLayer.weight += Time.deltaTime / aimDuration;
            if(bodyAimLayer.weight < 0.5f) bodyAimLayer.weight += Time.deltaTime / aimDuration;
        }

    }
    
    private void LateUpdate() {
        CameraRotation();
    }

    private void CameraRotation() {
        if(InputManager.Instance.GetLook.normalized != Vector2.zero) {
            Quaternion rotation = Quaternion.Euler(xyRotation.x, xyRotation.y, 0f);
            camFollowAround.rotation = rotation; // xoay theo huong di chuyen
        } else {
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(delayTimeVcLookForward);
        camFollowAround.rotation = Quaternion.Slerp(camFollowAround.rotation, 
                PlayerController.Instance.transform.rotation, speedReturn * Time.deltaTime);
        xyRotation = Vector2.zero; //xRotation = 0; yRotation = 0;
    }

}
