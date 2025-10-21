public class PlayerWallJumpState : EntityState
{
    public PlayerWallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        float jumpDirection = -player.FacingDirection; // Jump to left of player (Local left)
        player.SetVelocity(player.wallJumpForce.x * jumpDirection, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.FallState);
        if (player.WallDetected)
            stateMachine.ChangeState(player.WallSlideState);
    }
}