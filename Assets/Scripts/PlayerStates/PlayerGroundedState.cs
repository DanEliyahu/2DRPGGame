public abstract class PlayerGroundedState : EntityState
{
    protected PlayerGroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGrounded && rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.FallState);

        if (player.ShouldJump)
            stateMachine.ChangeState(player.JumpState);

        if (player.ShouldAttack)
            stateMachine.ChangeState(player.BasicAttackState);
    }
}