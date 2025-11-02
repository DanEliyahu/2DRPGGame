using UnityEngine;

public abstract class EnemyState : EntityState
{
    private static readonly int MoveAnimSpeedMultiplier = Animator.StringToHash("MoveAnimSpeedMultiplier");
    private static readonly int XVelocity = Animator.StringToHash("XVelocity");
    private static readonly int BattleAnimSpeedMultiplier = Animator.StringToHash("BattleAnimSpeedMultiplier");
    protected readonly Enemy enemy;

    protected EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        animator = enemy.Anim;
        rb = enemy.Rb;
    }

    protected override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        
        var battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;

        animator.SetFloat(MoveAnimSpeedMultiplier, enemy.moveAnimSpeedMultiplier);
        animator.SetFloat(XVelocity, rb.linearVelocity.x);
        animator.SetFloat(BattleAnimSpeedMultiplier, battleAnimSpeedMultiplier);
    }
}