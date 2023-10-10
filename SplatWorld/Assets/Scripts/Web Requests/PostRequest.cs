// Moss Limpert
// references: https://www.youtube.com/playlist?list=PLgdnKWI1HG5A6tCOPaVFXIfiL4H9_1wrU

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PostRequest : MonoBehaviour
{
    //
    // FIELDS
    //
    [SerializeField]
    InputField output;
    [SerializeField]
    List<InputField> input;                 // input fields. VERY IMPORTANT:
                                            // naming of input fields MUST match json 
                                            // field titles
    List<(string fName, Text value)> args;  // list of tuples...
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
        args = new List<(string fName, Text value)>();
        //Debug.Log(send);
        //this.onClick.AddListener(PostData);

        // fill args (list of tuples)
        for (int i = 0; i < input.Count; i++)
        {
            args.Add((input[i].name, input[i].textComponent));
            //Debug.Log(input[i].name);
        }
        // make sure args is full
        //for (int i = 0; i < args.Count; i++)
        //{
        //    Debug.Log(args[i]);
        //}
    }

    /// <summary>
    /// Post Data to the server
    /// </summary>
    /// <returns></returns>
    IEnumerator PostData_Coroutine()
    {
        output.text = "Loading...";

        WWWForm form = new WWWForm();

        //form.AddField("title", "test data");
        for (int i = 0; i < args.Count; i++)
        {
            form.AddField(args[i].fName, args[i].value.text.ToString());
            //Debug.Log(args[i].fName + "," + args[i].value.text.ToString());
        }

        //Debug.Log(form);
        //Debug.Log(form.ToString());
        //Debug.Log(args.ToString());
        //Debug.Log(uri);
        

        using (UnityWebRequest req = UnityWebRequest.Post(uri, form))
        {
            yield return req.SendWebRequest();

            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(String.Format("Connection Error: {0}", req.error));
                    output.text = (String.Format("Connection Error: {0}", req.error));
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(String.Format("Protocol Error: {0}", req.error));
                    output.text = String.Format("Protocol Error: {0}", req.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(String.Format("Data Processing Error: {0}", req.error));
                    output.text = String.Format("Data Processing Error: {0}", req.error);
                    break;
                case UnityWebRequest.Result.Success:
                    output.text = req.downloadHandler.text;
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
