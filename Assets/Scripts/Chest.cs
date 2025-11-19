using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private static readonly int Open = Animator.StringToHash("Open");
    private Animator animator;
    private bool isOpen;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float amount, Transform damageDealer)
    {
        if (isOpen) return;

        isOpen = true;
        animator.SetTrigger(Open);
    }
}