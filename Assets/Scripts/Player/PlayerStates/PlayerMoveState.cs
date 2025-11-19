public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.MoveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x == 0 || player.WallDetected)
            stateMachine.ChangeState(player.IdleState);

        player.SetVelocity(player.MoveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}