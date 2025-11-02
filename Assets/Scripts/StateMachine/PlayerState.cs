using UnityEngine;

public abstract class PlayerState : EntityState
{
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    protected readonly Player player;

    protected PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine,
        animBoolName)
    {
        this.player = player;

        animator = player.Anim;
        rb = player.Rb;
    }


    public override void Update()
    {
        base.Update();

        if (player.ShouldDash && CanDash())
            stateMachine.ChangeState(player.DashState);
    }

    protected override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        animator.SetFloat(YVelocity, rb.linearVelocity.y);
    }

    private bool CanDash()
    {
        if (player.WallDetected)
            return false;
        if (stateMachine.CurrentState == player.DashState)
            return false;

        return true;
    }
}