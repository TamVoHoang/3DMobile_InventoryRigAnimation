
public class RunState : MovementBaseState
{
    public override void EnterState(PlayerGun movement) => movement.animator.SetBool("Running", true);

    public override void UpdateState(PlayerGun movement)
    {
        if (!movement.IsSprinting) ExitState(movement, movement.Walk);
        else if (movement.dir.magnitude < 0.1f && !movement.IsSprinting) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.CurrentSpeed(movement.RunBackSpeed); // movement.currentSpeed = movement.runBackSpeed
        else movement.CurrentSpeed(movement.RunSpeed); // movement.currentSpeed = movement.runSpeed
        
        if (InputManager.Instance.IsJumpButton)
        {
            movement.previousState = this;
            ExitState(movement, movement.Jump);
        }
    }

    void ExitState(PlayerGun movement, MovementBaseState state)
    {
        movement.animator.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
