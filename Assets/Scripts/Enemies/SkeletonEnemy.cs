using System;

public class SkeletonEnemy : Enemy, ICounterable
{
    protected override void Awake()
    {
        base.Awake();

        IdleState = new EnemyIdleState(this, stateMachine, "Idle");
        MoveState = new EnemyMoveState(this, stateMachine, "Move");
        AttackState = new EnemyAttackState(this, stateMachine, "Attack");
        BattleState = new EnemyBattleState(this, stateMachine, "Battle");
        DeadState = new EnemyDeadState(this, stateMachine, "Dead");
        StunnedState = new EnemyStunnedState(this, stateMachine, "Stunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }

    public bool CanBeCountered => canBeStunned;

    public void HandleCounter()
    {
        if (!CanBeCountered)
            return;

        try
        {
            stateMachine.ChangeState(StunnedState);
        }
        catch (Exception e)
        {
            if (e is not StateSwitchedException) throw;
        }
    }
}