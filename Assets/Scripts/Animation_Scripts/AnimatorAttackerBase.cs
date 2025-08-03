using UnityEngine;

public abstract class AnimatorAttackerBase : MonoBehaviour, IAttacker
{
protected Animator animator;
[SerializeField] protected string triggerName = "Shoot";

public void SetAnimator(Animator animator)
{
    this.animator = animator;
}


    public virtual bool CanAttack() => true;

    public void Attack()
    {
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.ResetTrigger(triggerName);
            animator.SetTrigger(triggerName);
            Debug.Log($"[{GetType().Name}] Triggered animation: {triggerName}");
        }
        else
        {
            Debug.LogWarning($"[{GetType().Name}] Missing animator or trigger name.");
        }

        OnAttack();
    }

    protected abstract void OnAttack();
}
