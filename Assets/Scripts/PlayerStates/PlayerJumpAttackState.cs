using UnityEngine;

public class PlayerJumpAttackState : EntityState
{
    private static readonly int JumpAttackTrigger = Animator.StringToHash("JumpAttackTrigger");

    private bool touchedGround;

    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        touchedGround = false;
        //player.SetVelocity(player.jumpAttackVelocity.x * player.FacingDirection, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGrounded && !touchedGround)
        {
            touchedGround = true;
            animator.SetTrigger(JumpAttackTrigger);
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}