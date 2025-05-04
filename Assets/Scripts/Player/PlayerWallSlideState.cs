using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        //在墙面时可以跳跃离开
        if (isJumping)
        {
            isJumping = false;
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
        //可以在下滑时加速
        if (yInput < 0)
        {
            player.SetVelocity(0, player.rb.velocity.y);
        }
        else
        {
            player.SetVelocity(0, player.rb.velocity.y * player.slideScale);
        }
        if (player.CheckIsGround())
        {
            stateMachine.ChangeState(player.idleState);
        }
        //并未落地但未检测到墙体时
        else if (!player.CheckIsWall())
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (xInput != 0 && xInput * player.faceDir == -1)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
