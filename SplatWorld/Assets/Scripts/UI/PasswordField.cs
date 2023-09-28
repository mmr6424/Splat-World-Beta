// Moss Limpert

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordField : MonoBehaviour
{
    //
    // FIELDS
    //

    private string password = "";

    //
    // ACCESS
    //

    /// <summary>
    /// getter for password so we can send get request for login
    /// </summary>
    public string passwd
    {
        get { return password; }
    }

    //
    // METHODS
    //

    /// <summary>
    /// when gui loads, add a password input field
    /// </summary>
    private void OnGUI()
    {
        // currently the below isnt working to get the style
        GUIStyle passwordStyle = new GUIStyle("inputField");
        passwordStyle.fontSize = 32;
        password = GUI.PasswordField(
            new Rect (200, 350, 600, 50), 
            password, 
            "*"[0], 
            45,
            passwordStyle
            );
    }
}
