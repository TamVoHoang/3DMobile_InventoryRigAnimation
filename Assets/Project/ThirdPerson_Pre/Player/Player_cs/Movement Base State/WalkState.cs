using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(PlayerController movement) => movement.animator.SetBool("Walking", true);

    public override void UpdateState(PlayerController movement)
    {
        if (InputManager.Instance.GetSprintButton) ExitState(movement, movement.Run);
        //else if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.Crouch);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.currentSpeed = movement.walkBackSpeed; //movement.currentSpeed = movement.walkBackSpeed
        else movement.currentSpeed = movement.walkSpeed; //movement.currentSpeed = movement.walkSpeed
        if (InputManager.Instance.GetJumpButton)
        {
            movement.previousState = this;
            ExitState(movement, movement.Jump);
        }
    }

    void ExitState(PlayerController movement, MovementBaseState state)
    {
        movement.animator.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
