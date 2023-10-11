// Authors: Charles Begle, Moss Limpert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public GameObject startButton, loadingInterface;
    public Image loadingProgressBar;

    [SerializeField]
    GameObject LoginForm;
    [SerializeField]
    GameObject SignupForm;
    [SerializeField]
    GameObject LoginSignupToggle;
    //Event LoginSucceeded;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    
    void Start(){
        startButton.SetActive(false);
        loadingInterface.SetActive(false);

        //LoginSucceeded = GetComponentInChildren<Event>();
    }

    public void StartGame()
    {
        HideMenu();
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Debug"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Crew", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }

    // Update is called once per frame
    public void HideMenu()
    {
        startButton.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress=0;
        for(int i=0; i<scenesToLoad.Count; ++i)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                loadingProgressBar.fillAmount = totalProgress/scenesToLoad.Count;
                yield return null;
            }
        }
    }

    public void OnLoginEnableStartGame()
    {
        LoginForm.SetActive(false);
        SignupForm.SetActive(false);
        startButton.SetActive(true);
        LoginSignupToggle.SetActive(false);
    }
}
