public class PlayerDashState : PlayerState
{
    private float originalGravityScale;
    private int dashDirection;

    public PlayerDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dashDirection = (int)(player.MoveInput.x != 0 ? player.MoveInput.x : player.FacingDirection);
        stateTimer = player.dashDuration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        CancelDashIfNeeded();

        player.SetVelocity(player.dashSpeed * dashDirection, 0);

        if (stateTimer < 0)
        {
            if (player.IsGrounded)
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.FallState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDashIfNeeded()
    {
        if (player.WallDetected)
        {
            if (player.IsGrounded)
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.WallSlideState);
        }
    }
}