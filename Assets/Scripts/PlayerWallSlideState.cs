public class PlayerWallSlideState : EntityState
{
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleWallSlide();

        if (player.ShouldJump)
            stateMachine.ChangeState(player.WallJumpState);


        if (!player.WallDetected)
            stateMachine.ChangeState(player.FallState);

        if (player.IsGrounded)
        {
            player.Flip();
            stateMachine.ChangeState(player.IdleState);
        }
    }

    private void HandleWallSlide()
    {
        if (player.MoveInput.y < 0)
            player.SetVelocity(player.MoveInput.x, rb.linearVelocity.y);
        else
            player.SetVelocity(player.MoveInput.x, rb.linearVelocity.y * player.slideSpeedMultiplier);
    }
}