
public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string stateBoolName) : base(player, stateMachine, stateBoolName)
    {

    }

    override public void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
    }

    override public void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    override public void Exit()
    {
        base.Exit();
    }
}
