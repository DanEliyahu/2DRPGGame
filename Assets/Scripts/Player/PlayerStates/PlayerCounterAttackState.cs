using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private static readonly int CounterAttackTrigger = Animator.StringToHash("CounterAttackTrigger");
    private bool hasCountered;

    public PlayerCounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player,
        stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterRecoveryTime;
        player.SetVelocity(0, rb.linearVelocity.y);

        hasCountered = player.PerformCounterAttack();
        if (hasCountered)
            animator.SetTrigger(CounterAttackTrigger);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);

        if (stateTimer <= 0 && !hasCountered)
            stateMachine.ChangeState(player.IdleState);
    }
}