using System;
using UnityEngine;

public class PixelFieldController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PixelFieldView _pixelFieldView;
    [SerializeField] private LevelController _levelController;
    [SerializeField] private InputHandler _inputHandler;

    [Header("Settings")]
    [SerializeField] private float _levelPreviewDuration = 3f;
    [SerializeField] private bool _showLevelPreview = true;

    // Специализированные компоненты
    private FieldInputProcessor _inputProcessor;
    private LevelCompletionChecker _completionChecker;
    private FieldPreviewManager _previewManager;

    // Состояние
    private IPixelFieldModel _pixelFieldModel;
    private IPixelFieldView _fieldView;

    // События
    public event Action<float> OnLevelCompleted;
    public event Action OnFieldInitialized;

    #region Unity Lifecycle

    private void Awake()
    {
        ValidateDependencies();
        InitializeComponents();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
        DisposeComponents();
    }

    #endregion

    #region Initialization

    private void ValidateDependencies()
    {
        if (_inputHandler == null)
            Debug.LogError($"[{nameof(PixelFieldController)}] InputHandler not found!");

        if (_pixelFieldView == null)
            Debug.LogError($"[{nameof(PixelFieldController)}] PixelFieldView not assigned!");

        if (_levelController == null)
            Debug.LogError($"[{nameof(PixelFieldController)}] LevelController not assigned!");
    }

    private void InitializeComponents()
    {
        _fieldView = _pixelFieldView;
        _completionChecker = new LevelCompletionChecker();
        //_previewManager = new FieldPreviewManager(_fieldView, this);
    }

    #endregion

    #region Event Management

    private void SubscribeToEvents()
    {
        if (_levelController != null)
            _levelController.OnLevelLoad += HandleLevelLoad;

        if (_completionChecker != null)
            _completionChecker.OnLevelCompleted += HandleLevelCompleted;

        if (_previewManager != null)
            _previewManager.OnPreviewComplete += HandlePreviewComplete;
    }

    private void UnsubscribeFromEvents()
    {
        if (_levelController != null)
            _levelController.OnLevelLoad -= HandleLevelLoad;

        if (_completionChecker != null)
            _completionChecker.OnLevelCompleted -= HandleLevelCompleted;

        if (_previewManager != null)
            _previewManager.OnPreviewComplete -= HandlePreviewComplete;

        UnsubscribeFromModelEvents();
        DisposeInputProcessor();
    }

    private void SubscribeToModelEvents()
    {
        if (_pixelFieldModel != null)
        {
            _pixelFieldModel.OnPixelChanged += HandlePixelChanged;
            _pixelFieldModel.OnFieldCleared += HandleFieldCleared;
        }
    }

    private void UnsubscribeFromModelEvents()
    {
        if (_pixelFieldModel != null)
        {
            _pixelFieldModel.OnPixelChanged -= HandlePixelChanged;
            _pixelFieldModel.OnFieldCleared -= HandleFieldCleared;
        }
    }

    #endregion

    #region Event Handlers

    private void HandleLevelLoad()
    {
        var levelModel = _levelController.LevelModel;
        if (levelModel == null)
        {
            Debug.LogError("Level model is null!");
            return;
        }

        InitializeField(levelModel);

        if (_showLevelPreview)
        {
            ShowLevelPreview(levelModel);
        }
    }

    private void HandlePixelChanged(PixelData pixelData)
    {
        _fieldView?.UpdatePixel(pixelData);

        // Проверяем завершение уровня после каждого изменения
        CheckLevelCompletion();
    }

    private void HandleFieldCleared()
    {
        Debug.Log("Field cleared");
    }

    private void HandleLevelCompleted(float completionTime)
    {
        //SetInputEnabled(false);
        OnLevelCompleted?.Invoke(completionTime);
    }

    private void HandlePreviewComplete()
    {
        SetInputEnabled(true);
        Debug.Log("Preview completed. Game can start!");
    }

    #endregion

    #region Core Logic

    private void InitializeField(LevelModel levelModel)
    {
        // Создаем модель поля
        CreateFieldModel(levelModel.Rows, levelModel.Cols);

        // Инициализируем представление
        FieldInitializer.InitializeView(_fieldView, _pixelFieldModel);

        // Создаем обработчик ввода
        CreateInputProcessor();

        OnFieldInitialized?.Invoke();
    }

    private void CreateFieldModel(int rows, int cols)
    {
        UnsubscribeFromModelEvents();
        _pixelFieldModel = FieldInitializer.CreateModel(rows, cols);
        SubscribeToModelEvents();
    }

    private void CreateInputProcessor()
    {
        DisposeInputProcessor();

        if (_inputHandler != null && _pixelFieldModel != null)
        {
            _inputProcessor = new FieldInputProcessor(_pixelFieldModel, _inputHandler);
            _inputProcessor.OnInputProcessed += CheckLevelCompletion;
        }
    }

    private void CheckLevelCompletion()
    {
        var levelModel = _levelController?.LevelModel;
        if (levelModel != null && _pixelFieldModel != null)
        {
            _completionChecker?.ProcessCompletionCheck(_pixelFieldModel, levelModel);
        }
    }

    private void ShowLevelPreview(LevelModel levelModel)
    {
        //SetInputEnabled(false);
        _previewManager?.ShowLevelPreview(levelModel, _levelPreviewDuration);
    }

    #endregion

    #region Public API

    public void SetInputEnabled(bool enabled)
    {
        _inputProcessor?.SetInputEnabled(enabled);
    }

    public void ClearField()
    {
        _pixelFieldModel?.Clear();
    }

    public void SetPixelColor(int row, int col, PixelColorType color)
    {
        if (_pixelFieldModel?.IsValidPosition(row, col) == true)
        {
            _pixelFieldModel.SetColor(row, col, color);
        }
        else
        {
            Debug.LogWarning($"Invalid position: ({row}, {col})");
        }
    }

    public PixelColorType GetPixelColor(int row, int col)
    {
        return _pixelFieldModel?.GetColor(row, col) ?? PixelColorType.White;
    }

    public void ShowLevelSolution(float duration = 3f)
    {
        var levelModel = _levelController?.LevelModel;
        if (levelModel == null) return;

        var solutionData = new FieldData(
            levelModel.Rows,
            levelModel.Cols,
            levelModel.SolutionPixels
        );

        //SetInputEnabled(false);
        _previewManager?.ShowCustomPreview(solutionData, duration, () => SetInputEnabled(true));
    }

    public float GetCompletionPercentage()
    {
        var levelModel = _levelController?.LevelModel;
        if (levelModel == null || _pixelFieldModel == null) return 0f;

        return _completionChecker?.GetCompletionPercentage(_pixelFieldModel, levelModel) ?? 0f;
    }

    public bool IsLevelSolved()
    {
        var levelModel = _levelController?.LevelModel;
        if (levelModel == null || _pixelFieldModel == null) return false;

        return _completionChecker?.CheckCompletion(_pixelFieldModel, levelModel) ?? false;
    }

    #endregion

    #region Cleanup

    private void DisposeComponents()
    {
        DisposeInputProcessor();
        _pixelFieldModel = null;
    }

    private void DisposeInputProcessor()
    {
        if (_inputProcessor != null)
        {
            _inputProcessor.OnInputProcessed -= CheckLevelCompletion;
            _inputProcessor.Dispose();
            _inputProcessor = null;
        }
    }
}
    #endregion
    
