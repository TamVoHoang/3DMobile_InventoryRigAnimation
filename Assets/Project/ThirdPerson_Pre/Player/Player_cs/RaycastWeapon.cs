using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public ActiveGun.WeaponSlots weaponSlot; // cho phep chon o nao trong enum class Active
    [SerializeField] public string weaponName; 
    [SerializeField] private bool isFiring = false;
    [SerializeField] private Transform raycsatOrigin;
    [SerializeField] private Transform raycastDestination;
    [SerializeField] TrailRenderer tracerEffect;


    private Ray ray;
    private RaycastHit hitInfo;

    public Transform SetRaycastDes (Transform rayDes)
    {
        return raycastDestination = rayDes;
    }

    public void StartFiring()
    {
        isFiring = true;
        Debug.Log("StartFiring");

        ray.origin = raycsatOrigin.position;
        ray.direction = raycastDestination.position - raycsatOrigin.position;

        var tracerEffectObject = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracerEffectObject.AddPosition(ray.origin);

        if(Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);

            tracerEffectObject.transform.position = hitInfo.point;
        }
    }

    public void StopFiring()
    {
        isFiring = false;
        Debug.Log("StopFiring");
    }
    
}
