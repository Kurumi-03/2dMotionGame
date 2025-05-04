using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    bool flag = false;
    public SkeletonIdleState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _skeleton, _boolname)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = skeleton.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (timer >= 0 && timer < skeleton.idleTime / 2 && !flag)
        {
            flag = true;
            skeleton.Filp();
        }
        else if (timer < 0)
        {
            stateMachine.ChangeState(skeleton.skeletonMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        flag = false;
    }
}
