using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private LevelModel _levelModel;

    public LevelModel LevelModel => _levelModel;
    
    public event Action OnLevelLoad;


    public void LoadLevel(int id)
    {
        _levelModel = LevelLoadServices.Instance.GetLevel(id);
        Debug.Log($"Level {id} loaded: {_levelModel.Rows}x{_levelModel.Cols}");
        OnLevelLoad?.Invoke();
    }
}
