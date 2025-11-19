using System;

public class StateMachine
{
    public EntityState CurrentState { get; private set; }
    private bool canChangeState = true;

    private StateSwitchedException stateSwitchedException;

    public void Initialize(EntityState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
        stateSwitchedException = new StateSwitchedException();
    }

    public void ChangeState(EntityState newState)
    {
        if (!canChangeState)
            return;

        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
        throw stateSwitchedException;
    }

    public void Update()
    {
        try
        {
            CurrentState.Update();
        }
        catch (Exception e)
        {
            if (e is not StateSwitchedException) throw;
        }
    }

    public void SwitchOffStateMachine() => canChangeState = false;
}