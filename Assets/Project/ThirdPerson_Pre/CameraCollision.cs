using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player;
    public Transform camTransform;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    public Vector3 dollyDirAdjusted;
    public float distance;

    private Vector3 dollyDir;

    void Awake()
    {
        dollyDir = camTransform.localPosition.normalized;
        distance = camTransform.localPosition.magnitude;
    }

    private void LateUpdate() {
        if(CheckSpawnerScene.IsInMenuScene()) return;
        
        Vector3 desiredCameraPos = player.position + player.rotation * dollyDir * maxDistance;
        RaycastHit hit;

        if (Physics.Linecast(player.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }

}
