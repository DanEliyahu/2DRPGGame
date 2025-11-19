public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.DisableInput();
        rb.simulated = false;
    }
}