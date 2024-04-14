using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Thomas Martinez
/// outputs basic user data on current user's profile page
/// </summary>
public class ProfileInfoManager : MonoBehaviour
{
    private CurrentUserStorage userData;

    // ui objects:
    [SerializeField]
    private Text usernameText;

    public int UID { get { return userData.CurrentUserID; } }

    // Start is called before the first frame update
    void Start()
    {
        // get info from Current User Storage
        CurrentUserStorage[] curruserstore = FindObjectsOfType(typeof(CurrentUserStorage)) as CurrentUserStorage[];
        CurrentUserStorage userData = curruserstore[0];

        // output info
        usernameText.text = userData.CurrentUserName;
    }
}
