using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{

    [Header("Output")]
    [SerializeField] private InputManager inputs;

    private void Start() {
        inputs = InputManager.Instance;
    }

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        inputs.SetMove(Vector2.ClampMagnitude(virtualMoveDirection, 1));
        //inputs.SetMove(virtualMoveDirection); // khi chua Clamp gia gia tri neu di xeo se bi over 1
    }
    
    public void VirtualAimInput(Vector2 virtualLookDirection)
    {
        inputs.SetAim(Vector2.ClampMagnitude(virtualLookDirection, 1));
    }

    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
        Debug.Log("co dang set virtual look input");
        inputs.SetLook(Vector2.ClampMagnitude(virtualLookDirection, 1));

    }
    public void VirtualAttackInput(bool virtualAttackState)
    {
        Debug.Log("co set attack button" + virtualAttackState);
        inputs.SetIsAttackButton(virtualAttackState);
    }

    public void VirtualJumpInput(bool virtualJumpState)
    {
        inputs.SetIsJumpButton(virtualJumpState);
    }

    public void VirtualSprintInput(bool virtualSprintState)
    {
        Debug.Log("co nhan sprint button" + virtualSprintState);
        inputs.SetIsSprintButton(virtualSprintState);
    }

    public void VirtualSwitchBallInput(bool virtualSwitchState)
    {
        inputs.SetIsSwitchStateButton(virtualSwitchState);
    }
}