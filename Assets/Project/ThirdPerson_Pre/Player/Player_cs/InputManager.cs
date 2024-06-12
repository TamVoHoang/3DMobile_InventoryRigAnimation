using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : Singleton<InputManager>
{
    private PlayerControls playerControls;
    private Vector2 move;//ok
    private Vector2 aim;//aming
    private Vector2 look; //look arount
    [SerializeField] private bool isAttackButton;
    private bool isJumpButton;
    [SerializeField] private bool isSprintButton;
    private bool isSwitchStateButton;

    public Vector2 GetMove {get{return move;}}
    public Vector2 GetAim {get{return aim;}}
    public Vector2 GetLook {get{return look;}}
    public bool IsAttackButton {get{return isAttackButton;}}
    public bool IsJumpButton {get{return isJumpButton;}}
    public bool IsSprintButton {get{return isSprintButton;}}
    public bool IsSwitchStateButton {get{return isSwitchStateButton;}}
    
    public void SetMove(Vector2 move) => this.move = move;
    public void SetAim(Vector2 aim) => this.aim = aim;
    public void SetLook(Vector2 look) => this.look = look;
    ////public void SetIsAttackButton(bool isAttackButton) => this.isAttackButton = isAttackButton;
    public void SetIsAttackButton(bool isAttackButton) {
        this.isAttackButton = isAttackButton;
    }
    public void SetIsSprintButton(bool isSprintButton) => this.isSprintButton = isSprintButton;
    public void SetIsJumpButton(bool isJumpButton) => this.isJumpButton = isJumpButton;
    public void SetIsSwitchStateButton(bool isSwitchStateButton) => this.isSwitchStateButton = isSwitchStateButton;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
        
        //todo lay gia tri trong PlayerOCntrols -> AttackButton
        playerControls.Player.Attack.started += _ => StartPressing_AttackButton();
        playerControls.Player.Attack.canceled += StopPressing_AttackButton;

        //todo lay gia tri trong PlayerOCntrols -> SpaceButton
        // playerControls.Player.Jump.started += _ => isJumpButton = true;
        // playerControls.Player.Jump.canceled += _ => isJumpButton = false;

        //todo lay gia tri trong PlayerOCntrols -> LeftShift
        // playerControls.Player.Sprint.performed += _ => isSprintButton = true;
        // playerControls.Player.Sprint.canceled += _ => isSprintButton = false;

        //todo lay gia tri trong PlayerOCntrols -> Ctrl
        // playerControls.Player.SwitchState.started += _ => isSwitchBallButton = true;
        // playerControls.Player.SwitchState.canceled += _ => isSwitchBallButton = false;
    }

    private void OnEnable() {
        playerControls.Player.Move.Enable();
        //playerControls.Player.Look.Enable(); // look around bang onScreen
        playerControls.Player.Aim.Enable();
        playerControls.Player.Attack.Enable();
        //playerControls.Player.Jump.Enable();
        //playerControls.Player.Sprint.Enable();
        //playerControls.Player.SwitchState.Enable();
    }
    private void OnDisable() {
        playerControls.Player.Attack.Disable();
    }

    private void StopPressing_AttackButton(InputAction.CallbackContext context)
    {
        //Debug.Log(context.phase);
        Debug.Log("Stop_Press Attack Button");
        isAttackButton = false;
    }

    private void StartPressing_AttackButton()
    {
        //Debug.Log(context.phase);
        Debug.Log("Start_Press Attack Button");
        isAttackButton = true;
    }

    private void Update() {
        if(SceneManager.GetActiveScene().name == "MainMenu") return;
        if(SceneManager.GetActiveScene().name == "AccountDataOverview") return;
        if(SceneManager.GetActiveScene().name == "Testing_SpawnPlayer") return;

        //? neu dung OnScreenControl (joyStick + WASD) thi DUNG dong nay
        //? neu chi dung UICanvasControllerInput (joyStick) thi KO DUNG dong nay (vi bi xung dot khi Set)
        move = playerControls.Player.Move.ReadValue<Vector2>();
        Magnitude();

        aim = playerControls.Player.Aim.ReadValue<Vector2>();
        //look = playerControls.Player.Look.ReadValue<Vector2>(); // look around bang onScreen
    }
    private void Magnitude() //? dam bao khi hz && vz = 1 => move.x && move.y = 1
    {
        if(move.normalized.x < -0.5f && move.normalized.y < -0.5f) move = new Vector2(-1,-1);
        else if(move.normalized.x < -0.5f && move.normalized.y > 0.5) move = new Vector2(-1,1);
        else if(move.normalized.x > 0.5f && move.normalized.y < -0.5f) move = new Vector2(1,-1);
        else if(move.normalized.x > 0.5f && move.normalized.y > 0.5f) move = new Vector2(1,1);
    }

    //todo End
} 
