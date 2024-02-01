using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PlayerControls playerControls;

    private bool isAttackButton;
    [SerializeField] private Vector2 move;//ok
    private Vector2 look;
    [SerializeField] private Vector2 aim;
    private bool isSwitchBallButton;
    [SerializeField] private bool isSprintButton;
    private bool isJumpButton;

    public bool GetAttackButton {get{return isAttackButton;} private set{isAttackButton = value;}}
    public bool GetSwitchStateButton {get{return isSwitchBallButton;} private set{isSwitchBallButton = value;}}

    public bool GetSprintButton {get{return isSprintButton;} private set{isSprintButton = value;}}
    public bool GetJumpButton {get{return isJumpButton;} private set{isJumpButton = value;}}
    
    public Vector2 GetMove {get{return move;} private set{move = value;}}
    public void SetMove(Vector2 move) => this.move = move;

    public Vector2 GetLook {get{return look;} private set{look = value;}}
    public void SetLook(Vector2 look) => this.look = look;

    public Vector2 GetAim {get{return aim;} private set{aim = value;}}
    public void SetAim(Vector2 aim) => this.aim = aim;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
        
        // playerControls.Player.Attack.started += _ => StartAttacking();
        // playerControls.Player.Attack.canceled += StopAttacking;

        //todo lay gia tri trong PlayerOCntrols -> Ctrl
        // playerControls.Player.SwitchState.started += _ => isSwitchBallButton = true;
        // playerControls.Player.SwitchState.canceled += _ => isSwitchBallButton = false;

        //todo lay gia tri trong PlayerOCntrols -> LeftShift
        // playerControls.Player.Sprint.performed += _ => isSprintButton = true;
        // playerControls.Player.Sprint.canceled += _ => isSprintButton = false;

        //todo lay gia tri trong PlayerOCntrols -> SpaceButton
        // playerControls.Player.Jump.started += _ => isJumpButton = true;
        // playerControls.Player.Jump.canceled += _ => isJumpButton = false;
    }

    private void OnEnable() {
        playerControls.Player.Move.Enable();
        //playerControls.Player.Look.Enable();
        //playerControls.Player.Aim.Enable();
        // playerControls.Player.SwitchState.Enable();
        //playerControls.Player.Sprint.Enable();
        //playerControls.Player.Jump.Enable();
        // playerControls.Player.Attack.Enable();
    }

    private void StopAttacking(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        isAttackButton = false;
    }

    private void StartAttacking()
    {
        //Debug.Log(context.phase);
        Debug.Log("Start attacking");
        isAttackButton = true;
    }

    private void Update() {
        //? neu dung OnScreenControl (joyStick + WASD) thi DUNG dong nay
        //? neu chi dung UICanvasControllerInput (joyStick) thi KO DUNG dong nay (vi bi xung dot khi Set)
        move = playerControls.Player.Move.ReadValue<Vector2>();
        if(move.normalized.x < -0.5f && move.normalized.y < -0.5f) move = new Vector2(-1,-1);
        else if(move.normalized.x < -0.5f && move.normalized.y > 0.5) move = new Vector2(-1,1);
        else if(move.normalized.x > 0.5f && move.normalized.y < -0.5f) move = new Vector2(1,-1);
        else if(move.normalized.x > 0.5f && move.normalized.y > 0.5f) move = new Vector2(1,1);

        //aim = playerControls.Player.Aim.ReadValue<Vector2>();
        //look = playerControls.Player.Look.ReadValue<Vector2>();
    }
    
    public void SetAttackButton(bool isAttackButton) {
        this.isAttackButton = isAttackButton;
    }
    public void SetSwitchStateButton(bool isSwitchStateButton) {
        this.isSwitchBallButton = isSwitchStateButton;
    }

    public void SetSprintButton(bool isSprintButton) {
        this.isSprintButton = isSprintButton;
    }
    public void SetJumpButton(bool isJumpButton) {
        this.isJumpButton = isJumpButton;
    }

    //todo End
}
