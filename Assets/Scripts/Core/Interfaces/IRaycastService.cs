using UnityEngine;

public interface IRaycastService
{
    T GetComponentUnderMouse<T>() where T : Component;
    bool TryGetComponentUnderMouse<T>(out T component) where T : Component;
}
