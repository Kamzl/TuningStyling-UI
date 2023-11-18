using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform thisTransform;
    [SerializeField] ContentSliderRow rowScript;
    private int rowCount = 0;

    public Button addDetail()
    {
        if (rowCount <= 2) rowCount += 1;
        else
        {
            rowScript = Instantiate(rowPrefab, thisTransform).GetComponent<ContentSliderRow>();
            rowCount = 1;
        }
        Button result = rowScript.AddDetail();
        return result;
    }
}
