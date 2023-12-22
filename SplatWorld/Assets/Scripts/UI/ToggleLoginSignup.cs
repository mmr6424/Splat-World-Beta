// Authors: Moss Limpert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleLoginSignup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The User Log in form.")]
    GameObject LoginForm;
    [SerializeField]
    [Tooltip("The User Sign in form.")]
    GameObject SignupForm;

    // from this button
    Text buttonText;

    // button text label
    string SignUp = "Sign Up";
    string Login = "Log In";

    // always start at login form, have user manually toggle to get to sign in form
    bool login = true;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        //Debug.Log(buttonText);
    }

    // toggles the button and login/signup form 
    public void Toggle()
    {
        // if we are currently at login form, switch to signup form
        if (login)
        {
            buttonText.text = Login;
            LoginForm.SetActive(false);
            SignupForm.SetActive(true);
            login = !login;
        } else // if we are at signup form, switch to login form
        {
            buttonText.text = SignUp;
            LoginForm.SetActive(true);
            SignupForm.SetActive(false);
            login = !login;
        }
    }
}
