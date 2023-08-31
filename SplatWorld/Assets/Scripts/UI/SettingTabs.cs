using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingTabs : MonoBehaviour
{
    public GameObject tab1, tab2, tab3, tab4;
    public Text settingsText;
    // Start is called before the first frame update
    void Start()
    {
        TabOne();
    }

    public void TabOne()
    {
        settingsText.text = "Video";
        tab1.SetActive(true);
        tab2.SetActive(false);
        tab3.SetActive(false);
        tab4.SetActive(false);
    }

    public void TabTwo()
    {
        settingsText.text = "Audio";
        tab1.SetActive(false);
        tab2.SetActive(true);
        tab3.SetActive(false);
        tab4.SetActive(false);
    }

    public void TabThree()
    {
        settingsText.text = "Community";
        tab1.SetActive(false);
        tab2.SetActive(false);
        tab3.SetActive(true);
        tab4.SetActive(false);
    }

    public void TabFour()
    {
        settingsText.text = "Security";
        tab1.SetActive(false);
        tab2.SetActive(false);
        tab3.SetActive(false);
        tab4.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
