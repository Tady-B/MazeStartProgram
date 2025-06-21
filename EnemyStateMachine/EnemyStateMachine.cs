public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }

    public void Initialize(EnemyState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void SwitchState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

   
}
