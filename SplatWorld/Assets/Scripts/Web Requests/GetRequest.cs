// Moss Limpert

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GetRequest : MonoBehaviour
{
    //
    // FIELDS
    //
    [SerializeField]
    InputField output;
    [SerializeField]
    Button send;

    //
    // METHODS
    //

    
    /// <summary>
    /// Gets data from the server
    /// </summary>
    /// <param name="args">parameters to search with</param>
    /// <param name="uri">url location to search at</param>
    /// <returns></returns>
    IEnumerator GetData_Coroutine(string uri, string[] args)    // coroutines allow you to spread tasks across several frames
    {                                                           // can pause execution and return control to Unity,     
        string fullUrl = uri;                                   // then coninue where it left off on the following frame. 

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
                    output.text = req.downloadHandler.text;
                    break;
            }
        }
    }

    /// <summary>
    /// Function that starts the coroutine to get data from server 
    /// </summary>
    /// <param name="args">parameters to search with</param>
    /// <param name="uri">url location to search at</param>
    void GetData(string uri, string[] args) => StartCoroutine(GetData_Coroutine(uri, args));
}
