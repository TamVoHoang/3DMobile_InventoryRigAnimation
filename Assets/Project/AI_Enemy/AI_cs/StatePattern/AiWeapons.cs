using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AiWeapons : MonoBehaviour
{
    //! gameobject = AiAgen enemy
    public enum WeaponState {
        Holster,
        Active,
        Reloading
    }
    
    private WeaponState weaponState = WeaponState.Holster;
    //private bool weaponActive_Ai = false;
    //private bool isReloading = false;
    private bool IsActive() {
        return weaponState == WeaponState.Active;
    }
    private bool IsHolster() {
        return weaponState == WeaponState.Holster;
    }
    private bool IsReloading() {
        return weaponState == WeaponState.Reloading;
    }

    private RaycastWeapon currentWeapon;
    public RaycastWeapon CurrentWeapon{get => currentWeapon;}
    private RaycastWeapon[] weapons =  new RaycastWeapon[2];
    private Animator animator;
    private MeshSockets sockets;
    private WeaponIK weaponIK;
    [SerializeField] private Transform currentTarget;
    
    [SerializeField] private float inaccuracy = 0.5f;
    [SerializeField] private float waitingTimeEquipWeapon = 0.5f;

    //reloading attach drop mag
    private GameObject magHand;
    [SerializeField] private float dropFroce = 1.5f;
    

    /// Initializes components needed by the AI agent's weapons.
    /// Gets references to the Animator and MeshSockets components.
    private void Start() {
        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIK = GetComponent<WeaponIK>();
    }

    private void Update() {
        /* if(currentWeapon && currentTarget && IsActive() && !IsReloading()) {
            weaponIK.SetTargetOffset_Aim(currentWeapon.TargetOffset_AImWeaponIK); // lay targetoffset tung loia bo vao
            Vector3 target = currentTarget.position + weaponIK.TargetOffset;
            target += Random.insideUnitSphere * inaccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);//ok
        } */

        //! testing
        if(currentWeapon && currentTarget) {
            weaponIK.SetTargetOffset_Aim(currentWeapon.TargetOffset_AImWeaponIK);
            Vector3 target = currentTarget.position + weaponIK.TargetOffset;
            target += Random.insideUnitSphere * inaccuracy;

            if(IsActive()) currentWeapon.StartFiring();
            if(!IsActive() || IsReloading()) currentWeapon.StopFiring();

            currentWeapon.UpdateWeapon(Time.deltaTime, target);//ok
        }
    }

    public void SetFiring(bool enabled) {
        if(enabled) {
            currentWeapon.StartFiring(); //=> xet isFiring - UpdateFiring() - FireBullet() - toa vien dan - UpdateBullet() mo phong dan bay vay ly
        } else {
            currentWeapon.StopFiring();
        }
    }

    // khi chap voi pickup se goi ham nay chay
    public void EquipWeapon(RaycastWeapon weapon) {
        currentWeapon = weapon;
        if(currentWeapon.WeaponName == "rifle") {
            sockets.Attach(weapon.transform, MeshSockets.SocketId.Spine); // sung duoc hinh thanh trong aiAgent
        }
        if(currentWeapon.WeaponName == "pistol") {
            sockets.Attach(weapon.transform, MeshSockets.SocketId.UperLegR); // sung duoc hinh thanh trong aiAgent
        }
    }

    //todo dung de lien ket voi aiFindWeapon.cs
    public void ActiveWeapon() {
        StartCoroutine(EquipWeaponAnimation());
    }

    public void DeActiveWeapon() {
        SetTarget(null);
        SetFiring(false);
        StartCoroutine(HolsterWeaponAnimation());
    }
    public void ReloadWeapon() {
        if(IsActive()) {
            StartCoroutine(RealoadWeaponAnimation());
        }
    }

    IEnumerator EquipWeaponAnimation() {
        //weaponState = WeaponState.Holster; //! isactive = false - sung chua trang bi
        animator.runtimeAnimatorController = currentWeapon.runtimeAnimatorController;
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(waitingTimeEquipWeapon);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
        weaponState = WeaponState.Active;
    }
    IEnumerator HolsterWeaponAnimation() {
        weaponState = WeaponState.Holster; // da trang bi sung len ai se cho phep ban weaponState = WeaponState.Active;

        animator.runtimeAnimatorController = currentWeapon.runtimeAnimatorController;
        animator.SetBool("Equip", false);
        yield return new WaitForSeconds(waitingTimeEquipWeapon);

        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
    }
    IEnumerator RealoadWeaponAnimation() {
        weaponState = WeaponState.Reloading;
        animator.runtimeAnimatorController = currentWeapon.runtimeAnimatorController;
        animator.SetTrigger("Reload_Weapon");
        weaponIK.enabled = false;
        yield return new WaitForSeconds(0.7f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        weaponIK.enabled = true;
        weaponState = WeaponState.Active;
    }

    public void DropWeapon() {
        if(currentWeapon) {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }
    public bool HasWeapon() {
        return currentWeapon != null;
    }

    //entr() aiAttackPlayer.cs call
    public void SetTarget(Transform target) {
        weaponIK.SetTargetTranform(target);
        currentTarget = target;
    }

    // chen su kien tai day vao animation equip
    public void OnAnimationEvent(string eventName) {
        switch (eventName) {
            case "equip_Rifle":
                AttachRifle();
                break;
            case "equip_Pistol":
                AttachPistol();
                break;
            case "deatch_mag":
                DeatchMag();
                break;
            case "drop_mag":
                DropMag();
                break;
            case "refill_mag":
                RefillMag();
                break;
            case "attach_mag":
                AttachMag();
                break;
            default:
                break;
        }
    }

    private void AttachRifle() {
        bool equipping = animator.GetBool("Equip");
        if(equipping) {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.RightHand);
        } else {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.Spine);
        }
    }

    private void AttachPistol() {
        bool equipping = animator.GetBool("Equip");
        if(equipping) {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.RightHandPistol);
        } else {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.UperLegR);
        }
    }

    void DeatchMag() {
        var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand); // tay trai se duoc animate
        RaycastWeapon weapon= currentWeapon;
        magHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }
    void DropMag() {
        GameObject droppedMag = Instantiate(magHand, magHand.transform.position, magHand.transform.rotation);
        droppedMag.SetActive(true);
        Rigidbody body =  droppedMag.AddComponent<Rigidbody>();

        Vector3 dropDirection = -gameObject.transform.right;
        dropDirection += Vector3.down;
        
        body.AddForce(dropDirection * dropFroce, ForceMode.Impulse);
        droppedMag.AddComponent<BoxCollider>();
        Destroy(droppedMag,2f);
        magHand.SetActive(false);

        /* GameObject droppedMag = Instantiate(magHand, magHand.transform.position, magHand.transform.rotation);
        droppedMag.AddComponent<Rigidbody>();
        droppedMag.AddComponent<BoxCollider>();
        magHand.SetActive(false); */
    }
    void RefillMag() {
        magHand.SetActive(true);
    }
    void AttachMag() {
        RaycastWeapon weapon = currentWeapon;
        weapon.magazine.SetActive(true);
        Destroy(magHand);
        weapon.ammoCount = weapon.clipSize;
        animator.ResetTrigger("Reload_Weapon");
    }


}
