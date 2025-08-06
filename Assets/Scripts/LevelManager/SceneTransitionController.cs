using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TransitionTo(string sceneName)
    {
        // TODO: Add fade, save, sfx, etc.
        SceneManager.LoadScene(sceneName);
    }
}
