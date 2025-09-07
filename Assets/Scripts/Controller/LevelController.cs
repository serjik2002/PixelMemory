using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] RectTransform _field;
     private LevelModel _levelModel;

    public LevelModel LevelModel => _levelModel;
    
    public event Action OnLevelLoad;


    public void LoadLevel(int id)
    {
        LevelConfig config = LevelLoader.LoadLevel(id);
        _levelModel = new LevelModel(config);
        OnLevelLoad?.Invoke();
    }

    public void ShowLevel(float delay)
    {

    }
}
