using UnityEngine;

public class PlayerBasicAttackState : PlayerState
{
    private static readonly int BasicAttackIndex = Animator.StringToHash("BasicAttackIndex");
    private const int FirstComboIndex = 1; // Combo starts with 1, this is used in the Animator.

    private int attackDirection;
    private float attackVelocityTimer;
    private int comboIndex = 1;
    private readonly int comboLimit;
    private float lastTimeAttacked;
    private bool comboAttackQueued;
    private float originalGravityScale;

    public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
        comboLimit = player.attackVelocity.Length;
    }

    public override void Enter()
    {
        base.Enter();

        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        // Define attack direction according to input
        attackDirection = (int)(player.MoveInput.x != 0 ? player.MoveInput.x : player.FacingDirection);

        animator.SetInteger(BasicAttackIndex, comboIndex);
        ApplyAttackVelocity();

        if (!player.IsGrounded)
        {
            originalGravityScale = rb.gravityScale;
            rb.gravityScale = 0;
        }
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        // detect and handle damaging enemies

        if (player.ShouldAttack)
            QueueNextAttack();

        if (triggerCalled)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;
        lastTimeAttacked = Time.time;
        if (originalGravityScale != 0)
            rb.gravityScale = originalGravityScale;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
            stateMachine.ChangeState(player.BasicAttackState);
        else
            stateMachine.ChangeState(player.IdleState);
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, 0);
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = FirstComboIndex;

        if (comboIndex > comboLimit)
            comboIndex = FirstComboIndex;
    }
}