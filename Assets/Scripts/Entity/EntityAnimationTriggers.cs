using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{
    private Entity entity;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        entity.PerformAttack();
    }
}