
using System.Collections;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    //todo gameObject = Player
    //todo goi weapon through interface

    [SerializeField] private Animator playerAnimator;
    [SerializeField] public MonoBehaviour CurrenActiveWeapon {get; private set;}
    [SerializeField] ItemScriptableObject defaultWeapon;
    [SerializeField] private float timeBetweenAttacks = .5f;
    public AnimatorOverrideController overrideControllers;
    private string weaponName;
    //private int weaponDamage;
    private bool isAttacking = false;
    
    private void Start() {
        playerAnimator = GetComponent<Animator>();
        CurrenActiveWeapon = defaultWeapon.pfWeaponInterface.GetComponent<MonoBehaviour>();
        NewWeapon(CurrenActiveWeapon);
    }
    private void Update() {
        AttackCurrentWeapon();
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrenActiveWeapon = newWeapon;
        weaponName = (CurrenActiveWeapon as IWeapon).GetWeaponInfo().itemName;
        overrideControllers = (CurrenActiveWeapon as IWeapon).GetWeaponInfo().animatorOverrideController;
        playerAnimator.runtimeAnimatorController = overrideControllers;
    }

    private void AttackCurrentWeapon() {
        if(InputManager.Instance.IsAttackButton && !isAttacking && CurrenActiveWeapon && ActiveGun.Instance.IsHolstered) {
            isAttacking = true;
            StopAllCoroutines();
            AttackCoolDown();
            (CurrenActiveWeapon as IWeapon).Attack();
            playerAnimator.SetTrigger("Attack");
        }
    }
    IEnumerator TimeBetweenAttackRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
    private void AttackCoolDown() //? gia tri float coolDown quyet dinh khoang cach thuc hien hanh dong attack moi loai vu khi
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttackRoutine()); //isAttacking = false;
    }
    

}
