

using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {

    }

    override public void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (isCounterAttack)
        {
            isCounterAttack = false;
            stateMachine.ChangeState(player.counterAttackState);
        }
        if (isAttacking)
        {
            isAttacking = false;
            stateMachine.ChangeState(player.primaryAttackState);
        }
        if (isJumping && player.CheckIsGround())
        {
            isJumping = false;
            stateMachine.ChangeState(player.jumpState);
        }
        if (!player.CheckIsGround())
        {
            stateMachine.ChangeState(player.fallState);
        }

    }

    override public void Exit()
    {
        base.Exit();
    }
}
