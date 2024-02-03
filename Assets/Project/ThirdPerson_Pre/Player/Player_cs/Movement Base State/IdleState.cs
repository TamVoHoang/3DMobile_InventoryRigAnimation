using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(PlayerGun movement) {}

    public override void UpdateState(PlayerGun movement)
    {
        if (movement.dir.magnitude > 0.1f)
        {
            // if (InputManager.Instance.IsJumpButton) movement.SwitchState(movement.Run);
            // else movement.SwitchState(movement.Walk);

            movement.SwitchState(movement.Walk);
        }

        if (movement.IsSprinting) movement.SwitchState(movement.Run);
        //if (Input.GetKeyDown(KeyCode.C)) movement.SwitchState(movement.Crouch);

        if (InputManager.Instance.IsJumpButton && movement.dir.magnitude == 0f) // THEM VAO DE DAM BAO DUNG YEN THI SE IDLE
        {
            movement.previousState = this;
            movement.SwitchState(movement.Jump);
        }
    }
}
