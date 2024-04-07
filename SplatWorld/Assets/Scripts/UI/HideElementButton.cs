using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HideElementButton : MonoBehaviour
{
    [SerializeField]
    GameObject UiToHide;

    /// <summary>
    /// 
    /// </summary>
    public void HideUI()
    {
        UiToHide.SetActive(false);
    }
}
