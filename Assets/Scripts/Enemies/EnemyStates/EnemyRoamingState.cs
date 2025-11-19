public abstract class EnemyRoamingState : EnemyState
{
    protected EnemyRoamingState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine,
        animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.DetectPlayer())
            stateMachine.ChangeState(enemy.BattleState);
    }
}