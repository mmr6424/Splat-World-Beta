using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Controls scrolling list of crew select buttons on crew page
/// implementation from https://www.youtube.com/watch?v=1phTiu_qtik
/// </summary>
public class DynamicScrollView : MonoBehaviour
{

    [SerializeField]
    private Transform scrollViewContent;

    [SerializeField]
    private GameObject crewButtonPrefab;

    [SerializeField]
    private List<string> crewNames = new List<string>();

    [SerializeField]
    private Button.ButtonClickedEvent movePanelAction;

    private void Start()
    {
        // filler data
        crewNames.Add("Splat Team");

        foreach (string name in crewNames)
        {
            ListCrew(name);
        }
    }

    private void ListCrew(string name)
    {
        GameObject newCrewButton = Instantiate(crewButtonPrefab, scrollViewContent);
        if (newCrewButton.TryGetComponent<ScrollViewItem>(out ScrollViewItem item))
        {
            item.ChangeText(name);
            item.AddOnclickFunction(movePanelAction);
        }
    }

    public void AddCrew(string crewName)
    {
        crewNames.Add(crewName);
        ListCrew(crewName);
    }
}
