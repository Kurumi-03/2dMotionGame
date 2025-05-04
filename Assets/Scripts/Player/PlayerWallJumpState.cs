using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();

        player.SetVelocity(player.moveSpeed * player.faceDir * -1, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if (player.rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        
    }

    override public void Exit()
    {
        base.Exit();
    }
}
