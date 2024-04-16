using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUIElement : MonoBehaviour
{
    //
    // FIELDS
    //

    [SerializeField]
    GameObject uiToShow;

    /// <summary>
    /// Show a piece of UI
    /// </summary>
    public void showUI()
    {
        uiToShow.SetActive(true);
    }
}
