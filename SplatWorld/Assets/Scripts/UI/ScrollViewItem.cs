using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

/// <summary>
/// Controls items on scrolling list in crew page
/// implementation from https://www.youtube.com/watch?v=1phTiu_qtik
/// </summary>

public class ScrollViewItem : MonoBehaviour
{
    [SerializeField]
    private Text childText;

    [SerializeField]
    private Button button;

    public void ChangeText(string name)
    {
        childText.text = name;
    }

    public void AddOnclickFunction(Button.ButtonClickedEvent action)
    {
        button.onClick = action;
    }
}
