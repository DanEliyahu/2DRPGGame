using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    [Header("Input")] [SerializeField] private InputActionAsset inputActions;

    [Header("Attack")] public Vector2[] attackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1;

    [Header("Counter Attack")] public float counterRecoveryTime = 0.2f;

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
    public bool ShouldAttack => attackAction.WasPressedThisFrame();
    public bool CounterPressed => counterAttackAction.WasPressedThisFrame();

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction attackAction;
    private InputAction counterAttackAction;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerBasicAttackState BasicAttackState { get; private set; }
    public PlayerJumpAttackState JumpAttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    private PlayerDeadState DeadState { get; set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new PlayerIdleState(this, stateMachine, "Idle");
        MoveState = new PlayerMoveState(this, stateMachine, "Move");
        JumpState = new PlayerJumpState(this, stateMachine, "JumpFall");
        FallState = new PlayerFallState(this, stateMachine, "JumpFall");
        WallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, stateMachine, "JumpFall");
        DashState = new PlayerDashState(this, stateMachine, "Dash");
        BasicAttackState = new PlayerBasicAttackState(this, stateMachine, "BasicAttack");
        JumpAttackState = new PlayerJumpAttackState(this, stateMachine, "JumpAttack");
        CounterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        DeadState = new PlayerDeadState(this, stateMachine, "Dead");

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");
        counterAttackAction = InputSystem.actions.FindAction("Counter");
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += _ => MoveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
    }

    protected override void Die()
    {
        base.Die();

        OnPlayerDeath?.Invoke();
        try
        {
            stateMachine.ChangeState(DeadState);
        }
        catch (Exception e)
        {
            if (e is not StateSwitchedException) throw;
        }
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    public bool PerformCounterAttack()
    {
        var counterPerformed = false;

        foreach (var target in GetDetectedColliders())
        {
            var counterable = target.GetComponent<ICounterable>();

            if (counterable is not { CanBeCountered: true })
                continue;

            counterable.HandleCounter();
            counterPerformed = true;
        }

        return counterPerformed;
    }
}