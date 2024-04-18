using System.Collections;
using UnityEngine;

public class AiWeapons : MonoBehaviour
{
    //! gameobject = AiAgen enemy
    public enum WeaponState {
        Holstering,
        Holstered,
        Activating,
        Active,
        Reloading
    }

    public enum WeaponSlot {
        Primary,
        Secondary
    }
    
    private WeaponState weaponState = WeaponState.Holstered;
    private bool IsActive() {
        return weaponState == WeaponState.Active;
    }
    private bool IsHolstered() {
        return weaponState == WeaponState.Holstered;
    }
    private bool IsReloading() {
        return weaponState == WeaponState.Reloading;
    }

    //private RaycastWeapon currentWeapon;
    public RaycastWeapon currentWeapon {
        get {
            return weapons[current];
        }
    }

    public WeaponSlot currentWeaponSlot {
        get { return (WeaponSlot)current; }
    }
    private RaycastWeapon[] weapons =  new RaycastWeapon[2];
    private int current = 0;
    private Animator animator;
    private MeshSockets sockets;
    private WeaponIK weaponIK;
    [SerializeField] private Transform currentTarget;    
    [SerializeField] private float inAccuracy = 0.5f;
    [SerializeField] private float waitingTimeEquipWeapon = 0.5f;

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
        /* if(currentWeapon && currentTarget && IsActive()) {
            weaponIK.SetTargetOffset_Aim(currentWeapon.TargetOffset_AImWeaponIK); // lay targetoffset tung loia bo vao
            Vector3 target = currentTarget.position + weaponIK.TargetOffset;
            target += Random.insideUnitSphere * inaccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);//ok
        } */

        //! testing
        if(currentWeapon && currentTarget) {
            weaponIK.SetTargetOffset_Aim(currentWeapon.TargetOffset_AImWeaponIK);
            Vector3 target = currentTarget.position + weaponIK.TargetOffset;
            target += Random.insideUnitSphere * inAccuracy;

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
    public bool HasWeapon() {
        return currentWeapon != null;
    }
    public void SetTarget(Transform target) {
        weaponIK.SetTargetTranform(target);
        currentTarget = target;
    }

    // khi chap voi pickup se goi ham nay chay
    public void Equip(RaycastWeapon weapon) {
        //currentWeapon = weapon;
        weapons[(int)weapon.weaponSlot] = weapon; // gan wepaon vao weapons[]
        if(weapon.WeaponName == "rifle") {
            sockets.Attach(weapon.transform, MeshSockets.SocketId.Spine); // sung duoc hinh thanh trong aiAgent
        }
        if(weapon.WeaponName == "pistol") {
            sockets.Attach(weapon.transform, MeshSockets.SocketId.UperLegR); // sung duoc hinh thanh trong aiAgent
        }
    }

    //todo dung de lien ket voi aiFindWeapon.cs
    public void ActiveWeapon() {
        StartCoroutine(EquipWeaponAnimation());
    }

    public void HolsterWeapon_FindAmmo() => StartCoroutine(HolsterWeaponAnimation());

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

    public void SwitchWeapon(WeaponSlot slot) {
        Debug.Log("Switch Weapon Slot = "+slot);
        if(IsHolstered()) {
            Debug.Log("switch 1");
            current = (int)slot;
            ActiveWeapon();
            return;
        }

        int equipIndex = (int)slot;
        Debug.Log(current + "//" + equipIndex + "//" + IsActive()); 
        if(IsActive() && current != equipIndex) {
            Debug.Log("switch 2");
            StartCoroutine(SwitchWeaponAnimation(equipIndex));
        }
    }

    public int Count() {
        int count = 0;
        foreach (var weapon in weapons) {
            if(weapon != null) count ++;
        }
        return count;
    }
    IEnumerator EquipWeaponAnimation() {
        weaponState = WeaponState.Activating;

        Debug.Log(currentWeapon);
        animator.runtimeAnimatorController = currentWeapon.runtimeAnimatorController;
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(waitingTimeEquipWeapon);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        weaponIK.enabled = true; //! de khi doi sung ngan cu ly gan ko bi loi
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);

        weaponState = WeaponState.Active;
    }

    IEnumerator HolsterWeaponAnimation() {
        
        weaponState = WeaponState.Holstering; // da trang bi sung len ai se cho phep ban weaponState = WeaponState.Active;
        animator.runtimeAnimatorController = currentWeapon.runtimeAnimatorController;
        animator.SetBool("Equip", false);
        weaponIK.enabled = false; //! de khi doi sung ngan cu ly gan ko bi loi
        yield return new WaitForSeconds(0.3f); //! 0.3f

        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        
        weaponState = WeaponState.Holstered;
        
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
    IEnumerator SwitchWeaponAnimation(int index) {
        Debug.Log("co vao switch animation");
        yield return StartCoroutine(HolsterWeaponAnimation());
        current = index;
        Debug.Log("HolsterWeaponAnimation "+ current + "//" + index);
        //
        yield return StartCoroutine(EquipWeaponAnimation());
        Debug.Log("EquipWeaponAnimation "+ current + "//" + index);
        Debug.Log("co ra switch animation");
    }

    public void DropWeapon() {
        if(currentWeapon) {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();

            //currentWeapon = null;
            weapons[current] = null;
        }
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
        //weapon.ammoCount = weapon.clipSize;
        weapon.RefillAmmo(); // nap dan
        animator.ResetTrigger("Reload_Weapon");
    }
    public void RefillAmmo_AiWeapon(int clipCount) {
        var weapon = currentWeapon;
        if(weapon) {
            weapon.clipCount += clipCount;
        }
    }
    public bool IsLowAmmo_AiWeapon() {
        var weapon = currentWeapon;
        if(weapon) {
            return weapon.IsLowAmmo();
        }
        return false;
    }
    
    //todo
}
