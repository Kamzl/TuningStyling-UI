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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        ChangeColor();
        sliderValueBrightness.onValueChanged.AddListener(delegate { ChangeColor(); });
        sliderValueTransparency.onValueChanged.AddListener(delegate { ChangeColor(); });
    }

    // Update is called once per frame
    void Update()
    {
        
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
        temp = position - (Vector2)parentCircle.position;
        pointerCircle.localPosition = Vector2.ClampMagnitude(temp, parentCircle.sizeDelta.x / 2);
        hue = Mathf.Repeat(Mathf.Atan2(temp.x, temp.y), Mathf.PI * 2) / (Mathf.PI * 2);
        saturation = temp.magnitude / (parentCircle.sizeDelta.x / 2);
        ChangeColor();
    }

    private void ChangeColor()
    {
        color = Color.HSVToRGB(hue, saturation, 1);
        Debug.Log($"1 {color}");
        sliderBackgroundBrightness.color = color;
        Debug.Log($"2 {color}");
        if (sliderBackgroundTransparency)
        {
            color = Color.HSVToRGB(hue, saturation, sliderValueBrightness.value);
            color.a = sliderValueTransparency.value;
            sliderBackgroundTransparency.color = Color.HSVToRGB(hue, saturation, sliderValueBrightness.value);
        }
        Debug.Log($"3 {color} {color.a}");
    }

    public Color GetColor()
    {
        return color;
    }
}
