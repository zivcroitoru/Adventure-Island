using UnityEngine;

/// <summary>
/// Base class for attackers that use animation.
/// </summary>
public abstract class AnimatorAttackerBase : MonoBehaviour, IAttacker
{
    protected Animator animator;

    public void SetAnimator(Animator anim)
    {
        animator = anim;
    }

    public virtual void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot"); // Always use "Shoot"
            Debug.Log("[AnimatorAttackerBase] Triggering 'Shoot' animation");
        }

        OnAttack(); // 🔁 Logic hook for subclasses
    }

    public virtual bool CanAttack() => true;

    /// <summary>
    /// Called after the shoot animation is triggered.
    /// Override in subclasses to handle logic.
    /// </summary>
    protected virtual void OnAttack() { }
}
