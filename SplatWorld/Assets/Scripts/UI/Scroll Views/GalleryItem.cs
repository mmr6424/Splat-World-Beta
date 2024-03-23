using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class GalleryItem : MonoBehaviour
{
    [SerializeField]
    private Text artistText;
    [SerializeField]
    private Text locationText;

    [SerializeField]
    private Button button;

    public void SetTagArtist(string artist)
    {
        artistText.text = artist;
    }

    public void SetTagLocation(string location)
    {
        locationText.text = location;
    }
        

    public void AddOnclickFunction(Button.ButtonClickedEvent action)
    {
        button.onClick = action;
    }
}
