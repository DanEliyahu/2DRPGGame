public class PlayerAiredState : EntityState
{
    protected PlayerAiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x != 0)
            player.SetVelocity(player.MoveInput.x * player.moveSpeed * player.airSpeedMultiplier, rb.linearVelocity.y);

        if (player.ShouldAttack)
        {
            if (player.MoveInput.y < 0)
                stateMachine.ChangeState(player.JumpAttackState);
            else
                stateMachine.ChangeState(player.BasicAttackState);
        }
    }
}