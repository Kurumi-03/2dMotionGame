using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    float attackTimer;
    float attackDir = 1;//攻击方向
    public PlayerPrimaryAttack(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();
        //攻击时转换方向
        attackDir = xInput != 0 ? xInput : player.faceDir;
        //攻击反馈
        player.SetVelocity(player.attackMovement[(int)combo].x * attackDir, player.attackMovement[(int)combo].y);
        timer = 0.1f;
        //计时结束
        if (attackTimer + player.attackCollDown < Time.time)
        {
            combo = 0;
        }
        player.anim.speed = player.attackSpeed;//设置动画速度即攻击速度
        player.attackCombo = (int)combo;
    }

    public override void Update()
    {
        base.Update();
        //使攻击反馈能够生效
        if (timer < 0)
        {
            player.SetVelocity(0, 0);
        }
        if (animationTrriger)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    override public void Exit()
    {
        base.Exit();
        attackTimer = Time.time;//从这里计时
        combo = (combo + 1) % 3;
        player.anim.speed = 1;//重置动画速度
    }
}
