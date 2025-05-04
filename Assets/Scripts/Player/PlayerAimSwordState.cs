using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.sword.SetDotVisible(true);
    }

    public override void Update()
    {
        base.Update();
        //瞄准时静止
        player.SetVelocity(0, 0);
        //设置轨迹线
        SkillManager.Instance.sword.SetPos();
        //轨迹线的移动时人物进行翻转
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(dir.x > player.transform.position.x && player.faceDir == -1){
            player.Filp();
        }
        else if(dir.x < player.transform.position.x && player.faceDir == 1){
            player.Filp();
        }
        //瞄准结束
        if (isAnimEnd)
        {
            isAnimEnd = false;
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.Instance.sword.SetDotVisible(false);
    }
}
