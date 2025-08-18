using UnityEngine;

public class PixelFieldView : MonoBehaviour
{
    [SerializeField] private PixelFieldModel _pixelFieldModel;
    [SerializeField] private GameObject _pixelPrefab;
    [SerializeField] Transform _pixelContainer;
    
    private Pixel[,] _pixels;

    

    private void Start()
    {
        _pixelFieldModel.OnModelInitialized.AddListener(InitializeField);
        _pixelFieldModel.OnPixelChanged.AddListener(UpdatePixel);
    }

    private void InitializeField()
    {
        _pixels = new Pixel[_pixelFieldModel.Rows, _pixelFieldModel.Cols];

        print("Initializing Pixel Field View");
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
