using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

/// <summary>
/// ScrollViewItem
/// Author: Thomas Martinez
/// Controls items on scrolling list in crew page
/// implementation from https://www.youtube.com/watch?v=1phTiu_qtik
/// 
/// Right now this is specifically geared towards the crew menu
/// </summary>

public class ScrollViewItem : MonoBehaviour
{
    [SerializeField]
    private Text childText;

    [SerializeField]
    private Button button;

    /// <summary>
    /// Called by Dynamic Scroll View to set display text
    /// </summary>
    /// <param name="name">List item name</param>
    public void ChangeText(string name)
    {
        childText.text = name;
    }

    /// <summary>
    ///  Called by Dynamic Scroll View to set onlick function (currently that is open crew menu)
    /// </summary>
    /// <param name="action"></param>
    public void AddOnclickFunction(Button.ButtonClickedEvent action)
    {
        button.onClick = action;
    }
}
