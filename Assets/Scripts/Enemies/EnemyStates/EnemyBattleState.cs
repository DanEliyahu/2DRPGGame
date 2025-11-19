using UnityEngine;

public class EnemyBattleState : EnemyState
{
    private Transform player;
    private float lastBattleTime;

    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player = enemy.PlayerTransform;
        UpdateBattleTimer();

        if (ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.DetectPlayer())
            UpdateBattleTimer();

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.IdleState);

        if (WithinAttackRange() && enemy.DetectPlayer())
            stateMachine.ChangeState(enemy.AttackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
    }

    private void UpdateBattleTimer() => lastBattleTime = Time.time;

    private bool BattleTimeIsOver() => Time.time > lastBattleTime + enemy.battleTimeDuration;

    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    private float DistanceToPlayer()
    {
        if (!player)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (!player)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}