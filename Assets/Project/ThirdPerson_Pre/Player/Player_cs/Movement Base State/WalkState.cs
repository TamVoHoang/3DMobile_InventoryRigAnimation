using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(PlayerGun movement) => movement.animator.SetBool("Walking", true);

    public override void UpdateState(PlayerGun movement)
    {
        if (movement.IsSprinting) ExitState(movement, movement.Run);
        //else if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.Crouch);
        else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.currentSpeed = movement.WalkBackSpeed; //movement.currentSpeed = movement.walkBackSpeed
        else movement.currentSpeed = movement.WalkSpeed; //movement.currentSpeed = movement.walkSpeed

        if (InputManager.Instance.IsJumpButton)
        {
            movement.previousState = this;
            ExitState(movement, movement.Jump);
        }
    }

    void ExitState(PlayerGun movement, MovementBaseState state)
    {
        movement.animator.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
