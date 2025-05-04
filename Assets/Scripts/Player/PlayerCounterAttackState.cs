using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    bool counterAttacking;//用于标记
    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = player.attackCounterDuring;
        player.anim.SetBool("counterAttackSuccess", false);
        counterAttacking = true;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        //反击时间结束或是成功反击完成后退至idle
        if (timer < 0 || animationTrriger)
        {
            stateMachine.ChangeState(player.idleState);
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius[0]);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().IsStuning())
                {
                    timer = player.attackCounterTime;
                    player.anim.SetBool("counterAttackSuccess", true);
                    //反击技能升级后的治疗效果
                    SkillManager.Instance.parry.UseSkill();
                    //一次反击无论攻击到多少敌人都只创建一个克隆体 
                    if (counterAttacking && SkillManager.Instance.parry.parryCloneUnlock)
                    {
                        counterAttacking = false;
                        SkillManager.Instance.parry.ParryCreateClone(hit.transform);
                    }
                }

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
