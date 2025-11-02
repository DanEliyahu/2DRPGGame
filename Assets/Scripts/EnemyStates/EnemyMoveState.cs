public class EnemyMoveState : EnemyRoamingState
{
    public EnemyMoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!enemy.IsGrounded || enemy.WallDetected)
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.FacingDirection, rb.linearVelocity.y);
        if (!enemy.IsGrounded || enemy.WallDetected)
            stateMachine.ChangeState(enemy.IdleState);
    }
}