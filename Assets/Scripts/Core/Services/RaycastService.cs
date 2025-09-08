using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastService : MonoBehaviour, IRaycastService
{
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;

    private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

    private void Awake()
    {
        // јвтоматически находим компоненты если они не заданы
        if (_raycaster == null)
            _raycaster = FindObjectOfType<GraphicRaycaster>();

        if (_eventSystem == null)
            _eventSystem = FindObjectOfType<EventSystem>();
    }

    public T GetComponentUnderMouse<T>() where T : Component
    {
        if (TryGetComponentUnderMouse<T>(out var component))
            return component;

        return null;
    }

    public bool TryGetComponentUnderMouse<T>(out T component) where T : Component
    {
        component = null;

        if (_raycaster == null || _eventSystem == null)
            return false;

        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        _raycastResults.Clear();
        _raycaster.Raycast(pointerEventData, _raycastResults);

        foreach (var result in _raycastResults)
        {
            if (result.gameObject.TryGetComponent<T>(out component))
                return true;
        }

        return false;
    }

    // ƒополнительный метод дл€ получени€ всех компонентов определенного типа под мышью
    public List<T> GetAllComponentsUnderMouse<T>() where T : Component
    {
        var components = new List<T>();

        if (_raycaster == null || _eventSystem == null)
            return components;

        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        _raycastResults.Clear();
        _raycaster.Raycast(pointerEventData, _raycastResults);

        foreach (var result in _raycastResults)
        {
            if (result.gameObject.TryGetComponent<T>(out var component))
                components.Add(component);
        }

        return components;
    }
}