using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    //todo game object = gun pistol and SMG001
    class Bullet
    {
        public float time; // givenTime of bullet
        public Vector3 initialPos;
        public Vector3 initialVelocity;
        public TrailRenderer tracer; // co the thay bang gameObject (bulletShape)
        public int bounce;
    }

    public ActiveGun.WeaponSlots weaponSlot; //! cho phep chon o nao trong enum class Active
    [SerializeField] private string weaponName;
    public string WeaponName {get {return weaponName;}}
    [HideInInspector] public Transform raycastOrigin; // tren ong sung
    ////public Transform GetRaycastOrigin {get {return raycastOrigin;}}
    //[SerializeField] private Transform raycastDes; // crossHairTarget position
    //public Transform SetRaycastDes (Transform rayDes) => raycastDes = rayDes;
    [SerializeField] TrailRenderer tracerEffect;
    [SerializeField] private ParticleSystem[] muzzleFlash; // keo tha muzzleFlash vao khai bao de loa sang khi ban
    [SerializeField] private ParticleSystem hiteffect; // tao hieu ung bi ban tren be mat - keo tha doi tuong vao 
    
    [Header("RAYCAST")]
    private Ray ray;
    private RaycastHit hitInfo;
    [SerializeField] private float distance;
    public bool IsFiring { get => isFiring;}
    public bool SetIsFiring(bool value) => isFiring = value;

    [Header("FIRE RATE")]
    private bool isFiring = false;
    public float fireRate = 25f;    // bullets per seconds
    public float accumulateTime;    // chi duoc ban khi accumulate >=0

    [Header("AMOUNTCOUNT")]
    public int ammoCount = 30;
    public int clipSize = 30;
    public int clipCount = 1;// 2 lan thay dan

    [Header("PHYSICAL BULLET")]
    public float bulletSpeed = 1000.0f;
    [SerializeField] private float bulletDrop = 0.0f;       // bao xa bullet se roi xuong
    private List<Bullet> Bullets = new List<Bullet>();      //list add vien dan KHI FIRE
    public int bulletCount => Bullets.Count;
    private float maxLifeTime = 0.05f;                      //todo 5
    [SerializeField] private float damage = 10.0f;          // damage of sung
    public float Damage { get => damage; private set => damage = value; }
    
    [Header("           RELOAD")]
    public GameObject magazine;
    [SerializeField] LayerMask layerMask;                   //todo loai layerMash ma cay sung se ban trung

    public RuntimeAnimatorController runtimeAnimatorController;
    [SerializeField] private Vector3 targetOffset_AimWeaponIK;
    public Vector3 TargetOffset_AImWeaponIK{get => targetOffset_AimWeaponIK;}
    private void Awake() {
        
    }

    private Vector3 GetPosition(Bullet bullet)  //simualateBullet goi
                                                // TINH VI TRI VIEN DAN THEO time = 0 va time = time.deltaTime tu simulateBullet 
                                                // s1= s0 + vt + 1/2gt*t
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPos) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }
    private Bullet CreateBullet(Vector3 position, Vector3 velocity) // fireBullet goi
                                                                    // tra ve lai BULLET (THUOC TINH + SHAPE VAT LY)
    {
        Bullet bullet = new Bullet(); // khoi tao doi tuong + gan thuoc tinh
        bullet.initialPos = position; // vien dan duoc tao o dau - thi truyen tham so Pos + velocity o do vao
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;

        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity); // khoi tao bulletShap vat ly
        bullet.tracer.AddPosition(position);
        //bullet.bounce = maxBounces;

        return bullet; //vien dan dc tao ra va se add vao Bullet<list> tai ham FIREBULLET
    }

    //? ActiveGun.cs || coll 68 Update() call this function
    public void UpdateWeapon(float deltaTime, Vector3 target) {
        if (isFiring) UpdateFiring(target);
        accumulateTime += deltaTime;

        UpdateBullet(deltaTime);
    }

    //? StartFiring() || UpdateFiring()
    private void FireBullet(Vector3 target)
    {
        if(ammoCount <= 0) return;

        ammoCount--;
        Debug.Log("Ban");
        
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
        ////FireBulletWithRaycast(); //? BAN DAN raycast tai day

        Vector3 directionToAimPoint = target - raycastOrigin.position; // huong tu vt tren nong sung den muc tieu
        Debug.Log("dir =" + directionToAimPoint);

        Vector3 velocity = directionToAimPoint.normalized * bulletSpeed; // vector van toc ban dau tai nong sung
        //Debug.Log("vel =" + velocity);

        var bullet = CreateBullet(raycastOrigin.position, velocity); // gan toan bo thuoc tinh cua vien dan tao ra tren ham create bullet cho bien var bullet
        Bullets.Add(bullet); // gan bien bulet sau khi duoc khoi tao tren CREATEBULLET gan vao list => simulate
        //recoil.GenerateRecoil(weaponName); //? sun giat

    }

    public void StartFiring() //? coll 33 UpdateWeapon() call this function
    {
        Debug.Log("StartFiring");
        isFiring = true;
        if(accumulateTime > 0.0f) accumulateTime = 0.0f; // khi ban thoi gian cho se tro ve 0
    }

    public void StopFiring()
    {
        Debug.Log("StopFiring");
        isFiring = false;
    }

    public bool ShouldReload() => ammoCount ==0 && clipCount > 0; // het dan, con bang dan
    public bool IsLowAmmo() => ammoCount == 0 && clipCount == 0; // het dan, het bang dan
    public void RefillAmmo() {
        ammoCount = clipSize; // nap so luong dan = so luong dan trong bang dan
        clipCount --; // giam so luong bang dan
    }

    public void UpdateFiring(Vector3 target) // dang dc goi tu folder player (characterAimming class)
    {
        float fireInterval = 1.0f / fireRate; // 1 vien = 1/25s
        while (accumulateTime >= 0.0f) // time.deltaTime la hang so, neu fireRate >> thi vong lap while se chay nhiu lan
                                       // do accumulatedime >=0 va ra nhieu vien dan
        {
            FireBullet(target);
            accumulateTime -= fireInterval; // phai cho 1/25s de ban vien tiep theo - de dam bao 1s ban duoc 25 vien
        }
    }
    private void SimulateBullets(float deltaTime) // updateBullet goi
    {
        Bullets.ForEach(bullet => // moi bullet trong list Bullets - duoc tao ra tu ham CreateBullet- khi FireBullte yeu cau
        {
            Vector3 p0 = GetPosition(bullet);   // goi ham GetPos time = 0 do khi khoi tao CreateBullet de add vao list - da gan bullet.time=0
            bullet.time += deltaTime;           // lay thoi gian time.deltatime ben ham goi AimstateMnaager gan vao bullet.time cua vien dan dang can simulate

            Vector3 p1 = GetPosition(bullet);   //  toa do p1 sau khi time cua vien tang len. (chinh la time nhan tu time.deltaTime ben AimSatateMnaager goi qua)
            RaycastSegment(p0, p1, bullet);
        }); 
    }

    private void DestroyBullets() => Bullets.RemoveAll(bullet => bullet.time >= maxLifeTime); //!updateBullet goi
    private void DesTroyBulletTracer() {
        foreach (var bulletTracer in Bullets)
        {
            if(bulletTracer.time >= maxLifeTime){

                Destroy(bulletTracer.tracer);
            } 
        }
    }
    public void UpdateBullet(float deltaTime) //updateWeapon() goi => (simulate + destroy)
    {
            SimulateBullets(deltaTime);
            DestroyBullets();       //destroy bullets trong list Bullets
            DesTroyBulletTracer();  // destory bullet.Tracer
    }
    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet) // updateBullet goi
                                                                   // chia theo tung doan nho p0 - p1 theo tiem moi frame
    {
        Vector3 direction = end - start;
        distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction; // co 1 tia ray ban ra tu nong sung

        if (Physics.Raycast(ray, out hitInfo, distance, layerMask)) // tia ray da ban ra tu nong de co duoc hitInfo
        {
            hiteffect.transform.position = hitInfo.point; // vi tri cua vet dan la vi tri tia hit va cham
            hiteffect.transform.forward = hitInfo.normal;

            hiteffect.Emit(1);
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f); // VE TIA
            Debug.Log(hitInfo.transform.name);
            //drawLaserToAimPoint(); //VE TIA
            bullet.tracer.transform.position = hitInfo.point; // vi tri di den giau man hinh croosHair
            bullet.time = maxLifeTime;
            end = hitInfo.point;
            print("stop");

            //? bullet ricohet
            /* if (bullet.bounce > 0) {
                bullet.time = 0;
                bullet.initialPos = hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hitInfo.normal);
                bullet.bounce--;
            } */

            //? Collision Impluse
            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d) rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);

            //? vien dan va cham voi hitbox.cs tren nguoi enemy HOAC tren nguoi Player
            var hitBox = hitInfo.collider.GetComponent<HitBox>();
            if (hitBox) hitBox.OnRaycastHit(this, ray.direction); //todo this = cs cay sung nay

        }

        bullet.tracer.transform.position = end;
    }

    //? FIRE BULLET WITH RAYCAST
    /* private void FireBulletWithRaycast()
    {
        //Debug.Log("FireBulletWithRaycast");
        ray.origin = raycsatOrigin.position;
        ray.direction = raycastDestination.position - raycsatOrigin.position;

        var tracerEffectObject = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracerEffectObject.AddPosition(ray.origin);

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
            tracerEffectObject.transform.position = hitInfo.point;

            Debug.Log("Hit: " + hitInfo.transform.name);
            hiteffect.transform.position = hitInfo.point;
            hiteffect.transform.forward = hitInfo.normal;
            hiteffect.Emit(1);
        }
    } */
    
    //todo
}