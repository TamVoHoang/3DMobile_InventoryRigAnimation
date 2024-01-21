
public class RunState : MovementBaseState
{
    public override void EnterState(PlayerController movement) => movement.animator.SetBool("Running", true);

    public override void UpdateState(PlayerController movement)
    {
        if (!InputManager.Instance.GetSprintButton) ExitState(movement, movement.Walk);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.CurrentSpeed(movement.RunBackSpeed); // movement.currentSpeed = movement.runBackSpeed
        else movement.CurrentSpeed(movement.RunSpeed); // movement.currentSpeed = movement.runSpeed
        
        if (InputManager.Instance.GetJumpButton)
        {
            movement.previousState = this;
            ExitState(movement, movement.Jump);
        }
    }

    void ExitState(PlayerController movement, MovementBaseState state)
    {
        movement.animator.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
