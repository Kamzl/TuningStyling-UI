using UnityEngine;
using UnityEngine.UI;

public class TuningButton : MonoBehaviour
{
    [SerializeField] Sprite filledNode;
    [SerializeField] Image[] levels = new Image[5];
    private readonly Color _yellowColor = new Color(1f, 213 / 255f, 84 / 255f);
    private readonly Color _purpleColor = new Color(154 / 255f, 118 / 255f, 1);

    public void SetLevel(int level)
    {
        for(int i = 0; i < level; i++)
        {
            levels[i].sprite = filledNode;
            if (i < 3) levels[i].color = _yellowColor;
            else levels[i].color = _purpleColor;
        }
    }


}
