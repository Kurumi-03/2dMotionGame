using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * xInput, player.rb.velocity.y);
        }
        if (player.CheckIsGround())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    override public void Exit()
    {
        base.Exit();
    }
}
