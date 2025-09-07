using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public struct PixelData
{
    public Vector2Int Position { get; }
    public PixelColorType Color { get; }

    public PixelData(Vector2Int position, PixelColorType color)
    {
        Position = position;
        Color = color;
    }
}

public struct FieldData
{
    public int Rows { get; }
    public int Cols { get; }
    public PixelColorType[,] Colors { get; }

    public FieldData(int rows, int cols, PixelColorType[,] colors)
    {
        Rows = rows;
        Cols = cols;
        Colors = colors;
    }
}

public class PixelFieldController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private ColorPickerModel _colorPickerModel;
    [SerializeField] private PixelFieldView _pixelFieldView;
    [SerializeField] private LevelController _levelController;

    [Header("Settings")]
    [SerializeField] private bool _enableInput = true;

    private PixelFieldModel _pixelFieldModel;
    private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

    public PixelFieldModel PixelFieldModel => _pixelFieldModel;

    private void Start()
    {
        _levelController.OnLevelLoad += Initialize;
    }

    private void Initialize()
    {
        var levelModel = _levelController.LevelModel;
        CreateModel(levelModel.Rows, levelModel.Cols);
        SubscribeToModelEvents();
        InitializeView();
    }

    private void InitializeView()
    {
        if (_pixelFieldModel == null) return;

        int rows = _pixelFieldModel.Rows;
        int cols = _pixelFieldModel.Cols;

        var colors = new PixelColorType[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                colors[row, col] = _pixelFieldModel.GetColor(row, col);
            }
        }

        var fieldData = new FieldData(rows, cols, colors);
        _pixelFieldView?.InitializeField(fieldData);
        var levelFieldData = CreateFieldDataFromLevel();
        _pixelFieldView.ShowFieldTemporarily(levelFieldData, 3f, OnFieldShowComplete);
    }
    private void OnFieldShowComplete()
    {
        Debug.Log("Поле скрыто, игра может начинаться");
        // Здесь можно включить ввод или начать игру
    }

    private FieldData CreateFieldDataFromLevel()
    {
        var levelModel = _levelController.LevelModel;
        // Предполагаю, что в levelModel есть массив с целевым полем
        return new FieldData(levelModel.Rows, levelModel.Cols, levelModel.SolutionPixel); // или как там называется массив
    }

    private void CreateModel(int rows, int cols)
    {
        UnsubscribeFromModelEvents(); // Отписываемся от старых событий
        _pixelFieldModel = new PixelFieldModel(rows, cols);
    }

    private void SubscribeToModelEvents()
    {
        if (_pixelFieldModel != null)
        {
            _pixelFieldModel.OnPixelChanged += HandlePixelChanged;
            //_pixelFieldModel.OnFieldInitialized += HandleFieldInitialized;
            print("Подписался на события модели поля");
        }
    }

    private void UnsubscribeFromModelEvents()
    {
        if (_pixelFieldModel != null)
        {
            _pixelFieldModel.OnPixelChanged -= HandlePixelChanged;
            //_pixelFieldModel.OnFieldInitialized -= HandleFieldInitialized;
        }
    }

    private void HandlePixelChanged(PixelData pixelData)
    {
        _pixelFieldView?.UpdatePixel(pixelData);
    }

    private void HandleFieldInitialized(FieldData fieldData)
    {
        print("Вьюшка поля инициализирована");
        _pixelFieldView?.InitializeField(fieldData);
        

    }

    private void Update()
    {
        if (_enableInput && Input.GetMouseButton(0))
        {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        var pixel = GetPixelUnderMouse();
        if (pixel != null && _colorPickerModel != null)
        {
            var pos = pixel.Position;
            _pixelFieldModel?.SetColor(pos.x, pos.y, _colorPickerModel.SelectedColor);
        }
    }

    private Pixel GetPixelUnderMouse()
    {
        if (_raycaster == null || _eventSystem == null) return null;

        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        _raycastResults.Clear();
        _raycaster.Raycast(pointerEventData, _raycastResults);

        foreach (var result in _raycastResults)
        {
            if (result.gameObject.TryGetComponent<Pixel>(out var pixel))
            {
                return pixel;
            }
        }

        return null;
    }

    // Публичные методы для внешнего управления
    public void SetEnableInput(bool enable)
    {
        _enableInput = enable;
    }

    public void ClearField()
    {
        _pixelFieldModel?.Clear();
    }

    public void SetPixelColor(int row, int col, PixelColorType color)
    {
        _pixelFieldModel?.SetColor(row, col, color);
    }

    private void OnDestroy()
    {
        UnsubscribeFromModelEvents();
    }
}
