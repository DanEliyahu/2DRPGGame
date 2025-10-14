public class PlayerFallState : PlayerAiredState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGrounded)
            stateMachine.ChangeState(player.IdleState);

        if (player.WallDetected)
            stateMachine.ChangeState(player.WallSlideState);
    }
}