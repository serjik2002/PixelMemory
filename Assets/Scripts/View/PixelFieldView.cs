using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelFieldView : MonoBehaviour
{
    [SerializeField] private GameObject _pixelPrefab;
    [SerializeField] private Transform _pixelContainer;

    private Pixel[,] _pixels;
    private int _currentRows, _currentCols;

    private Coroutine _showFieldCoroutine;

    public void InitializeField(FieldData fieldData)
    {
        Debug.Log($"Вьюшка поля");
        // Очищаем предыдущее поле если оно было
        ClearField();

        _currentRows = fieldData.Rows;
        _currentCols = fieldData.Cols;
        _pixels = new Pixel[_currentRows, _currentCols];

        SetupGridLayout();
        CreatePixels(fieldData.Colors);

       
    }

    public void ShowFieldTemporarily(FieldData levelFieldData, float duration, System.Action onComplete = null)
    {
        if (_showFieldCoroutine != null)
        {
            StopCoroutine(_showFieldCoroutine);
        }

        _showFieldCoroutine = StartCoroutine(ShowFieldCoroutine(levelFieldData, duration, onComplete));
    }

    private IEnumerator ShowFieldCoroutine(FieldData levelFieldData, float duration, System.Action onComplete)
    {
        // Сохраняем текущее состояние пикселей (из PixelFieldModel)
        var currentFieldState = SaveCurrentFieldState();

        // Показываем поле из LevelModel
        UpdateFieldWithData(levelFieldData);

        // Ждем указанное время
        yield return new WaitForSeconds(duration);

        // Возвращаем обратно поле из PixelFieldModel
        RestoreFieldState(currentFieldState);

        // Вызываем колбэк если он есть
        onComplete?.Invoke();

        _showFieldCoroutine = null;
    }

    private PixelColorType[,] SaveCurrentFieldState()
    {
        if (_pixels == null) return null;

        var savedState = new PixelColorType[_currentRows, _currentCols];

        for (int row = 0; row < _currentRows; row++)
        {
            for (int col = 0; col < _currentCols; col++)
            {
                if (_pixels[row, col] != null)
                {
                    savedState[row, col] = _pixels[row, col].GetPixelColor(); // Предполагаю такой метод существует
                }
            }
        }

        return savedState;
    }

    private void UpdateFieldWithData(FieldData fieldData)
    {
        for (int row = 0; row < fieldData.Rows && row < _currentRows; row++)
        {
            for (int col = 0; col < fieldData.Cols && col < _currentCols; col++)
            {
                if (_pixels[row, col] != null)
                {
                    _pixels[row, col].SetPixelColor(fieldData.Colors[row, col]);
                }
            }
        }
    }

    private void RestoreFieldState(PixelColorType[,] savedState)
    {
        if (savedState == null) return;

        for (int row = 0; row < _currentRows; row++)
        {
            for (int col = 0; col < _currentCols; col++)
            {
                if (_pixels[row, col] != null)
                {
                    _pixels[row, col].SetPixelColor(savedState[row, col]);
                }
            }
        }
    }

    public void UpdatePixel(PixelData pixelData)
    {
        var pos = pixelData.Position;
        if (IsValidPixel(pos.x, pos.y))
        {
            _pixels[pos.x, pos.y].SetPixelColor(pixelData.Color);
        }
    }

    public void UpdateMultiplePixels(IEnumerable<PixelData> pixelDataList)
    {
        foreach (var pixelData in pixelDataList)
        {
            UpdatePixel(pixelData);
        }
    }

    private void SetupGridLayout()
    {
        var gridLayoutGroup = GetComponent<GridLayoutGroup>();
        var rectTransform = GetComponent<RectTransform>();

        if (gridLayoutGroup == null || rectTransform == null) return;

        var fieldSize = rectTransform.rect.size;
        var cellSize = new Vector2(
            fieldSize.x / _currentCols,
            fieldSize.y / _currentRows
        );

        gridLayoutGroup.cellSize = cellSize;
    }

    private void CreatePixels(PixelColorType[,] colors)
    {
        for (int row = 0; row < _currentRows; row++)
        {
            for (int col = 0; col < _currentCols; col++)
            {
                var pixelObject = Instantiate(_pixelPrefab, _pixelContainer);
                var pixelComponent = pixelObject.GetComponent<Pixel>();

                pixelComponent.SetPixelPosition(new Vector2Int(row, col));
                pixelComponent.SetPixelColor(colors[row, col]);

                _pixels[row, col] = pixelComponent;
            }
        }
    }

    private bool IsValidPixel(int row, int col)
    {
        return _pixels != null &&
               row >= 0 && row < _currentRows &&
               col >= 0 && col < _currentCols &&
               _pixels[row, col] != null;
    }

    public void ClearField()
    {
        if (_pixels == null) return;

        for (int row = 0; row < _pixels.GetLength(0); row++)
        {
            for (int col = 0; col < _pixels.GetLength(1); col++)
            {
                if (_pixels[row, col] != null)
                {
                    Destroy(_pixels[row, col].gameObject);
                }
            }
        }

        _pixels = null;
    }

    private void OnDestroy()
    {
        ClearField();
    }
}
