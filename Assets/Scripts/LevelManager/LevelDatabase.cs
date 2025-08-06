using System.Collections.Generic;

public static class LevelDatabase
{
    private static readonly Dictionary<LevelId, string> levelToSceneName = new()
    {
        { LevelId.Level1,   "Level1" },
        { LevelId.Level2,   "Level2" },
        { LevelId.Credits_Scene,  "Credits_Scene" }
    };

    public static string GetSceneName(LevelId id) => levelToSceneName[id];
}
