public static class LevelExitHandler
{
    public static void RequestLevelTransition(string sceneName)
    {
        SceneTransitionController.Instance.TransitionTo(sceneName);
    }
}
