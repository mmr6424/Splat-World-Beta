using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.UI.Button;

/// <summary>
/// GalleryItem
/// Author: Thomas Martinez
/// Gallery Item that displays previews of tag w/ a location and open larger view when tapped
/// 
/// This script only displays filler data and does not dynamically load gallery information
/// </summary>
public class GalleryItem : MonoBehaviour
{
    // this button
    [SerializeField]
    private Button button;

    // this button's text
    [SerializeField]
    private Text locationText;

    // Tag View panel information
    [SerializeField]
    private GameObject tagViewPanel;
    [SerializeField]
    private Text panelArtistText;
    [SerializeField]
    private Text panelLocationText;

    // this button's tag
    private List<string> tagData;
    public List<string> TagData { set { tagData = value; } }

    /// <summary>
    /// Sets the info that is displayed by the tag view
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="artist"></param>
    /// <param name="location"></param>
    public void SetTagViewPanelData(GameObject panel, Text artist, Text location)
    {
        locationText.text = tagData[1];
        tagViewPanel = panel;
    }

    /// <summary>
    /// stores tag information related to this gallery item
    /// called by DynamicGalleryGrid that controls items
    /// </summary>
    public void SetButtonData()
    {
        locationText.text = tagData[1];
    }

    /// <summary>
    /// sets the function that is called when gallery item is clicked
    /// 
    /// called by DynamicGalleryGrid that controls items
    /// </summary>
    /// <param name="action"> Tag View -> GameObject.SetActive(true) </param>
    public void AddOnclickFunction(Button.ButtonClickedEvent action)
    {
        button.onClick = action;
    }
}
