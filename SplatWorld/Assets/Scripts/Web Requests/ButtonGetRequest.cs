// Moss Limpert

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ButtonGetRequest : MonoBehaviour
{
    //
    // FIELDS
    //
    [SerializeField]
    Text output;
    [SerializeField]
    Button send;
    [SerializeField]
    List<Text> input;
    [SerializeField]
    List<(string fName, string value)> args;
    [SerializeField]
    string uri;


    //
    // METHODS
    //
    /// <summary>
    /// Sets up buttons to send request on click
    /// </summary>
    private void Start()
    {
        args = new List<(string fName, string value)>();

        // fill args (list of tuples)
        for (int i = 0; i < input.Count; i++)
        {
            args.Add((input[i].name, input[i].text));
        }

    }

    /// <summary>
    /// Gets data from the server
    /// </summary>
    /// <returns></returns>
    IEnumerator GetData_Coroutine()    // coroutines allow you to spread tasks across several frames
    {                                                           // can pause execution and return control to Unity,     
        string fullUrl = uri;                                   // then coninue where it left off on the following frame. 

        if (args.Count > 0)
        {
            fullUrl += "?";
            for (int i = 0; i < args.Count; i++)
            {
                if (i > 0) fullUrl += "&";
                fullUrl += args[i].fName + "=" + args[i].value;
            }
        }

        Debug.Log(fullUrl);

        // using defines a boundary for the object, outside of which,
        // the object is automatically destroyed
        using (UnityWebRequest req = UnityWebRequest.Get(fullUrl))
        {
            // yield works with IEnumerable objects. if a function
            // starts for eaching over it, the func. is called again until it yields.
            yield return req.SendWebRequest();

            // if we error, print the error to output
            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(String.Format("Connection Error: {0}", req.error));
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(String.Format("Protocol Error: {0}", req.error));
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(String.Format("Data Processing Error: {0}", req.error));
                    break;
                case UnityWebRequest.Result.Success:
                    if (req.downloadHandler.isDone)
                    {
                        output.text = req.downloadHandler.text;
                    }
                    else output.text = "Nothing to download";
                    break;
            }
        }
    }

    /// <summary>
    /// Function that starts the coroutine to get data from server 
    /// </summary>
    public void GetData() => StartCoroutine(GetData_Coroutine());
}
