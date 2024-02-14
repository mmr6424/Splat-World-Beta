using UnityEngine;

public class MatchScreenHeight : MonoBehaviour
{
    [SerializeField]
    RectTransform parentCanvas;
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(parentCanvas.sizeDelta.x, parentCanvas.sizeDelta.y);
        rectTransform.transform.position = new Vector2(Screen.width / 2, -Screen.height / 2);
    }
}