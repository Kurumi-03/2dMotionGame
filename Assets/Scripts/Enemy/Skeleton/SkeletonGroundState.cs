using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected EnemySkeleton skeleton;
    Transform player;

    public SkeletonGroundState(EnemyStateMachine _stateMachine, Enemy _enemy, EnemySkeleton _skeleton, string _boolname) : base(_stateMachine, _enemy, _boolname)
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
        if (skeleton.CheckIsPlayer() || Vector3.Distance(skeleton.transform.position, player.position) < skeleton.battleDistance)
        {
            stateMachine.ChangeState(skeleton.skeletonBattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
