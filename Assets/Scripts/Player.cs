using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Animator playerVisualsAnimator;

    [Header("Attack")] public Vector2[] attackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1;

    public bool ShouldAttack => attackAction.WasPressedThisFrame();

    [Header("Movement")] public float moveSpeed;
    [Range(0, 1)] public float airSpeedMultiplier = 0.8f;
    public float jumpForce = 12f;
    public Vector2 wallJumpForce;
    [Range(0, 1)] public float slideSpeedMultiplier = 0.3f;
    [Space] public float dashDuration = 0.25f;
    public float dashSpeed = 20;

    public Vector2 MoveInput { get; private set; }
    public bool ShouldJump => jumpAction.WasPressedThisFrame();
    public bool ShouldDash => dashAction.WasPressedThisFrame();
    public int FacingDirection => (int)transform.right.x;
    private bool isFacingRight = true;

    [Header("Collision Detection")] [SerializeField]
    private float groundCheckDistance = 1f;

    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;

    public bool IsGrounded { get; private set; }
    public bool WallDetected { get; private set; }

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction attackAction;

    private StateMachine stateMachine;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerBasicAttackState BasicAttackState { get; private set; }
    public PlayerJumpAttackState JumpAttackState { get; private set; }
    public Animator PlayerVisualsAnimator => playerVisualsAnimator;
    public Rigidbody2D Rb { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, stateMachine, "Idle");
        MoveState = new PlayerMoveState(this, stateMachine, "Move");
        JumpState = new PlayerJumpState(this, stateMachine, "JumpFall");
        FallState = new PlayerFallState(this, stateMachine, "JumpFall");
        WallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, stateMachine, "JumpFall");
        DashState = new PlayerDashState(this, stateMachine, "Dash");
        BasicAttackState = new PlayerBasicAttackState(this, stateMachine, "BasicAttack");
        JumpAttackState = new PlayerJumpAttackState(this, stateMachine, "JumpAttack");
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");
        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        HandleCollisionDetection();
        stateMachine.Update();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.CurrentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
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
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        WallDetected = Physics2D.Raycast(primaryWallCheck.position, transform.right, wallCheckDistance, whatIsGround)
                       && Physics2D.Raycast(secondaryWallCheck.position, transform.right, wallCheckDistance,
                           whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + transform.right * wallCheckDistance);
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + transform.right * wallCheckDistance);
    }
}