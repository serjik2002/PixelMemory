using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorPickerController : MonoBehaviour
{
    [SerializeField] private ColorPickerModel _model;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TrySelectColor();
    }

    private void TrySelectColor()
    {
        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out ColorPickerView colorView))
            {
                _model.ChangeSelectedColor(colorView.ColorType);
                break;
            }
        }
    }
}
