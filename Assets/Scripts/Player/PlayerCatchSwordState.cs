using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.velocity = new Vector2(SkillManager.Instance.sword.SetReturnForce() * -player.faceDir, player.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (animationTrriger)
        {
            //只有在动画完成后才可以继续投掷
            SkillManager.Instance.sword.canThrow = true;
            stateMachine.ChangeState(player.idleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
