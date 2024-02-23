using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoginPlayerInfo : MonoBehaviour
{
    string username;
    int id;
    string pfp_link;
    string header_link;
    string join_date;

    public override string ToString()
    {
        return "username: " + username + "  pfp link: " + pfp_link +"  header link: " + header_link +"  join date: " + join_date;
    }
}
