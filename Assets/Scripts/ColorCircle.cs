using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorCircle : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] RectTransform parentCircle;
    [SerializeField] Transform pointerCircle;
    [SerializeField] Slider sliderValueBrightness;
    [SerializeField] Image sliderBackgroundBrightness;
    [SerializeField] Slider sliderValueTransparency;
    [SerializeField] Image sliderBackgroundTransparency;

    private float _hue;
    private float _saturation;
    private float _brightness;

    private Color _color;

    private float _scale = 1;

    private void Awake()
    {
        ChangeColor();
        sliderValueBrightness.onValueChanged.AddListener(delegate { ChangeColor(); });
        sliderValueTransparency.onValueChanged.AddListener(delegate { ChangeColor(); });
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        ChangePointerPosition(pointerEventData.position);
    }
    public void OnDrag(PointerEventData data)
    {
        ChangePointerPosition(data.position);
    }

    private void ChangePointerPosition(Vector2 position)
    {
        Vector2 temp;
        float radius = parentCircle.sizeDelta.x / 2;
        _scale = parentCircle.lossyScale.x;
        temp = (position - (Vector2)parentCircle.position) / _scale;
        pointerCircle.localPosition = Vector2.ClampMagnitude(temp, radius);
        _hue = Mathf.Repeat(Mathf.Atan2(temp.x, temp.y), Mathf.PI * 2) / (Mathf.PI * 2);
        _saturation = Vector2.ClampMagnitude(temp, radius).magnitude / radius;
        Debug.Log(_saturation);
        Debug.Log(temp.magnitude / (radius));
        ChangeColor();
    }

    private void ChangeColor()
    {
        _color = Color.HSVToRGB(_hue, _saturation, 1);
        sliderBackgroundBrightness.color = _color;
        if (sliderBackgroundTransparency)
        {
            _color = Color.HSVToRGB(_hue, _saturation, sliderValueBrightness.value);
            _color.a = sliderValueTransparency.value;
            sliderBackgroundTransparency.color = Color.HSVToRGB(_hue, _saturation, sliderValueBrightness.value);
        }
    }

    public Color GetColor()
    {
        return _color;
    }
}
