public class LevelModel
{
    public int Id { get; private set; }
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public int [,] SolutionPixel{ get; private set; }

    public LevelModel(int id, int rows, int cols, int[,] solutionPixels)
    {
        Id = id;
        Rows = rows;
        Cols = cols;
        SolutionPixel = solutionPixels;
    }

    public LevelModel(LevelConfig config)
    {
        Id = config.level;
        Rows = config.gridSize.rows;
        Cols = config.gridSize.cols;
        SolutionPixel = config.colors;
    }

}