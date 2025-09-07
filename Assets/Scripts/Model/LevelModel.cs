public class LevelModel
{
    public int Id { get; private set; }
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public PixelColorType [,] SolutionPixel{ get; private set; }

    public LevelModel(int id, int rows, int cols, PixelColorType[,] solutionPixels)
    {
        Id = id;
        Rows = rows;
        Cols = cols;
        SolutionPixel = solutionPixels;
    }

    public LevelModel(LevelConfig config)
    {
        Id = config.levelId;
        Rows = config.gridSize.rows;
        Cols = config.gridSize.cols;
        SolutionPixel = config.colors;
    }

}