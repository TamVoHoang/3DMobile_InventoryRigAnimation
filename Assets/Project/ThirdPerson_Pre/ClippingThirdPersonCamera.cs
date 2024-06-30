using UnityEngine;

public class ClippingThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 2f;

    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    private void Start() {
        
    }

    void LateUpdate()
    {
        if(CheckSpawnerScene.IsInMenuScene()) return;
        // Calculate desired position
        desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // Check for obstacles
        RaycastHit hit;
        if (Physics.Linecast(target.position, desiredPosition, out hit))
        {
            desiredPosition = hit.point + hit.normal * 0.2f; // Offset to avoid clipping
        }

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Calculate and set rotation
        desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
