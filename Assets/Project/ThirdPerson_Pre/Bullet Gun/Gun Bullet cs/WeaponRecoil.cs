using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    //[SerializeField] PlayerAim aimRecoil;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraShake;

    // rigControllerRecoil se Play (ten animation trong layer weaponRecoil)
    // do script nay dang o folder gun (khi play chua duoc kich hoat
    // khi player(chua script activeWeapon) cham vao
    // se gan gia tri rigcontroller (tu ben class activeWapon) vao day
    [HideInInspector] public Animator rigControllerRecoil; 
    public Vector2[] recoilParten;
    public float duration;

    float virtualRecoil;
    float horizontalRecoil;
    float time;
    int index;
    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
        //aimRecoil = GetComponentInParent<PlayerAim>();
    }
    private void Start()
    {
        // cho phep bien aimRecoil tai day truy cap  vao class characterAim
        //aimRecoil = GetComponentInParent<ChracterAim>();
    }
    public void Reset()
    {
        index = 0;
    }
    int NextIndex(int index)
    {
        return (index+1)%recoilParten.Length;
    }
    public void GenerateRecoil(string weaponName)
    {
        time = duration; // gan time >0
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        horizontalRecoil = recoilParten[index].x;
        virtualRecoil = recoilParten[index].y;
        index = NextIndex(index);

        // thuc hien animation 2 folder bodyRecoil va weaponRecoil( da record position)
        rigControllerRecoil.Play("weapon_recoil_" + weaponName,1,0.0f);
    }
    void Update()
    {
        if(time >0) // thoi gian sung dao dong khi giat
        {
            // aimRecoil.yAxis -= (virtualRecoil * Time.deltaTime)/duration;
            // aimRecoil.xAixs -= (horizontalRecoil * Time.deltaTime) / duration;
            time -= Time.deltaTime; // ha time de dung recoil
        }
    }
}
