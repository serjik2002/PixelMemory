using System;

public interface IPixelFieldView
{
    void InitializeField(FieldData fieldData);
    void UpdatePixel(PixelData pixelData);
    void ShowFieldTemporarily(FieldData fieldData, float duration, Action onComplete = null);
    void ClearField();
}
