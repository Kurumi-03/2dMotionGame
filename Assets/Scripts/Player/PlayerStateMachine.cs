
public class PlayerStateMachine
{
    public PlayerState currentState{ get; private set; }
    //初始化状态
    public void Initialize(PlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState playerState)
    {
        currentState.Exit();
        currentState = playerState;
        currentState.Enter();
    }
}
