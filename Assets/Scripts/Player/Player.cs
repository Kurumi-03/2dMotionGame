using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Player : Entity
{

    public PlayerStateMachine stateMachine { get; private set; }//状态机不能被外部修改

    //状态不能被外部修改
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttack primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDieState dieState { get; private set; }

    public float jumpForce = 15f;
    public float dashSpeed = 25f;
    public float dashTime = 0.2f;
    public float slideScale = 0.7f;
    public float attackCollDown = 1f;
    public Vector2[] attackMovement = { new Vector2(5, 3), new Vector2(1, 1), new Vector2(10, 3) };
    public float attackSpeed = 1f;
    public float attackCounterDuring = 0.2f;//反击持续时间
    public float attackCounterTime = 5f;//反击间隔时间，至少要能完整播放完反击成功动画

    float defaultMoveSpeed;
    float defaultJumpForce;
    float defaultDashSpeed;
    float defaultAttackSpeed;

    public GameObject hpUI;//player的血量ui


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "move");
        jumpState = new PlayerJumpState(this, stateMachine, "jump");
        fallState = new PlayerFallState(this, stateMachine, "jump");
        dashState = new PlayerDashState(this, stateMachine, "dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "jump");

        primaryAttackState = new PlayerPrimaryAttack(this, stateMachine, "attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "counterAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "aim");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "catch");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "jump");

        dieState = new PlayerDieState(this, stateMachine, "die");
    }

    protected override void Start()
    {
        base.Start();
        moveSpeed = 8f;
        groundCheckDistance = 1.4f;
        wallCheckDistance = 0.52f;
        fx.fxDuring = 0.15f;
        hitBackTime = 0;
        hitBackPower = new Vector2(0, 0);
        gameObject.layer = LayerMask.NameToLayer("Player");
        stateMachine.Initialize(idleState);

        //设置默认值
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
        defaultAttackSpeed = attackSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }


    public void CallTrriger()
    {
        stateMachine.currentState.OnAnimationEnd();
    }


    public override void SlowAction(float slowTime, float slowPercent)
    {
        base.SlowAction(slowTime, slowPercent);
        moveSpeed *= 1 - slowPercent;
        jumpForce *= 1 - slowPercent;
        dashSpeed *= 1 - slowPercent;
        attackSpeed *= 1 - slowPercent;
    }

    public override void RecoverAction()
    {
        base.RecoverAction();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
        attackSpeed = defaultAttackSpeed;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieState);
    }
}


