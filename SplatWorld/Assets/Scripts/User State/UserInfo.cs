using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class UserInfo 
{
    // These are filled out by json utility
    private int id;
    private string name;
    private string pfp_link;
    private string header_link;

    // these are not
    private Image profilePic;
    private Image header;
    

    //
    // Getters and Setters
    //
    public Image ProfilePic
    {
        get
        {
            return profilePic;
        }
        set
        {
            profilePic = value;
        }
    }
    public Image Header
    {
        get
        {
            return header;
        }
        set
        {
            header = value;
        }
    }

    // link to download user's profile picture
    public string DownloadPfpLink()
    {
        return "https://splatworld.alchemi.dev/user-pfp?download-name=pfp-" + id + "&id=" + id;
    }

    // link to download user's header image
    public string DownloadHeaderLink()
    {
        return "https://splatworld.alchemi.dev/header?download-name=header-" + id + "&id=" + id;
    }


}