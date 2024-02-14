// Author: Moss Limpert
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DynamicGetRequest : MonoBehaviour
{
    private string uri = "";
    private string args = "";
    private string output = "";

    //
    // getters and setters
    //

    public string Output
    {
        get
        {
            return output;
        }
    }

    public string Url
    {
        set
        {
            uri = value;
        }
    }

    public string Args
    {
        set
        {
            args = value;
        }
    }

    //
    // constructor
    //

    public DynamicGetRequest(string uri, string args)
    {
        this.uri = uri;
        this.args = args;
    }
    
    IEnumerator GetData_Coroutine()
    {
        string fullUrl = uri + args;

        using (UnityWebRequest req = UnityWebRequest.Get(fullUrl))
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
                    if (req.downloadHandler.isDone)
                    {
                        output = req.downloadHandler.text;
                    }
                    break;
                default:
                    output = "";
                    break;
            }
        }
    }

    public void GetData() {

        if (uri == "" || args == "")
        {
            return;
        }

        StartCoroutine(GetData_Coroutine());
    }

    public override string ToString()
    {
        return $"url: {uri}  args: {args}  output: {output}";
    }
}
