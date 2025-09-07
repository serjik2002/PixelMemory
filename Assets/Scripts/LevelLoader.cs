using UnityEngine;
using Newtonsoft.Json;

public static class LevelLoader
{

    public static LevelConfig LoadLevel(int levelIndex)
    {
        string fileName = $"Levels/level_{levelIndex}";

        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        

        if (jsonFile != null)
        {
            var loadedLevel = JsonConvert.DeserializeObject<LevelConfig>(jsonFile.text);
            return loadedLevel;
        }
        else
        {
            Debug.LogError("Τΰιλ νε νΰιδεν β Resources: " + fileName);
            return null;
        }
    }
}

