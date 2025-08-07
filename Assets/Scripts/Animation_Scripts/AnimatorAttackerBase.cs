using UnityEngine;

public abstract class AnimatorAttackerBase : MonoBehaviour, IAttacker
{
    protected Animator animator;

    public void SetAnimator(Animator anim)
    {
        animator = anim;
    }

public virtual void Attack()
{
    animator?.SetTrigger("Shoot");
    OnAttack(); // 🔁
}


    public virtual bool CanAttack() => true;

    /// <summary>
    /// Called after triggering animation. Override in subclasses.
    /// </summary>
    protected virtual void OnAttack() { }
}
