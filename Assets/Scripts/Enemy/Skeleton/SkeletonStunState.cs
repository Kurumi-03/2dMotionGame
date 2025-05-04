using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : EnemyState
{
    EnemySkeleton skeleton;
    public SkeletonStunState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _boolname)
    {
        skeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.fx.CounterAcctackTip();
        skeleton.rb.velocity = new Vector2(skeleton.hitBackPower.x * -skeleton.faceDir,skeleton.hitBackPower.y);
    }

    public override void Update()
    {
        base.Update();
        if (animationTrriger)
        {
            stateMachine.ChangeState(skeleton.skeletonIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.fx.CancelBlind();
    }
}
