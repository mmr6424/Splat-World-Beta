using UnityEngine;

public class MatchScreenHeight : MonoBehaviour
{
    [SerializeField]
    RectTransform parentCanvas;

    enum Panel
    {
        Right,
        Down,
        DownRight,
        DownLeft,
        Left,
        PositionPreDefined,
    }

    [SerializeField]
    Panel panelType;

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(parentCanvas.sizeDelta.x, parentCanvas.sizeDelta.y);
        switch (panelType)
        {
            case Panel.Right:
                rectTransform.transform.position = new Vector2((Screen.width / 2) * 3, Screen.height / 2);
                break;
            case Panel.Left: // crew gallery
                rectTransform.transform.position = new Vector2(-(Screen.width / 2), Screen.height / 2);
                break;
            case Panel.Down: // profile panel
                rectTransform.transform.position = new Vector2(Screen.width / 2, -Screen.height / 2);
                break;
            case Panel.DownRight: // settings panel
                rectTransform.transform.position = new Vector2((Screen.width / 2) * 3, -Screen.height / 2);
                break;
            case Panel.DownLeft: // home gallery
                rectTransform.transform.position = new Vector2(-(Screen.width / 2), -Screen.height / 2);
                break;
        }
    }
}