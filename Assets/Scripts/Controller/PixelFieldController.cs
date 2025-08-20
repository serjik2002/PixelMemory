using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PixelFieldController : MonoBehaviour
{
    [SerializeField] private PixelFieldModel _pixelFieldModel;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;

    private void Update()
    {
        if (Input.GetMouseButton(0))
            ChangePixel();
    }

    private void ChangePixel()
    {
        var pointerEventData = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);
        Pixel pixel = null;
        foreach (var result in results)
        {
            if(result.gameObject.TryGetComponent<Pixel>(out pixel))
            {
                var pos = pixel.Position; // Vector2Int
                _pixelFieldModel.SetColor(pos.x, pos.y, PixelColorType.Red);
                break;
            }
        }
    }
}
