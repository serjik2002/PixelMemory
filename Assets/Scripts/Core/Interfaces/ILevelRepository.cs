public interface ILevelRepository
{
    LevelModel GetLevel(int id);
    bool HasLevel(int id);
    int GetLevelCount();
}
