using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TuningButton : MonoBehaviour
{
    [SerializeField] Sprite filledNode;
    [SerializeField] Image[] levels = new Image[5];
    private Color yellowColor = new Color(1f, 213 / 255f, 84 / 255f);
    private Color purpleColor = new Color(154 / 255f, 118 / 255f, 1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(int level)
    {
        Debug.Log($"{yellowColor} {purpleColor}");
        for(int i = 0; i < level; i++)
        {
            levels[i].sprite = filledNode;
            if (i < 3) levels[i].color = yellowColor;
            else levels[i].color = purpleColor;
        }
    }


}
