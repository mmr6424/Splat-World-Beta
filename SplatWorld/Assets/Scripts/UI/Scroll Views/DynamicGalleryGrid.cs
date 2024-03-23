using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// AUTHOR: Thomas Martinez
/// Fills grid on gallery page with tag buttons
/// </summary>
public class DynamicGalleryGrid : MonoBehaviour
{
    [SerializeField]
    private Transform scrollViewContent;

    [SerializeField]
    private GameObject tagButtonPrefab;

    [SerializeField]
    private List<List<string>> tags = new List<List<string>>();

    [SerializeField]
    private GridLayoutGroup gridlayoutgroup;

    [SerializeField]
    private Button.ButtonClickedEvent movePanelAction;

    private void Start()
    {
        // filler data
        for (int i = 1; i < 20; i++)
        {
            tags.Add(new List<string>() { "atist", "location" });
        }

        // set grid item size
        gridlayoutgroup.cellSize = new Vector2(Screen.width / 3, Screen.width / 3);

        // populate gallery
        foreach (List<string> tag in tags)
        {
            GameObject newCrewButton = Instantiate(tagButtonPrefab, scrollViewContent);
            if (newCrewButton.TryGetComponent<GalleryItem>(out GalleryItem item))
            {
                item.SetTagArtist(tag[0]);
                item.SetTagLocation(tag[1]);
                item.AddOnclickFunction(movePanelAction);
            }
        }
    }
}
