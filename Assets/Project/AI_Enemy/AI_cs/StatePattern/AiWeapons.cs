using System;
using System.Collections;
using Mono.Cecil;
using UnityEngine;

public class AiWeapons : MonoBehaviour
{
    //! gameobject = AiAgen enemy
    private RaycastWeapon currentWeapon;
    private Animator animator;
    private MeshSockets sockets;
    private WeaponIK weaponIK;
    [SerializeField] private Transform currentTarget;


    /// Initializes components needed by the AI agent's weapons.
    /// Gets references to the Animator and MeshSockets components.
    private void Start()
    {
        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIK = GetComponent<WeaponIK>();
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

    IEnumerator EuipWeapon() {
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.1f) {
            yield return null;
        }
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
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

    // chen su kien tai day vao animation equip
    public void OnAnimationEvent(string eventName) {
        if(eventName == "equipWeapon") {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketId.Righthand);
        }
    }

    public void SetTarget(Transform target) //entr() aiAttackPlayer.cs call
    {
        weaponIK.SetTargetTranform(target);
        currentTarget = target;
    } 
}
