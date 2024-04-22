using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowUIElement : MonoBehaviour
{
    //
    // FIELDS
    //

    [SerializeField]
    GameObject uiToShow;
    [SerializeField]
    bool throwEvent;
    [SerializeField]
    GameObject eventObject;
    UnityEvent unityEvent;

    private void Start()
    {
        
    }

    /// <summary>
    /// Show a piece of UI
    /// </summary>
    public void showUI()
    {
        if (throwEvent)
        {
            unityEvent.Invoke();
        }
        uiToShow.SetActive(true);
    }
}
