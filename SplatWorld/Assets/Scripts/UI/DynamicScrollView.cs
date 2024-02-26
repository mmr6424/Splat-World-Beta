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
    private UnityAction<GameObject> movePanelAction;

    private void Start()
    {
        crewNames.Add("Franks");
        crewNames.Add("Eclipse");
        crewNames.Add("DOT EXE");
        crewNames.Add("Devil Theory");
        crewNames.Add("Devil Theory");
        crewNames.Add("The Oldheads");
        crewNames.Add("Bomb Rush Crew");
        foreach (string name in crewNames)
        {
            GameObject newCrewButton = Instantiate(crewButtonPrefab, scrollViewContent);
            if (newCrewButton.TryGetComponent<ScrollViewItem>(out ScrollViewItem item))
            {
                item.ChangeText(name);
            }
        }
    }
}
