using System;
using UnityEngine;
using UnityEngine.UI;

public class PixelFieldView : MonoBehaviour, IPixelFieldView
{
    [SerializeField] private GameObject _pixelPrefab;
    [SerializeField] Transform _pixelContainer;

    private PixelView[,] _pixelViews;


    public void ClearField()
    {
        if( _pixelViews != null)
        {
            foreach (var pixelView in _pixelViews)
            {
                Destroy(pixelView.gameObject);
            }

        }
    }

    public void ShowFieldTemporarily(FieldData fieldData, float duration, Action onComplete = null)
    {
        throw new NotImplementedException();
    }

    public void InitializeField(FieldData fieldData)
    {
        ClearField();

        int rows = fieldData.Rows;
        int cols = fieldData.Cols;
        _pixelViews = new PixelView[rows, cols];

        // Настройка грида
        var gridLayout = _pixelContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            var rect = _pixelContainer.GetComponent<RectTransform>().rect;
            gridLayout.cellSize = new Vector2(
                rect.width / cols,
                rect.height / rows
            );
        }

        // Создание пикселей
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var pixelView = Instantiate(_pixelPrefab, _pixelContainer);
                var view = pixelView.GetComponent<PixelView>();
                view.Init(row, col, PixelColorType.White);
                _pixelViews[row, col] = view;
            }
        }
    }
    
    

    private void UpdatePixel(int row, int col, PixelColorType color)
    {
        if (_pixelViews[row, col] != null)
        {
            _pixelViews[row, col].SetColor(color);
        }
    }

    private void UpdatePixel(PixelData pixelData)
    {
        var position = pixelData.Position;
        UpdatePixel(position.x, position.y, pixelData.Color);
    }

    void IPixelFieldView.UpdatePixel(PixelData pixelData)
    {
        UpdatePixel(pixelData);
    }
}
