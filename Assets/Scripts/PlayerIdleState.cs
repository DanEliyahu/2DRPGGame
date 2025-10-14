using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (Mathf.Approximately(player.MoveInput.x, player.FacingDirection) && player.WallDetected)
            return;

        if (player.MoveInput.x != 0)
            stateMachine.ChangeState(player.MoveState);
    }
}