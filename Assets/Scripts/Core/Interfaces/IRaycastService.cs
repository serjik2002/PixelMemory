using UnityEngine;

public interface IRaycastService
{
    bool TryGetComponentUnderPointer<T>(Vector2 screenPosition, out T component) where T : Component;
}
