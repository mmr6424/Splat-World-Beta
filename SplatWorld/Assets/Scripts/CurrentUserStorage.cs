using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUserStorage : MonoBehaviour
{
    /// <summary>
    /// author: Thomas Martinez
    /// stores basic info about profile for get requests
    /// </summary>
    private int currentUserID;
    private string currentUserName;

    public int CurrentUserID { get { return currentUserID; } set { currentUserID = value; } }
    public string CurrentUserName { get { return currentUserName; } set { currentUserName = value; } }

    // maintains this object between scene loads
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetUserData(string data)
    {
        currentUserName = data.;
    }
}
