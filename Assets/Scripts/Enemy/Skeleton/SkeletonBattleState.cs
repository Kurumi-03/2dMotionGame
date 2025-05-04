using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    EnemySkeleton skeleton;
    Transform player;
    int moveDir;
    float moveTimer;
    public SkeletonBattleState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _boolname)
    {
        skeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        //在玩家进入攻击范围后停止移动并攻击
        if (skeleton.CheckIsPlayer())
        {
            if (skeleton.CheckIsPlayer().distance < skeleton.attackDistance && skeleton.CheckIsPlayer().distance > 0)
            {
                timer = skeleton.battleTime;
                if (CheckAttack())
                {
                    stateMachine.ChangeState(skeleton.skeletonAttackState);
                    return;
                }
            }
        }
        else
        {
            if (timer < 0)
            {
                stateMachine.ChangeState(skeleton.skeletonIdleState);
            }
        }
        //加1减1是为了解决重叠时敌人持续左右寻敌时抖动的问题
        if (skeleton.transform.position.x < player.position.x - skeleton.battleOffset)
        {
            moveDir = 1;
        }
        else if (skeleton.transform.position.x > player.position.x + skeleton.battleOffset)
        {
            moveDir = -1;
        }
        skeleton.SetVelocity(skeleton.moveSpeed * moveDir, skeleton.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    bool CheckAttack()
    {
        if (lastAttackTime + skeleton.attackCoolDown < Time.time)
        {
            lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
}
