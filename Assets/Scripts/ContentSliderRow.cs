using UnityEngine;
using UnityEngine.UI;

public class ContentSliderRow : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform thisTransform;
    private Button _currentButton;

    public Button AddDetail()
    {
        _currentButton = Instantiate(buttonPrefab, thisTransform).GetComponent<Button>();
        return _currentButton;
    }
}
