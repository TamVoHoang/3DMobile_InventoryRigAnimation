using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : Singleton<PlayerGun>, IDataPersistence
{
    [SerializeField] private Image sprintingImage;
    private bool isSprinting = false;
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

    #region SAVE LOAD
    [SerializeField] private Vector3 playerTransform;
    [SerializeField] private Vector3 playerTransform_TempSave;

    IEnumerator SetPlayerPositionCoutine(float time) {
        characterController.enabled = false;
        yield return new WaitForSeconds(time);
        transform.position = new Vector3(playerTransform.x,
                                        playerTransform.y, 
                                        playerTransform.z);
        
        characterController.enabled = true;
    }

    #endregion SAVE LOAD


    protected override void Awake() {
        base.Awake();
        //playerTransform = PlayerDataLocal_Temp.Instance.position_Temp; //! chay ok
        //LoadData(PlayerDataJson.Instance.PlayerJson); //! load thong qua ham interface
        
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start() {
        StartCoroutine(SetPlayerPositionCoutine(0.3f)); //! chay ok

        SwitchState(Idle);
    }

    public void Update() {
        SetInputSprintClock(); //? khoa gia tri isPrint  trong 1 lan nhan

        GetIsGrounded = SetIsGrounded();
        hzInput = InputManager.Instance.GetMove.x;
        vInput = InputManager.Instance.GetMove.y;

        animator.SetFloat("hzInput", hzInput); // setFloat bien ben (Blendtree) , float (nut nhan dau vao trai phai) 
        animator.SetFloat("vInput", vInput);   // gan gia tri input trai phai tai day cho gia tri float hv ben blendtree -> de biet duoc huong di tai moi state

        // Gravity();
        //Falling();
        // GetDirectionAndMove();

        currentState.UpdateState(this);
        this.playerTransform_TempSave = transform.position;
    }
    private void FixedUpdate() {
        // Gravity();
        // Falling();
        // GetDirectionAndMove(); // camera bi lac khi dung fixedUpdate
    }
    private void LateUpdate() {
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
            if(!isSprinting) sprintingImage.color = new Color32(255, 255, 225, 225);
            else sprintingImage.color = new Color32(255, 255, 225, 100);
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
    void Gravity() // method dung de tinh velocity.y => controller.Move(velocity.y)
    {
        if (!SetIsGrounded()) velocity.y += gravity * Time.deltaTime; // velocity.y += gravity * Time.deltaTime
        else if (velocity.y < 0) velocity.y = -2f; // khi da ko con la khong Grounded va velocity.y <0 do bi giam khi dang roi
        characterController.Move(velocity * Time.deltaTime); // player Move(tac dong Time va vector van toc theo huong y
    }

    void Falling() => animator.SetBool("Falling", !SetIsGrounded());
    public void JumpForce() => velocity.y += jumpForce;
    public void Jumped() => isJumped = true;

    #region IDataPersistence
    public void LoadPlayerData(PlayerJson playerJson) {
        this.playerTransform = JsonUtility.FromJson<Vector3>(playerJson.position);
    }

    public void SavePlayerData(PlayerJson playerJson) {
        playerJson.position = JsonUtility.ToJson(this.playerTransform_TempSave);
    }
    #endregion IDataPersistence

    //todo
}
