using System.Collections.Generic;

public static class LevelDatabase
{
    private static readonly Dictionary<LevelId, string> levelToSceneName = new()
    {
        { LevelId.Level1,   "Level_1_Scene" },
        { LevelId.Level2,   "Level_2_Scene" },
        { LevelId.Level_Credits,  "Level_Credits" }
    };

    public static string GetSceneName(LevelId id) => levelToSceneName[id];
}
