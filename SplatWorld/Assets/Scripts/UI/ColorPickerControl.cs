using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using TMPro;

public class ColorPickerControl : MonoBehaviour
{
    public float currentHue, currentSaturation, currentValue;

    [SerializeField]
    private RawImage hueImage, satValImage, outputImage;

    [SerializeField]
    private Slider hueSlider;

    [SerializeField]
    private InputField hexInputField;

    private Texture2D hueTexure, svTexture, outputTexture;

    [SerializeField]
    MeshRenderer changeThisColor;

    [SerializeField]
    GameObject aRObject;

    private void Start()
    {
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();
        UpdateOutputImage();
    }

    private void CreateHueImage()
    {
        hueTexure = new Texture2D(1, 16);
        hueTexure.wrapMode = TextureWrapMode.Clamp;
        hueTexure.name = "HueTexture";

        for (int i = 0; i < hueTexure.height; i++)
        {
            hueTexure.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexure.height, 1, 1));

        }

        hueTexure.Apply();
        currentHue = 0;

        hueImage.texture = hueTexure;
    }

    private void CreateSVImage()
    {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SatValTexture";

        for (int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(
                                    currentHue,
                                    (float)x / svTexture.width,
                                    (float)y / svTexture.height));
            }
        }

        svTexture.Apply();
        currentSaturation = 0;
        currentValue = 0;

        satValImage.texture = svTexture;
    }

    private void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();
        outputImage.texture = outputTexture;
    }

    private void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSaturation, currentValue);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);

        changeThisColor.material.SetColor("_BaseColor", currentColor);

        if (aRObject != null)
            aRObject.GetComponent<ARLineRenderer>().ChangeColor(currentColor);
    }

    public void SetSV(float s, float v)
    {
        currentSaturation = s;
        currentValue = v;

        UpdateOutputImage();
    }

    public void UpdateSVImage()
    {
        currentHue = hueSlider.value;

        for(int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                svTexture.SetPixel(x, y, Color.HSVToRGB(
                                    currentHue,
                                    (float)x / svTexture.width,
                                    (float)y / svTexture.height));
            }
        }

        svTexture.Apply();

        UpdateOutputImage();
    }

    public void OnTextInput()
    {
        if (hexInputField.text.Length < 6)
        {
            return;
        }

        Color newCol;

        if(ColorUtility.TryParseHtmlString("#" + hexInputField.text, out newCol))
        {
            Color.RGBToHSV(newCol, out currentHue, out currentSaturation, out currentValue);
        }

        hueSlider.value = currentHue;

        hexInputField.text = "";

        UpdateOutputImage();
    }

}
