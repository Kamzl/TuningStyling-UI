using UnityEngine;
using UnityEngine.UI;

public class ContentSlider : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform thisTransform;
    [SerializeField] private ContentSliderRow rowScript;
    private int _rowCount = 0;

    public Button AddDetail()
    {
        if (_rowCount <= 2) _rowCount += 1;
        else
        {
            rowScript = Instantiate(rowPrefab, thisTransform).GetComponent<ContentSliderRow>();
            _rowCount = 1;
        }
        Button result = rowScript.AddDetail();
        return result;
    }
}
