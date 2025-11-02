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

    [Header("Player Detection")] [SerializeField]
    private LayerMask collisionLayers;

    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10f;

    public Transform Player { get; private set; }

    public EnemyIdleState IdleState { get; protected set; }
    public EnemyMoveState MoveState { get; protected set; }
    public EnemyAttackState AttackState { get; protected set; }
    public EnemyBattleState BattleState { get; protected set; }

    public RaycastHit2D DetectPlayer()
    {
        var hit = Physics2D.Raycast(playerCheck.position, Vector2.right * FacingDirection, playerCheckDistance,
            collisionLayers);

        if (!hit.collider || !hit.collider.CompareTag("Player"))
            return default;

        Player = hit.collider.transform;
        return hit;
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