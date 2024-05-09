using UnityEngine;

/// <summary>
/// MatchScreenSize
/// Author: Thomas Martinez
/// Matches panel height and width to camera size and then positions it at a specific position relative to the landing page
/// </summary>

public class MatchScreenHeight : MonoBehaviour
{
    // canvas to match panel size to
    [SerializeField]
    RectTransform parentCanvas;

    // possible positions
    enum Panel
    {
        Right,
        Down,
        DownRight,
        DownLeft,
        Left,
        PositionPreDefined,
    }

    // position of this panel
    [SerializeField]
    Panel panelType;

    void Start()
    {
        // set this panel's dimensions to the dimensions of the reference canvas (which is matched to the dimensions of the camera)
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(parentCanvas.sizeDelta.x, parentCanvas.sizeDelta.y);

        // set position
        switch (panelType)
        {
            case Panel.Right: // to the right of landing
                rectTransform.transform.position = new Vector2((Screen.width / 2) * 3, Screen.height / 2);
                break;
            case Panel.Left: // to the left of landing
                rectTransform.transform.position = new Vector2(-(Screen.width / 2), Screen.height / 2);
                break;
            case Panel.Down: // below landing
                rectTransform.transform.position = new Vector2(Screen.width / 2, -Screen.height / 2);
                break;
            case Panel.DownRight: // etc
                rectTransform.transform.position = new Vector2((Screen.width / 2) * 3, -Screen.height / 2);
                break;
            case Panel.DownLeft: // etc
                rectTransform.transform.position = new Vector2(-(Screen.width / 2), -Screen.height / 2);
                break;
            // more positions can be implemented here but these are the only positions that have been used so far
        }
    }
}