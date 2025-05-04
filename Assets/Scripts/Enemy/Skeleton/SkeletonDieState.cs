using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDieState : EnemyState
{
    EnemySkeleton skeleton;
    public SkeletonDieState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _boolname)
    {
        skeleton = _skeleton;
    }
    public override void Enter()
    {
        base.Enter();
        //死亡时停止x方向上的速度
        skeleton.SetVelocity(0, skeleton.rb.velocity.y);
        timer = skeleton.dieUpTime;
        //死亡时受到的效果不会再执行buff的特殊效果
        skeleton.RecoverAction();
        skeleton.fx.CancelBlind();
    }

    public override void Update()
    {
        base.Update();
        if (timer > 0)
        {
            skeleton.rb.velocity = skeleton.dieDir;
        }
        if(animationTrriger)
        {
            skeleton.cd.enabled = false;//避免在发生碰撞  并使怪死亡时掉落
        }
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.anim.speed = 0;
    }
}
