using System.Collections;
using UnityEngine;

public class AiWeapons : MonoBehaviour
{
    //! gameobject = AiAgen enemy
    private RaycastWeapon currentWeapon;
    private Animator animator;
    private MeshSockets sockets;
    private WeaponIK weaponIK;
    [SerializeField] private Transform currentTarget;
    private bool weaponActive_Ai = false;
    [SerializeField] private float inaccuracy = 0.5f;


    /// Initializes components needed by the AI agent's weapons.
    /// Gets references to the Animator and MeshSockets components.
    private void Start() {
        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIK = GetComponent<WeaponIK>();
    }

    private void Update() {
        if(currentWeapon && currentTarget && weaponActive_Ai) {
            Vector3 target = currentTarget.position + weaponIK.TargetOffset;
            target += Random.insideUnitSphere * inaccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);//ok
            if(currentWeapon.ammoCount <=0 ) currentWeapon.ammoCount = 60;
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
        sockets.Attach(weapon.transform, MeshSockets.SocketId.Spine); // sung duoc hinh thanh trong aiAgent
    }


    //todo dung de lien ket voi aiFindWeapon.cs
    public void ActiveWeapon() {
        StartCoroutine(EuipWeapon());
    }

    public void UnActiveWeapon() {
        StartCoroutine(UnEquipWeapon());
    }

    IEnumerator EuipWeapon() {
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(0.3f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
        weaponActive_Ai = true; // da trang bi sung len ai se cho phep ban
    }
    IEnumerator UnEquipWeapon() {
        animator.SetBool("Equip", false);
        yield return new WaitForSeconds(0.3f);
        weaponActive_Ai = false; // da trang bi sung len ai se cho phep ban
    }

    public void DropWeapon() {
        if(currentWeapon) {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();

            //Destroy(currentWeapon.gameObject);//testing bo sung neu ko se bi xoay aiagent
            
            currentWeapon = null;
        }
    }
    public bool HasWeapon() {
        return currentWeapon != null;
    }

    // chen su kien tai day vao animation equip
    public void OnAnimationEvent(string eventName) {
        if(eventName == "equipWeapon") {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.RightHand);
        }
        if(eventName == "unEquipWeapon") {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.Spine);
        }
    }
    public void SetTarget(Transform target) //entr() aiAttackPlayer.cs call
    {
        weaponIK.SetTargetTranform(target);
        currentTarget = target;
    } 
}
