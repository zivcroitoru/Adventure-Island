using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class ExitTrigger : MonoBehaviour
{
    [SerializeField] private LevelId nextLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        string sceneName = LevelDatabase.GetSceneName(nextLevel);
        LevelExitHandler.RequestLevelTransition(sceneName);
    }
}
