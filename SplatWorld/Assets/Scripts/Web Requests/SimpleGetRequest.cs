using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum ReqType
{
    GETUSERCREWS,
}

public class GetUserCrews
{
    public int count;
    public int[] ids;
    public int reqtype;

    public override string ToString()
    {
        return "count:" + count + " " + "ids: " + ids.ToString();
    }
}

public class SimpleGetRequest : MonoBehaviour
{
    //
    // FIELDS
    //
    [SerializeField]
    string userId;
    [SerializeField]
    string uri;
    [SerializeField]
    Text output;
    [SerializeField]
    char toReplace;
  
    List<(string fName, string value)> args;

    //
    // METHODS
    //
    /// <summary>
    /// Sets up buttons to send request on click
    /// </summary>
    private void Start()
    {
        GetData();
    }

    /// <summary>
    /// Gets data from the server
    /// </summary>
    /// <param name="args">parameters to search with</param>
    /// <param name="uri">url location to search at</param>
    /// <returns></returns>
    IEnumerator GetData_Coroutine()     // coroutines allow you to spread tasks across several frames
    {                                   // can pause execution and return control to Unity,     
        string fullUrl = uri;           // then coninue where it left off on the following frame. 

        fullUrl += "?id=" + userId;
        
        //if (args.Count > 0)
        //{
        //    fullUrl += "?";
        //    for (int i = 0; i < args.Count; i++)
        //    {
        //        if (i > 0) fullUrl += "&";
        //        fullUrl += args[i].fName + "=" + args[i].value.ToString();
        //    }
        //}

        //Debug.Log(fullUrl);

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
                    //Debug.Log(req.downloadHandler.text);
                    //Debug.Log(String.Format("result: {0}", req));
                    if (req.downloadHandler.isDone)
                    {
                        //Debug.Log(req.downloadHandler.data);
                        string temp = output.text;
                        //Debug.Log(req.downloadHandler.text);
                        GetUserCrews data = JsonUtility.FromJson<GetUserCrews>(req.downloadHandler.text);
                        Debug.Log(data.ToString());

                        // this is provided in the callback to the db query in the request handler 
                        // in the node js server code
                        switch (data.reqtype) {
                            case 0:
                                string[] split = temp.Split('%');
                                split[1] = String.Format("{0}", data.count);

                                temp = "";
                                // stitch back together
                                for (int i = 1; i < split.Length; i++)
                                {
                                    //Debug.Log(split[i]);
                                    temp += split[i] + " ";
                                }
                                //temp = temp.Replace("%NUMTOREPLACE%", String.Format("{0}", data.count));
                                break;
                            default:
                                output.text = "request failed";
                                break;
                        }

                        //Debug.Log(temp);
                        output.text = temp;
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
