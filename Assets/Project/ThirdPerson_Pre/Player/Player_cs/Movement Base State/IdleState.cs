using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(PlayerController movement) {}

    public override void UpdateState(PlayerController movement)
    {
        if (movement.dir.magnitude > 0.1f)
        {
            if (InputManager.Instance.GetJumpButton) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
        //if (Input.GetKeyDown(KeyCode.C)) movement.SwitchState(movement.Crouch);
        if (InputManager.Instance.GetJumpButton && movement.dir.magnitude == 0f) // THEM VAO DE DAM BAO DUNG YEN THI SE IDLE
        {
            movement.previousState = this;
            movement.SwitchState(movement.Jump);
        }
    }
}
