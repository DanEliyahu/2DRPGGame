public class SkeletonEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        IdleState = new EnemyIdleState(this, stateMachine, "Idle");
        MoveState = new EnemyMoveState(this, stateMachine, "Move");
        AttackState = new EnemyAttackState(this, stateMachine, "Attack");
        BattleState = new EnemyBattleState(this, stateMachine, "Battle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }
}