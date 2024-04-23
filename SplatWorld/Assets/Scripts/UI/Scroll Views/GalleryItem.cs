using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using static UnityEditor.FilePathAttribute;
using static UnityEngine.UI.Button;

public class GalleryItem : MonoBehaviour
{
    // this button
    [SerializeField]
    private Button button;

    // this button's text
    [SerializeField]
    private Text locationText;

    // Tag View panel
    [SerializeField]
    private GameObject tagViewPanel;
    [SerializeField]
    private Text panelArtistText;
    [SerializeField]
    private Text panelLocationText;

    // this button's tag
    private List<string> tagData;
    public List<string> TagData { set { tagData = value; } }

    public void SetTagViewPanelData(GameObject panel, Text artist, Text location)
    {
        locationText.text = tagData[1];
        tagViewPanel = panel;
    }

    public void SetButtonData()
    {
        locationText.text = tagData[1];
    }

    public void AddOnclickFunction(Button.ButtonClickedEvent action)
    {
        button.onClick = action;
    }
}
