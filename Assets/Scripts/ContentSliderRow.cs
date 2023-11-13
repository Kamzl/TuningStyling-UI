using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSliderRow : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform thisTransform;
    private Button currentButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Button AddDetail()
    {
        currentButton = Instantiate(buttonPrefab, thisTransform).GetComponent<Button>();
        return currentButton;
    }
}
