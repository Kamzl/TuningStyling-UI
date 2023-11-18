using System.Collections;
using System.Collections.Generic;
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

    private float hue;
    private float saturation;
    private float brightness;

    private Color color = new Color();

    private float scale = 1;

    private void Awake()
    {
        ChangeColor();
        sliderValueBrightness.onValueChanged.AddListener(delegate { ChangeColor(); });
        sliderValueTransparency.onValueChanged.AddListener(delegate { ChangeColor(); });
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        ChangePointerPosition((Vector2)pointerEventData.position);
    }
    public void OnDrag(PointerEventData data)
    {
        ChangePointerPosition((Vector2)data.position);
    }

    private void ChangePointerPosition(Vector2 position)
    {
        Vector2 temp;
        float radius = parentCircle.sizeDelta.x / 2;
        scale = parentCircle.lossyScale.x;
        temp = (position - (Vector2)parentCircle.position) / scale;
        pointerCircle.localPosition = Vector2.ClampMagnitude(temp, radius);
        hue = Mathf.Repeat(Mathf.Atan2(temp.x, temp.y), Mathf.PI * 2) / (Mathf.PI * 2);
        saturation = Vector2.ClampMagnitude(temp, radius).magnitude / radius;
        Debug.Log(saturation);
        Debug.Log(temp.magnitude / (radius));
        ChangeColor();
    }

    private void ChangeColor()
    {
        color = Color.HSVToRGB(hue, saturation, 1);
        sliderBackgroundBrightness.color = color;
        if (sliderBackgroundTransparency)
        {
            color = Color.HSVToRGB(hue, saturation, sliderValueBrightness.value);
            color.a = sliderValueTransparency.value;
            sliderBackgroundTransparency.color = Color.HSVToRGB(hue, saturation, sliderValueBrightness.value);
        }
    }

    public Color GetColor()
    {
        return color;
    }
}
