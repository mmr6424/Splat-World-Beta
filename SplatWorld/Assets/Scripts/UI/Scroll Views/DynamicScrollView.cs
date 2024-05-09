using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Author: Thomas Martinez
/// Controls scrolling list of crew select buttons on crew page
/// 
/// dynamic scroll implementation from https://www.youtube.com/watch?v=1phTiu_qtik
/// 
/// Right now this is specifically geared towards the crew menu. This script could be adapted to create other scroll views as well 
/// or it could be renamed to create more
/// 
/// This script only displays filler data and does not dynamically load crew information
/// </summary>
public class DynamicScrollView : MonoBehaviour
{
    // Unity scrolling element that displays list
    [SerializeField]
    private Transform scrollViewContent;

    // prefab to create list items
    [SerializeField]
    private GameObject crewButtonPrefab;

    // placeholder for crew information that should be retrieved from server
    [SerializeField]
    private List<string> crewNames = new List<string>();

    // function that triggers when button is clicked
    [SerializeField]
    private Button.ButtonClickedEvent movePanelAction;

    private void Start()
    {
        // populate filler data
        crewNames.Add("Splat Team");

        // iterate through data and add crews to list
        foreach (string name in crewNames)
        {
            ListCrew(name);
        }
    }

    /// <summary>
    /// creates a clickable button list item 
    /// </summary>
    /// <param name="name">name of crew</param>
    private void ListCrew(string name)
    {
        // create list item
        GameObject newCrewButton = Instantiate(crewButtonPrefab, scrollViewContent);
        if (newCrewButton.TryGetComponent<ScrollViewItem>(out ScrollViewItem item))
        {
            // set list item's display text
            item.ChangeText(name);
            // 
            item.AddOnclickFunction(movePanelAction);
        }
    }
}
