using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator anim;

    [Header("Health")] [SerializeField] private float maxHealth = 100;
    [SerializeField] private Vector2 knockbackPower = new(1.5f, 2.5f);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] [Range(0, 1)] private float heavyDamageThreshold = 0.3f;
    [SerializeField] private Vector2 heavyKnockbackPower = new(7, 7);
    [SerializeField] private float heavyKnockbackDuration = 0.5f;
    private float currentHealth;
    protected bool isDead;
    private bool isKnockedBack;
    private Coroutine knockBackCoroutine;
    private Slider healthBar;

    [Header("Damage")] [SerializeField] private float damage = 10;

    [Header("Collision Detection")] [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;

    public bool IsGrounded { get; private set; }
    public bool WallDetected { get; private set; }

    [Header("Target Detection")] [SerializeField]
    private Transform targetCheck;

    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask targetLayer;

    public int FacingDirection => (int)transform.right.x;
    private bool isFacingRight = true;

    protected StateMachine stateMachine;
    public Animator Anim => anim;
    public Rigidbody2D Rb { get; private set; }
    private EntityVFX entityVFX;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        entityVFX = GetComponentInChildren<EntityVFX>();
        healthBar = GetComponentInChildren<Slider>();
        stateMachine = new StateMachine();
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.Update();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.CurrentState.AnimationTrigger();
    }

    private void ReceiveKnockBack(Vector2 knockback, float duration)
    {
        if (knockBackCoroutine != null)
            StopCoroutine(knockBackCoroutine);

        knockBackCoroutine = StartCoroutine(KnockBackCoroutine(knockback, duration));
    }

    private IEnumerator KnockBackCoroutine(Vector2 knockback, float duration)
    {
        isKnockedBack = true;
        Rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        Rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnockedBack)
            return;

        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        switch (xVelocity)
        {
            case > 0 when !isFacingRight:
            case < 0 when isFacingRight:
                Flip();
                break;
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        HandleHealthBarFlip();
    }

    private void HandleHealthBarFlip()
    {
        if (!healthBar)
            return;
        healthBar.transform.rotation = Quaternion.identity;
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            var damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            damageable.TakeDamage(damage, transform);
            entityVFX.CreateOnHitVFX(target.transform);
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        // ReSharper disable once Unity.PreferNonAllocApi
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, targetLayer);
    }

    public virtual void TakeDamage(float amount, Transform damageDealer)
    {
        if (isDead)
            return;

        var knockback = CalculateKnockback(amount, damageDealer);
        var duration = KnockbackDuration(amount);

        ReceiveKnockBack(knockback, duration);
        entityVFX?.OnDamageVFX();
        ReduceHealth(amount);
    }

    private void ReduceHealth(float amount)
    {
        currentHealth -= amount;
        UpdateHealthBar();
        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (!healthBar)
            return;
        healthBar.value = currentHealth / maxHealth;
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    private Vector2 CalculateKnockback(float damageAmount, Transform damageDealer)
    {
        var direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        var knockback = IsHeavyDamage(damageAmount) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;

        return knockback;
    }

    private float KnockbackDuration(float damageAmount) =>
        IsHeavyDamage(damageAmount) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damageAmount) => damageAmount / maxHealth > heavyDamageThreshold;

    private void HandleCollisionDetection()
    {
        IsGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        if (secondaryWallCheck)
            WallDetected =
                Physics2D.Raycast(primaryWallCheck.position, transform.right, wallCheckDistance, whatIsGround)
                && Physics2D.Raycast(secondaryWallCheck.position, transform.right, wallCheckDistance,
                    whatIsGround);
        else
            WallDetected = Physics2D.Raycast(primaryWallCheck.position, transform.right, wallCheckDistance,
                whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + transform.right * wallCheckDistance);
        if (secondaryWallCheck)
            Gizmos.DrawLine(secondaryWallCheck.position,
                secondaryWallCheck.position + transform.right * wallCheckDistance);
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}