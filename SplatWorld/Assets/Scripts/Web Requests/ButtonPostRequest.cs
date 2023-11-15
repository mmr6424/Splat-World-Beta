// Moss Limpert
// references: https://www.youtube.com/playlist?list=PLgdnKWI1HG5A6tCOPaVFXIfiL4H9_1wrU

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ButtonPostRequest : MonoBehaviour
{
    //
    // FIELDS
    //
    [SerializeField]
    InputField output;
    [SerializeField]
    List<Text> input;                 // input fields. VERY IMPORTANT:
                                            // naming of input fields MUST match json 
                                            // field titles
    List<(string fName, string value)> args;  // list of tuples...
    [SerializeField]
    string uri;
    [SerializeField]
    bool throwEvent;

    public UnityEvent RequestSucceeded;

    //
    // METHODS
    //
    /// <summary>
    /// Sets up buttons to send request on click
    /// </summary>
    private void Start()
    {
        args = new List<(string fName, string value)>();
    }

    /// <summary>
    /// Post Data to the server
    /// </summary>
    /// <returns></returns>
    IEnumerator PostData_Coroutine()
    {
        output.text = "Loading...";

        WWWForm form = new WWWForm();

        for (int i = 0; i < input.Count; i++)
        {
            args.Add((input[i].name, input[i].text));
            //Debug.Log("i:" + i + " text: " + input[i].text);
        }

        if (args.Count > 0)
        {
            for (int i = 0; i < args.Count; i++)
            {
                form.AddField(args[i].fName, args[i].value);
                //Debug.Log(args[i].fName + "," + args[i].value);
            }
        }

        UnityWebRequest req;

        using (req = UnityWebRequest.Post(uri, form))
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(String.Format("Connection Error: {0}", req.error));
                    output.text = (String.Format("Connection Error: {0}", req.error));
                    if (req.downloadHandler.isDone)
                    {
                        output.text = req.downloadHandler.text;
                    }
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(String.Format("Protocol Error: {0}", req.error));
                    output.text = String.Format("Protocol Error: {0}", req.error);
                    if (req.downloadHandler.isDone)
                    {
                        output.text = req.downloadHandler.text;
                    }
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(String.Format("Data Processing Error: {0}", req.error));
                    output.text = String.Format("Data Processing Error: {0}", req.error);
                    if (req.downloadHandler.isDone)
                    {
                        output.text = req.downloadHandler.text;
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    if (req.downloadHandler.isDone)
                    {
                        output.text = req.downloadHandler.text;
                    }
                    // success triggers an event, if you set bool throwEvent = true in inspector
                    if (throwEvent)
                    {
                        RequestSucceeded.Invoke();
                    }
                    break;
                default:
                    output.text = String.Format("{0}", req.responseCode);
                    break;
            }
        }
    }

    /// <summary>
    /// Starts coroutine to post data to server
    /// </summary>
    public void PostData() => StartCoroutine(PostData_Coroutine());
}
