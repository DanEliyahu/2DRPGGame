using System;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Movement Details")] public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)] public float moveAnimSpeedMultiplier = 1f;

    [Header("Battle Details")] public float battleMoveSpeed = 3f;
    public float attackDistance = 2f;
    public float battleTimeDuration = 5f;
    public float minRetreatDistance = 1f;
    public Vector2 retreatVelocity;

    [Header("Stunned State Details")] public float stunnedDuration = 1;
    public Vector2 stunnedVelocity = new(7, 7);
    protected bool canBeStunned;

    [Header("Player Detection")] [SerializeField]
    private LayerMask collisionLayers;

    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10f;

    public Transform PlayerTransform { get; private set; }

    public EnemyIdleState IdleState { get; protected set; }
    public EnemyMoveState MoveState { get; protected set; }
    public EnemyAttackState AttackState { get; protected set; }
    public EnemyBattleState BattleState { get; protected set; }
    protected EnemyDeadState DeadState { get; set; }
    protected EnemyStunnedState StunnedState { get; set; }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    public void EnableCounterWindow(bool enable) => canBeStunned = enable;

    public RaycastHit2D DetectPlayer()
    {
        var hit = Physics2D.Raycast(playerCheck.position, Vector2.right * FacingDirection, playerCheckDistance,
            collisionLayers);

        if (!hit.collider || !hit.collider.CompareTag("Player"))
            return default;

        PlayerTransform = hit.collider.transform;
        return hit;
    }

    private void TryEnteringBattleState(Transform player)
    {
        if (stateMachine.CurrentState == BattleState || stateMachine.CurrentState == AttackState)
            return;

        PlayerTransform = player;
        try
        {
            stateMachine.ChangeState(BattleState);
        }
        catch (Exception e)
        {
            if (e is not StateSwitchedException) throw;
        }
    }

    public override void TakeDamage(float amount, Transform damageDealer)
    {
        base.TakeDamage(amount, damageDealer);

        if (isDead)
            return;

        if (damageDealer.CompareTag("Player"))
            TryEnteringBattleState(damageDealer);
    }

    protected override void Die()
    {
        base.Die();

        try
        {
            stateMachine.ChangeState(DeadState);
        }
        catch (Exception e)
        {
            if (e is not StateSwitchedException) throw;
        }
    }

    private void HandlePlayerDeath()
    {
        try
        {
            stateMachine.ChangeState(IdleState);
        }
        catch (Exception e)
        {
            if (e is not StateSwitchedException) throw;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position,
            playerCheck.position + Vector3.right * (FacingDirection * playerCheckDistance));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position,
            playerCheck.position + Vector3.right * (FacingDirection * attackDistance));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position,
            playerCheck.position + Vector3.right * (FacingDirection * minRetreatDistance));
    }
}