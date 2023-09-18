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
    Button send;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Post Data to the server
    /// </summary>
    /// <param name="uri">url to put data to</param>
    /// <param name="args">parameters including data</param>
    /// <returns></returns>
    IEnumerator PostData_Coroutine(string uri, string[] args)
    {
        output.text = "Loading...";

        WWWForm form = new WWWForm();

        //form.AddField("title", "test data");

        using (UnityWebRequest req = UnityWebRequest.Post(uri, form))
        {
            yield return req.SendWebRequest();

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
                    output.text = req.downloadHandler.text;
                    break;
            }
        }
    }

    /// <summary>
    /// Starts coroutine to post data to server
    /// </summary>
    /// <param name="uri">url to post data to</param>
    /// <param name="args">parameters, including data</param>
    void PostData(string uri, string[] args) => StartCoroutine(PostData_Coroutine(uri, args));
}
