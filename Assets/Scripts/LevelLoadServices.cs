using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class LevelLoadServices : MonoBehaviour
{
    public static LevelLoadServices Instance { get; private set; }

    private Dictionary<int, LevelModel> _levelMap = new Dictionary<int, LevelModel>();
    
    public Dictionary<int, LevelModel> Levels => _levelMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async Task Initialize()
    {
        LoadLevels();
    }

    public void LoadLevels()
    {
        _levelMap.Clear();

        TextAsset jsonFiles = Resources.Load<TextAsset>("Levels/levels");

        var wrapper = JsonConvert.DeserializeObject<LevelConfigWrapper>(jsonFiles.text);

        foreach (var level in wrapper.levels)
        {
            _levelMap.Add(level.levelId, new LevelModel(level));
        }

        Debug.Log($"Loaded {_levelMap.Count} levels");
    }

    public LevelModel GetLevel(int id)
    {
        if (_levelMap.TryGetValue(id, out var level))
            return level;

        Debug.LogError($"Level {id} not found!");
        return null;
    }
}
