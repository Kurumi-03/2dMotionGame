using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    float flyTime;//飞起时间
    float flyForce;//飞起的力度
    float fallForce;//下降力度
    float tempGravity;//默认重力
    bool CanUseSkill;
    public PlayerBlackHoleState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.blackHole.SkillData();
        tempGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0;
        timer = flyTime;
        CanUseSkill = false;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            player.rb.velocity = new Vector2(0, flyForce);
        }
        else
        {
            player.rb.velocity = new Vector2(0, -fallForce);
            //技能在状态期间只能使用一次
            if (!CanUseSkill)
            {
                SkillManager.Instance.blackHole.UseSkill();
                CanUseSkill = true;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = tempGravity;
    }

    public void SetUpData(float _flyTime, float _flyForce, float _fallForce)
    {
        flyTime = _flyTime;
        flyForce = _flyForce;
        fallForce = _fallForce;
    }
}
