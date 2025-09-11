using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

// Сервис загрузки уровней
public class LevelLoader
{
    public static async Task<LevelConfig> LoadLevelAsync(int levelIndex)
    {
        string fileName = $"Levels/level_{levelIndex}";

        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

            if (jsonFile == null)
            {
                throw new ArgumentException($"Level file not found: {fileName}");
            }

            // Имитация асинхронной загрузки
            await Task.Yield();

            var loadedLevel = JsonConvert.DeserializeObject<LevelConfig>(jsonFile.text);

            if (loadedLevel == null)
            {
                throw new InvalidOperationException($"Failed to deserialize level: {fileName}");
            }

            return loadedLevel;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading level {levelIndex}: {ex.Message}");
            throw;
        }
    }

    public static async Task<LevelConfigWrapper> LoadAllLevelsAsync()
    {
        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Levels/levels");

            if (jsonFile == null)
            {
                throw new ArgumentException("Levels file not found: Levels/levels");
            }

            await Task.Yield();

            var wrapper = JsonConvert.DeserializeObject<LevelConfigWrapper>(jsonFile.text);

            if (wrapper?.levels == null)
            {
                throw new InvalidOperationException("Failed to deserialize levels wrapper");
            }

            return wrapper;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading all levels: {ex.Message}");
            throw;
        }
    }
}

// Репозиторий для управления уровнями
public class LevelRepository : MonoBehaviour, ILevelRepository
{
    public static LevelRepository Instance { get; private set; }

    private readonly Dictionary<int, LevelModel> _levelMap = new Dictionary<int, LevelModel>();
    private bool _isInitialized;

    public event Action OnLevelsLoaded;
    public event Action<string> OnLoadError;

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

    public async Task InitializeAsync()
    {
        if (_isInitialized)
        {
            Debug.LogWarning("LevelRepository already initialized");
            return;
        }

        try
        {
            await LoadLevelsAsync();
            _isInitialized = true;
            OnLevelsLoaded?.Invoke();
            Debug.Log($"LevelRepository initialized with {_levelMap.Count} levels");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize LevelRepository: {ex.Message}");
            OnLoadError?.Invoke(ex.Message);
            throw;
        }
    }

    private async Task LoadLevelsAsync()
    {
        _levelMap.Clear();

        var wrapper = await LevelLoader.LoadAllLevelsAsync();

        foreach (var levelConfig in wrapper.levels)
        {
            try
            {
                var levelModel = new LevelModel(levelConfig);
                _levelMap.Add(levelModel.Id, levelModel);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create LevelModel for level {levelConfig.levelId}: {ex.Message}");
                // Продолжаем загрузку остальных уровней
            }
        }
    }

    public LevelModel GetLevel(int id)
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("LevelRepository not initialized. Call InitializeAsync first.");
        }

        if (!_levelMap.TryGetValue(id, out var level))
        {
            throw new ArgumentException($"Level {id} not found!");
        }

        return level;
    }

    public bool HasLevel(int id)
    {
        return _isInitialized && _levelMap.ContainsKey(id);
    }

    public int GetLevelCount()
    {
        return _isInitialized ? _levelMap.Count : 0;
    }

    public int[] GetAvailableLevelIds()
    {
        if (!_isInitialized) return new int[0];

        var ids = new int[_levelMap.Count];
        _levelMap.Keys.CopyTo(ids, 0);
        return ids;
    }

    private void OnDestroy()
    {
        OnLevelsLoaded = null;
        OnLoadError = null;
    }
}