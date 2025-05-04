using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    bool dashEffect;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();
        timer = player.dashTime;
        //冲刺开始时原地释放一个克隆体
        SkillManager.Instance.dash.CanCreateCloneStart();
        dashEffect = false;
    }

    public override void Update()
    {
        base.Update();
        //冲刺时特效并只执行一次
        if(dashEffect == false){
            dashEffect = true;
            player.fx.DashEffect();
        }
        //冲刺可以根据方向键的输入来决定方向并且可以在空中冲刺
        player.SetVelocity(player.dashSpeed * (xInput != 0 ? xInput : player.faceDir), 0);
        if (timer < 0)
        {
            stateMachine.ChangeState(player.CheckIsGround() ? player.idleState : player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //结束时
        SkillManager.Instance.dash.CanCreateCloneEnd();
        player.SetVelocity(0, player.rb.velocity.y);//冲刺退出时x速度归零
    }
}
