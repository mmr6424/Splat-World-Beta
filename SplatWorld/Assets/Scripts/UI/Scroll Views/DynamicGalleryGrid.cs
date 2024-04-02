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

    // tag panel references
    [SerializeField]
    private GameObject tagViewPanel;
    [SerializeField]
    private Text tagViewPanelArtist;
    [SerializeField]
    private Text tagViewPanelLocation;

    [SerializeField]
    private Button.ButtonClickedEvent showPanelAction;

    private void Start()
    {
        // filler data
        for (int i = 1; i < 20; i++)
        {
            tags.Add(new List<string>() { "artist" + i, "location" + i });
        }

        // set grid item size
        gridlayoutgroup.cellSize = new Vector2(Screen.width / 3, Screen.width / 3);

        // populate gallery
        foreach (List<string> tag in tags)
        {
            GameObject newCrewButton = Instantiate(tagButtonPrefab, scrollViewContent);
            if (newCrewButton.TryGetComponent<GalleryItem>(out GalleryItem item))
            {
                // store tag data in button
                item.TagData = tag;

                // set references to tag view panel text
                item.SetTagViewPanelData(tagViewPanel, tagViewPanelArtist, tagViewPanelLocation);

                // set button display text
                item.SetButtonData();

                // button opens tag view when clicked
                item.AddOnclickFunction(showPanelAction);
            }
        }
    }
}
