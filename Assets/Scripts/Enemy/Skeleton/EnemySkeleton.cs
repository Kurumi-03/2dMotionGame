using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    // Start is called before the first frame update
    public SkeletonIdleState skeletonIdleState { get; private set; }
    public SkeletonMoveState skeletonMoveState { get; private set; }
    public SkeletonBattleState skeletonBattleState { get; private set; }
    public SkeletonAttackState skeletonAttackState { get; private set; }
    public SkeletonStunState skeletonStunState { get; private set; }
    public SkeletonDieState skeletonDieState { get; private set; }

    public float idleTime = 2f;//待机时间  最好设置一个大一点的值
    public float attackDistance = 1.5f;//攻击距离
    public float attackCoolDown = 2.5f;//攻击cd
    public float battleTime = 4f;//追击时间  超过后不再追击
    public float battleDistance = 3f;//追击距离 使敌人能够从背后发现玩家
    public float battleOffset = 1.5f;//敌人寻敌向左向右的偏移，避免重叠时抖动

    public float dieUpTime = 0.2f;//死亡后会向上飞起的时间
    public Vector2 dieDir = new Vector2(0, 10);//死亡向上飞起的力度

    protected override void Awake()
    {
        base.Awake();
        skeletonIdleState = new SkeletonIdleState(stateMachine, this, this, "idle");
        skeletonMoveState = new SkeletonMoveState(stateMachine, this, this, "move");
        skeletonBattleState = new SkeletonBattleState(stateMachine, this, this, "move");
        skeletonAttackState = new SkeletonAttackState(stateMachine, this, this, "attack");
        skeletonStunState = new SkeletonStunState(stateMachine, this, this, "stun");

        skeletonDieState = new SkeletonDieState(stateMachine, this, this, "stun");
    }

    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        groundCheckDistance = 0.4f;
        wallCheckDistance = 1f;
        playerDistance = 20f;
        fx.fxDuring = 0.2f;
        hitBackTime = 0.07f;
        hitBackPower = new Vector2(7, 12);
        stateMachine.Initialize(skeletonIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    //更改状态
    public override bool IsStuning()
    {
        if (base.IsStuning())
        {
            stateMachine.ChangeState(skeletonStunState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(skeletonDieState);
    }
}
