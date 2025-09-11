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
        // ������������� ������� ���������� ���� ��� �� ������
        if (_raycaster == null)
            _raycaster = FindAnyObjectByType<GraphicRaycaster>();

        if (_eventSystem == null)
            _eventSystem = FindAnyObjectByType<EventSystem>();
    }


    public bool TryGetComponentUnderPointer<T>(Vector2 screenPosition, out T component) where T : Component
    {
        component = null;
        if (_raycaster == null || _eventSystem == null)
            return false;

        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = screenPosition
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


    // �������������� ����� ��� ��������� ���� ����������� ������������� ���� ��� �����
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