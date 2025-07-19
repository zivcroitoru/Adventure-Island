using UnityEngine;

public class PlayerDeathListener : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        SC_Death.OnSpikeCollision += TriggerDeathAnimation;
    }

    void OnDisable()
    {
        SC_Death.OnSpikeCollision -= TriggerDeathAnimation;
    }

    void TriggerDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die"); // Make sure you have a "Die" trigger in the Animator
        }

        // Optionally disable player control
        // GetComponent<PlayerController>().enabled = false;
    }
}
