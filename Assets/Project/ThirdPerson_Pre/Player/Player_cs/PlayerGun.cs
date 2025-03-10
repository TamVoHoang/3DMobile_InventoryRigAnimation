using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerGun : Singleton<PlayerGun>, IDataPersistence
{
    [SerializeField] private Image sprintingImage;
    [SerializeField] private bool isSprinting = false;
    public bool IsSprinting { get => isSprinting; }

    #region Movement
    public float currentSpeed = 0;
    public float CurrentSpeed (float value) => currentSpeed = value;
    [HideInInspector] public Vector3 dir; // vector 3 de ket hop huong theo x va y khi di chuyen mat dat
    [SerializeField] public float hzInput, vInput;
    private CharacterController characterController;

    public float airSpeed = 1f;
    [SerializeField] private float walkSpeed = 3, walkBackSpeed = 2;
    [SerializeField] private float runSpeed = 5, runBackSpeed = 3;
    [SerializeField] private float crouchSpeed = 2, crouchBackSpeed = 1;

    public float WalkSpeed { get => walkSpeed;}
    public float WalkBackSpeed { get => walkBackSpeed;}
    public float RunSpeed { get => runSpeed;}
    public float RunBackSpeed { get => runBackSpeed;}
    public float CrouchSpeed { get => crouchSpeed;}
    public float CrouchBackSpeed { get => crouchBackSpeed;}

    #endregion Movement

    #region GroundCheck
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.17f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] public bool GetIsGrounded;
    #endregion GroundCheck

    #region Gravity
    [SerializeField] public float gravity = -9.81f;
    Vector3 velocity;
    [SerializeField] private float jumpForce=10f;
    public bool isJumped;
    public bool SetIsJumped(bool value) => isJumped = value;
    #endregion Gravity

    #region States
    public MovementBaseState previousState;
    private MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    //public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();
    #endregion States

    [HideInInspector] public Animator animator;

    [Header("       Level Map Selection")]
    [SerializeField] private int levelTemp = 1;
    [SerializeField] private int mapSelected = 0;
    [SerializeField] int maxMap_UICanvas_SpawnerScene = 0;

    [SerializeField] private bool isTouchSpaceShip = false;
    public int MapSelected { get { return mapSelected; } }
    public bool IsTouchSpaceShip { set => isTouchSpaceShip = value; }


    bool isSetRandomPlayerPosition = false;

    [Header("       Check Fall")]
    [SerializeField] float fallHightToRespawn = -8f;
    [SerializeField] bool isRespawnRequested = false;

    #region SAVE LOAD
    Vector3 playerTransform;
    Vector3 playerTransform_TempSave;
    #endregion SAVE LOAD


    protected override void Awake() {
        base.Awake();
        
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start() {
        // plai co delay time - cho load data from coll 28 LoadData_IDataPersistence.cs
        StartCoroutine(SetPlayerPositionCoutine(0.3f));

        //levelTemp = PlayerDataJson.Instance.PlayerJson.level;
        isTouchSpaceShip = false;

        isSetRandomPlayerPosition = false;

        // locomotion player
        SwitchState(Idle);
    }

    void Update() {
        CheckFallToRespawn();
        // testing thu ham true khi dang la 1 trong nhung scene menu
        if(CheckSpawnerScene.IsInMenuScene()) return;
        if(isRespawnRequested) return;


        // random vi tri player khi vao navMesh -> truoc khi coundown =0
        if(!isSetRandomPlayerPosition) {
            Debug.Log("co vao random");
            SpawnPlayerOnNavMesh();
        }

        // neu dem chua xong count down thi return
        if(!GameManger.Instance.IsReady) return;

        SetInputSprintClock(); //? khoa gia tri isPrint  trong 1 lan nhan

        GetIsGrounded = SetIsGrounded();
        hzInput = InputManager.Instance.GetMove.x;
        vInput = InputManager.Instance.GetMove.y;

        animator.SetFloat("hzInput", hzInput); // setFloat bien ben (Blendtree) , float (nut nhan dau vao trai phai) 
        animator.SetFloat("vInput", vInput);   // gan gia tri input trai phai tai day cho gia tri float hv ben blendtree -> de biet duoc huong di tai moi state

        currentState.UpdateState(this);
        this.playerTransform_TempSave = transform.position;
    }

    private void LateUpdate() {
        // testing thu ham true khi dang la 1 trong nhung scene menu
        //if(CheckSpawnerScene.IsInMenuScene()) return;

        if(SceneManager.GetActiveScene().name == "MainMenu") return;
        if(SceneManager.GetActiveScene().name == "AccountDataOverview") return;
        if(SceneManager.GetActiveScene().name == "Testing_SpawnPlayer") return;
        if(SceneManager.GetActiveScene().name == "Login") return;

        // neu dem chua xong count down thi return
        if(!GameManger.Instance.IsReady) return;

        Falling();
        Gravity(); 
        GetDirectionAndMove();
    }
    public void SwitchState(MovementBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    private void SetInputSprintClock() {
        if(InputManager.Instance.IsSprintButton) {
            isSprinting = !isSprinting;
            InputManager.Instance.SetIsSprintButton(false);

            // thay doi mau sac cua nut nhan sprinting
            if(!isSprinting) sprintingImage.color = new Color32(255, 255, 225, 100);
            else sprintingImage.color = new Color32(255, 255, 225, 200);
        }
    }

    void GetDirectionAndMove() {
        Vector3 airDir = Vector3.zero; // tao mot bien vector 3 (x,y,z) rong chua gia tri toa do nhap tu ban phim khi o tren khong
        if (!SetIsGrounded()) airDir = transform.forward * vInput + transform.right * hzInput; // neu khong cham dat thi di chuyen theo airdir
        else dir = transform.forward * vInput + transform.right * hzInput; // huong di chuyen 2 vector3 x,z. Tu tren nhin xuong

        characterController.Move((dir.normalized * currentSpeed + airDir.normalized * airSpeed) * Time.deltaTime); //dir.normalized
    }

    public bool SetIsGrounded() {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask)) return true;
        else return false;
    }

    // method dung de tinh velocity.y => controller.Move(velocity.y)
    void Gravity() {
        if (!SetIsGrounded()) velocity.y += gravity * Time.deltaTime; // velocity.y += gravity * Time.deltaTime
        else if (velocity.y < 0) velocity.y = -2f; // khi da ko con la khong Grounded va velocity.y <0 do bi giam khi dang roi
        characterController.Move(velocity * Time.deltaTime); // player Move(tac dong Time va vector van toc theo huong y
    }

    void Falling() => animator.SetBool("Falling", !SetIsGrounded());
    public void JumpForce() => velocity.y += jumpForce;
    public void Jumped() => isJumped = true;



    //? kiem tra co touch duoc vao SpaceShip 
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Player co cham vao space ship");

        SpaceShip01 spaceShip01 = other.gameObject.GetComponent<SpaceShip01>();

        if(spaceShip01 && !isTouchSpaceShip && mapSelected >= levelTemp && mapSelected < maxMap_UICanvas_SpawnerScene) {
            // player touch spaceShip01 => tang gia tri level trong PlayerDataJson
            isTouchSpaceShip = true;
            levelTemp ++;
            PlayerDataJson.Instance.PlayerJson.level = this.levelTemp;

            // set update lap tuc cho level hien tren playerInfo canvas
            var playerInfo_UI = GetComponentInChildren<PlayerInfo_UI>();
            if(playerInfo_UI) playerInfo_UI.SetLevel(levelTemp);
        }
    }
    public void SetMapSelectAndIsTouch(int mapSelected, bool isTouchSpaceShip, int maxMap) {
        this.mapSelected = mapSelected;
        this.isTouchSpaceShip = isTouchSpaceShip;
        this.maxMap_UICanvas_SpawnerScene = maxMap;
    }
    
    //? lan dau tien khi spawn ra o Spawner Scene -> lay data PlayFab set vi tri
    //? sau khi nhan map level button -> random den vi tri theo world bound -> set player's position
    IEnumerator SetPlayerPositionCoutine(float time) {
        characterController.enabled = false;
        yield return new WaitForSeconds(time);
        transform.position = new Vector3(playerTransform.x, playerTransform.y, playerTransform.z);
        characterController.enabled = true;
    }

    void SpawnPlayerOnNavMesh() {
        WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
        if(worldBounds != null) {
            Vector3 randomPosition = RandomNavmeshLocation(10, worldBounds);
            if (randomPosition != Vector3.zero)
            {
                this.transform.position = randomPosition;

                isSetRandomPlayerPosition = true;
            }
        }
    }

    Vector3 RandomNavmeshLocation(float radius, WorldBounds worldBounds) {
        var randomDirection = worldBounds.RandomPosition();

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    //? check Player Fall down
    #region Check Fall Down
    private void CheckFallToRespawn() {
        if(transform.position.y < fallHightToRespawn) {
            Debug.Log($"request respawn");
            StartCoroutine(RespawnPlayerCo(0.5f));
        }
    }

    IEnumerator RespawnPlayerCo(float time) {
        isRespawnRequested = true;
        characterController.enabled = false;
        SpawnPlayerOnNavMesh();
        yield return new WaitForSeconds(time);
        characterController.enabled = true;
        isRespawnRequested = false;
    }
    #endregion

    #region IDataPersistence
    public void UpdateUIVisual(PlayerJson playerJson) {
        this.playerTransform = playerJson.position;
        levelTemp = playerJson.level;
    }

    public void SavePlayerData(PlayerJson playerJson) {
        // save position
        playerJson.position = this.playerTransform_TempSave;

        // save level neu cham duoc SpaceShip
        playerJson.level = this.levelTemp;

    }
    #endregion IDataPersistence

    
    //todo
}
