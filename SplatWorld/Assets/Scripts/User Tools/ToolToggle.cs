using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolToggle : MonoBehaviour
{
    public GameObject brush, can, colorPicker;
    // Start is called before the first frame update

    public void ActivateBrush()
    {
        brush.SetActive(true);
        can.SetActive(false);
        colorPicker.SetActive(false);
    }

    public void ActivateCan()
    {
        brush.SetActive(false);
        can.SetActive(true);
        colorPicker.SetActive(false);
    }

    public void ActivateColorPicker()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(true);
    }

    void Start()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
