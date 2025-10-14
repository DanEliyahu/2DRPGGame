using UnityEngine;

public abstract class EntityState
{
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    protected readonly Player player;
    protected readonly StateMachine stateMachine;
    private readonly string animBoolName;

    protected readonly Animator animator;
    protected readonly Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;

    protected EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        animator = player.PlayerVisualsAnimator;
        rb = player.Rb;
    }

    public virtual void Enter()
    {
        animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        animator.SetFloat(YVelocity, rb.linearVelocity.y);

        if (player.ShouldDash && CanDash())
            stateMachine.ChangeState(player.DashState);
        
    }

    public virtual void Exit()
    {
        animator.SetBool(animBoolName, false);
    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }

    private bool CanDash()
    {
        if (player.WallDetected)
            return false;
        if (stateMachine.CurrentState == player.DashState)
            return false;

        return true;
    }
}