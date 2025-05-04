using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    float flipTimer;

    public SkeletonMoveState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _skeleton, _boolname)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        skeleton.SetVelocity(skeleton.moveSpeed * skeleton.faceDir, skeleton.rb.velocity.y);
        if (!skeleton.CheckIsGround() || skeleton.CheckIsWall())
        {
            stateMachine.ChangeState(skeleton.skeletonIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
