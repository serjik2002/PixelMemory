using UnityEngine;
using UnityEngine.UI;

public class PixelFieldView : MonoBehaviour
{
    [SerializeField] private GameObject _pixelPrefab;
    [SerializeField] Transform _pixelContainer;
    
    private PixelFieldModel _pixelFieldModel;
    private Pixel[,] _pixels;

    public void Bind(PixelFieldModel model)
    {
        _pixelFieldModel = model;
        InitializeField();
        _pixelFieldModel.OnPixelChanged += UpdatePixel;
    }

    private void InitializeField()
    {
        _pixels = new Pixel[_pixelFieldModel.Rows, _pixelFieldModel.Cols];
        GridLayoutGroup gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        float fieldWidth = rectTransform.rect.width;
        float fieldHeight = rectTransform.rect.height;

        float cellWidth = fieldWidth / _pixelFieldModel.Cols;
        float cellHeight = fieldHeight / _pixelFieldModel.Rows;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        Debug.Log("Initializing Pixel Field View");
        for (int row = 0; row < _pixelFieldModel.Rows; row++)
        {
            for (int col = 0; col < _pixelFieldModel.Cols; col++)
            {
                var pixel = Instantiate(_pixelPrefab, _pixelContainer);
                var pixelModel = pixel.GetComponent<Pixel>();
                pixelModel.SetPixelPosition(new Vector2Int(row, col));
                var color = _pixelFieldModel.GetColor(row, col);
                pixelModel.SetPixelColor(color);
                _pixels[row, col] = pixelModel;
            }
        }
    }

    private void UpdatePixel(int row, int col, PixelColorType color)
    {
        if (_pixels[row, col] != null)
        {
            _pixels[row, col].SetPixelColor(color);
        }
    }
}
