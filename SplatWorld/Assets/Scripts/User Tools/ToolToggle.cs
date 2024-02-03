using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolToggle : MonoBehaviour
{
    public Image toolButton;
    public GameObject brush, can, colorPicker;
    // Start is called before the first frame update

    public Sprite brushSprite, canSprite, pencilSprite, bucketSprite, eraserSprite;

    public void ActivatePencil()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
        // replace tool button sprite
        toolButton.GetComponent<Image>().sprite = pencilSprite;
    }

    public void ActivateBucket()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
        // replace tool button sprite
        toolButton.GetComponent<Image>().sprite = bucketSprite;
    }

    public void ActivateEraser()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
        // replace tool button sprite
        toolButton.GetComponent<Image>().sprite = eraserSprite;
    }

    public void ActivateBrush()
    {
        brush.SetActive(true);
        can.SetActive(false);
        colorPicker.SetActive(false);
        // replace tool button sprite
        toolButton.GetComponent<Image>().sprite = brushSprite;
    }

    public void ActivateCan()
    {
        brush.SetActive(false);
        can.SetActive(true);
        colorPicker.SetActive(false);
        toolButton.GetComponent<Image>().sprite = canSprite;
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
