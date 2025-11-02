using UnityEngine;

public abstract class EntityState
{
    protected readonly StateMachine stateMachine;
    private readonly string animBoolName;

    protected Animator animator;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;

    protected EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        animator.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    protected virtual void UpdateAnimationParameters()
    {
        
    }
}