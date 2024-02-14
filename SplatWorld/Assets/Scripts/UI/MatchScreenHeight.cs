using UnityEngine;

public class MatchScreenHeight : MonoBehaviour
{
    [SerializeField]
    RectTransform parentCanvas;

    enum Panel
    {
        Profile,
        Settings,
        
    }

    [SerializeField]
    Panel panelType;

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(parentCanvas.sizeDelta.x, parentCanvas.sizeDelta.y);
        switch (panelType)
        {
            case Panel.Profile: // profile panel
                rectTransform.transform.position = new Vector2(Screen.width / 2, -Screen.height / 2);
                break;
            case Panel.Settings: // settings panel
                rectTransform.transform.position = new Vector2((Screen.width / 2) * 3, -Screen.height / 2);
                break;
        }
    }
}