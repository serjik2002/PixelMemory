using System;

public class FieldInitializer
{
    public static FieldData CreateFieldDataFromModel(IPixelFieldModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        int rows = model.Rows;
        int cols = model.Cols;

        var colors = new PixelColorType[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                colors[row, col] = model.GetColor(row, col);
            }
        }

        return new FieldData(rows, cols, colors);
    }

    public static IPixelFieldModel CreateModel(int rows, int cols)
    {
        return new PixelFieldModel(rows, cols);
    }

    public static void InitializeView(IPixelFieldView view, IPixelFieldModel model)
    {
        if (view == null || model == null) return;

        var fieldData = CreateFieldDataFromModel(model);
        view.InitializeField(fieldData);
    }
}
