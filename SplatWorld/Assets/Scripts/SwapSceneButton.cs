using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapSceneButton : MonoBehaviour
{
    [SerializeField]
    private string Scene = "Debug";
    public void CreateButton()
    {
        SceneManager.LoadScene(Scene);
    }
}
