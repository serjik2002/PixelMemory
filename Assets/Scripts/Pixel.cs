using UnityEngine;
using UnityEngine.UI;

public class Pixel : MonoBehaviour
{
    private Vector2Int _pixelPosition;
    private PixelColorType _pixelColor;

    public Vector2Int Position => _pixelPosition;


    public Vector2 GetPixelPosition()
    {
        return _pixelPosition;
    }
    
    public PixelColorType GetPixelColor()
    {
        return _pixelColor;
    }

    public void SetPixelPosition(Vector2Int position)
    {
        _pixelPosition = position;
    }

    public void SetPixelColor(PixelColorType color)
    {
        _pixelColor = color;
        gameObject.GetComponent<Image>().color = ColorData.ColorMap[color];
    }
}
