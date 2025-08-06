public static class LevelExitHandler
{
    public static void RequestLevelTransition(string sceneName)
    {
        // Optional: fade out, play sound, save state, etc.
        SceneTransitionController.Instance.TransitionTo(sceneName);
    }
}
