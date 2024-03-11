using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    //todo game object = gun pistol and SMG001
    
    public ActiveGun.WeaponSlots weaponSlot; // cho phep chon o nao trong enum class Active
    [SerializeField] public string weaponName; 
    [SerializeField] private bool isFiring = false;
    [SerializeField] private Transform raycsatOrigin; // tren ong sung
    [SerializeField] private Transform raycastDestination; // crossHairTarget position
    [SerializeField] TrailRenderer tracerEffect;
    [SerializeField] private ParticleSystem[] muzzleFlash; // keo tha muzzleFlash vao khai bao de loa sang khi ban
    [SerializeField] private ParticleSystem hiteffect; // tao hieu ung bi ban tren be mat - keo tha doi tuong vao 

    private Ray ray;
    private RaycastHit hitInfo;

    public Transform SetRaycastDes (Transform rayDes)
    {
        return raycastDestination = rayDes;
    }
    public bool IsFiring { get => isFiring;}
    public bool SetIsFiring(bool value) => isFiring = value;
    private void Start() {
        //ActiveGun.Instance.startFiring = StartFiring;
    }

    private void Update() {

    }
    public void UpdateWeapon() {
        if(InputManager.Instance.IsAttackButton) {
            //InputManager.Instance.SetIsAttackButton(false); // ban tung phat khi nhan 1 lan chuot
            StartFiring();
        }
        if(!InputManager.Instance.IsAttackButton && isFiring) StopFiring();
    }
    private void StartFiring()
    {
        Debug.Log("StartFiring");
        isFiring = true;
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }

        ray.origin = raycsatOrigin.position;
        ray.direction = raycastDestination.position - raycsatOrigin.position;

        var tracerEffectObject = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracerEffectObject.AddPosition(ray.origin);

        if(Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
            tracerEffectObject.transform.position = hitInfo.point;

            Debug.Log("Hit: " + hitInfo.transform.name);
            hiteffect.transform.position = hitInfo.point;
            hiteffect.transform.forward = hitInfo.normal;
            hiteffect.Emit(1);
        }
    }

    public void StopFiring()
    {
        Debug.Log("StopFiring");
        isFiring = false;
    }

    
}
