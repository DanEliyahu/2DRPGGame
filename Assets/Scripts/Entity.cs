using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int FacingDirection => (int)transform.right.x;
    private bool isFacingRight = true;

    [Header("Collision Detection")] [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;

    public bool IsGrounded { get; private set; }
    public bool WallDetected { get; private set; }

    protected StateMachine stateMachine;
    public Animator Anim => anim;
    public Rigidbody2D Rb { get; private set; }

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
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

    public void SetVelocity(float xVelocity, float yVelocity)
    {
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
    }

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
    }
}