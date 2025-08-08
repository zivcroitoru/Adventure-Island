using System.Collections.Generic;

public static class LevelDatabase
{
    private static readonly Dictionary<LevelId, string> levelToSceneName = new()
    {
        { LevelId.Level1,   "Level1" },
        { LevelId.Level2,   "Level2" },
        { LevelId.Level_Credits,  "Level_Credits" }
    };

    public static string GetSceneName(LevelId id) => levelToSceneName[id];
}
