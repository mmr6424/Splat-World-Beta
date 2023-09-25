using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnLoad : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }
}
