using System;
using UnityEngine;
using UnityEngine.Events;

public class PixelFieldModel : MonoBehaviour
{
    [SerializeField] private int _rows = 10;
    [SerializeField] private int _cols = 10;

    [SerializeField] private ColorPickerModel _colorPickerModel;

    private PixelColorType[,] _pixelsData;

    int[,] pixelGrid = new int[10, 10]
{
    { 0,0,1,0,2,0,1,0,0,0 },
    { 0,1,2,1,0,1,2,1,0,0 },
    { 1,2,3,2,1,2,3,2,1,0 },
    { 0,1,2,1,0,1,2,1,0,0 },
    { 2,0,1,0,2,0,1,0,2,0 },
    { 0,1,2,1,0,1,2,1,0,0 },
    { 1,2,3,2,1,2,3,2,1,0 },
    { 0,1,2,1,0,1,2,1,0,0 },
    { 0,0,1,0,2,0,1,0,0,0 },
    { 0,0,0,0,0,0,0,0,0,0 }
};

    public int Rows => _rows;
    public int Cols => _cols;

    public UnityEvent<int, int, PixelColorType> OnPixelChanged = new();
    public UnityEvent OnModelInitialized;

    private void Start()
    {
        InitializeField();
        //InitializeFromArray(pixelGrid);
    }

    public void InitializeField()
    {
        print("Initializing Pixel Field model");
        _pixelsData = new PixelColorType[_rows, _cols];

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                _pixelsData[row, col] = PixelColorType.White;
            }
        }

        OnModelInitialized.Invoke();
    }

    public void InitializeFromArray(int[,] inputArray)
    {
        if (inputArray.GetLength(0) != _rows || inputArray.GetLength(1) != _cols)
        {
            Debug.LogError("Input array size does not match field dimensions.");
            return;
        }

        _pixelsData = new PixelColorType[_rows, _cols];

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                int value = inputArray[row, col];

                PixelColorType color = (PixelColorType)value;
                _pixelsData[row, col] = color;
                //OnPixelChanged?.Invoke(row, col, color);
            }
        }

        OnModelInitialized?.Invoke();
    }


    public void SetColor(int row, int col, PixelColorType color)
    {
        if (InBounds(row, col))
        {
            _pixelsData[row, col] = _colorPickerModel.SelectedColor;
            OnPixelChanged?.Invoke(row, col, _colorPickerModel.SelectedColor);
        }
        
    }

    public PixelColorType GetColor(int row, int col)
    {
        if (InBounds(row, col))
            return _pixelsData[row, col];
        return PixelColorType.Black; // Default color if out of bounds
    }

    private bool InBounds(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }
}
