using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(PlayerGun movement)
    {
        if (movement.previousState == movement.Idle) movement.animator.SetTrigger("IdleJump");
        else if (movement.previousState == movement.Walk || movement.previousState == movement.Run) 
                movement.animator.SetTrigger("RunJump");
    }

    public override void UpdateState(PlayerGun movement)
    {
        if(movement.isJumped && movement.SetIsGrounded())
        {
            movement.SetIsJumped(false);
            if (movement.hzInput == 0 && movement.vInput == 0) movement.SwitchState(movement.Idle);
            else if (movement.IsSprinting) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
    }
}
