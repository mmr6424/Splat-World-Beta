using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// DynamicGalleryGrid
/// AUTHOR: Thomas Martinez
/// Fills grid on gallery page with tag buttons/previews
/// 
/// This script only displays filler data and does not dynamically load gallery information
/// </summary>
public class DynamicGalleryGrid : MonoBehaviour
{
    // Unity scrolling element that displays gallery
    [SerializeField]
    private Transform scrollViewContent;

    // prefab to create gallery items
    [SerializeField]
    private GameObject tagButtonPrefab;

    // list of tag information
    //  > can be changed from List<List<string>> to List<object> where each object is a collection of tag information whaterver form that may take
    [SerializeField]
    private List<List<string>> tags = new List<List<string>>();

    // // Unity GridLayoutGroup that controls gallery content
    [SerializeField]
    private GridLayoutGroup gridlayoutgroup;

    // elements that display info in the tag panel that appears when a gallery item is tapped
    [SerializeField]
    private GameObject tagViewPanel;
    [SerializeField]
    private Text tagViewPanelArtist;
    [SerializeField]
    private Text tagViewPanelLocation;

    // function that triggers when button is clicked
    [SerializeField]
    private Button.ButtonClickedEvent showPanelAction;

    private void Start()
    {
        // populate filler data
        for (int i = 1; i < 20; i++)
        {
            tags.Add(new List<string>() { "artist" + i, "location" + i });
        }

        // set grid item size -> there is an issue here where this doedn't work on android
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
