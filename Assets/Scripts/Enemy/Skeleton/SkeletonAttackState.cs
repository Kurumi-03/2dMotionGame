using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    EnemySkeleton skeleton;
    public SkeletonAttackState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _boolname)
    {
        skeleton = _skeleton;
    }
    public override void Enter()
    {
        base.Enter();
        skeleton.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();
        if(animationTrriger)
        {
            stateMachine.ChangeState(skeleton.skeletonBattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        lastAttackTime = Time.time;
    }
}
