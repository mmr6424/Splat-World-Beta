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
    public List<Image> toolSelectSprites = new List<Image>();

    // hides tool button while tool select panel is open
    public void hideToolButton() { toolButton.GetComponent<Image>().color = Color.clear; }

    // un-hide tool button 
    public void showToolButton() { toolButton.GetComponent<Image>().color = Color.white; }

    public void ActivatePencil()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
        // replace open panel button sprite
        toolButton.GetComponent<Image>().sprite = pencilSprite;
        // show open panel button
        showToolButton();
    }

    public void ActivateBucket()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
        toolButton.GetComponent<Image>().sprite = bucketSprite;
        showToolButton();
    }

    public void ActivateEraser()
    {
        brush.SetActive(false);
        can.SetActive(false);
        colorPicker.SetActive(false);
        toolButton.GetComponent<Image>().sprite = eraserSprite;
        showToolButton();
    }

    public void ActivateBrush()
    {
        brush.SetActive(true);
        can.SetActive(false);
        colorPicker.SetActive(false);
        toolButton.GetComponent<Image>().sprite = brushSprite;
        showToolButton();
    }

    public void ActivateCan()
    {
        brush.SetActive(false);
        can.SetActive(true);
        colorPicker.SetActive(false);
        toolButton.GetComponent<Image>().sprite = canSprite;
        showToolButton();
    }

    // not implemented yet
    public void switchToolUI(Sprite newButtonSprite, Image selectedIcon)
    {
        // update tool button to selected tool
        toolButton.GetComponent<Image>().sprite = newButtonSprite;
        // reset all tool select buttons to greyed out
        foreach(Image selectIcon in toolSelectSprites)
        {
            selectIcon.color = new Color32(255, 255, 255, 84);
        }
        // highlight celected tool
        selectedIcon.GetComponent<Image>().color = Color.white;
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
