using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("[RestartManager] Restarting level with F key...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
