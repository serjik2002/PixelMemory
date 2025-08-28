using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PixelFieldController : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;

    [SerializeField] private ColorPickerModel _colorPickerModel;
    [SerializeField] private PixelFieldView _pixelFieldView;

    private PixelFieldModel _pixelFieldModel;

    private void Awake()
    {
        _pixelFieldModel = new PixelFieldModel(10, 10); // размеры поля
        //_pixelFieldView = gameObject.GetComponent<PixelFieldView>();
        _pixelFieldView.Bind(_pixelFieldModel);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            ChangePixel();
    }

    private void ChangePixel()
    {
        print("changepixel");
        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);
        foreach (var result in results)
        {
            if(result.gameObject.TryGetComponent(out Pixel pixel))
            {
                var pos = pixel.Position;
                _pixelFieldModel.SetColor(pos.x, pos.y, _colorPickerModel.SelectedColor);
                break;
            }
        }
    }
}
