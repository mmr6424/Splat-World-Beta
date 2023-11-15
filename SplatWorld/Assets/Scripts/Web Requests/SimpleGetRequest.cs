// Authors: Moss Limpert
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// helper enum that allows us to format strings differently
/// based on what data we are actually asking for
/// </summary>
public enum ReqType
{
    GETUSERCREWS,
    GETTAGCOUNT,
    GETPOINTS,
    GENERIC,

}

/// <summary>
/// contain data from a getusercrews request
/// </summary>
public class GetUserCrews
{
    public int count;
    public int[] ids;
    public string[] names;

    public override string ToString()
    {
        //string n = "";
        //for (int i = 0; i < names.Length; i++)
        //{
        //    n += names[i] + ", ";
        //}
        return "count:" + count + " " + "ids: " + ids + " names: " + names;
    }
}

/// <summary>
/// contain data from a gettagcount request
/// </summary>
public class GetTagCount {
    public int count;

    public override string ToString()
    {
        return count + " tags";
    }
}

public class GetPoints
{
    public int points;

    public override string ToString()
    {
        return points + "splat points";
    }
}

/// <summary>
/// Contain data from a request that just sends a simple message as a response
/// </summary>
public class Generic
{
    public string message;

    public override string ToString()
    {
        return message;
    }
}


/// <summary>
/// Get Request Script that works without user-inputted form data
/// </summary>
public class SimpleGetRequest : MonoBehaviour
{
    //
    // FIELDS
    //
    [Header("Temporary Fields")]
    [SerializeField]
    string userId;

    [Header("Request Variables")]
    [SerializeField]
    string uri;
    [SerializeField]
    ReqType requestType;

    [Header("Response Variables")]
    [SerializeField]
    Text output;
    [SerializeField]
    char toReplace;
    [SerializeField]
    string prefix;
    [SerializeField]
    string suffix;
    
  
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
                        string[] split;
                        //Debug.Log(req.downloadHandler.text);
                        
                        // this is provided in the inspector, makes this script re-usable
                        switch (requestType) {
                            case ReqType.GETUSERCREWS:
                                // format: %NUMOFTAGS% Tags
                                // convert the response data to a class with fields
                                GetUserCrews userCrews = JsonUtility.FromJson<GetUserCrews>(req.downloadHandler.text);
                                Debug.Log(userCrews.ToString());
                                Debug.Log(req.downloadHandler.text);
                                
                                //for (int i = 0; i < userCrews.names.Length; i++)
                                //{
                                //    Debug.Log(userCrews.names[i]);
                                //}

                                split = temp.Split(toReplace);
                                //split[1] = String.Format("{0}", userCrews.count);

                                // i will fix this later when the request provides names for each id
                                string listOfNames = "";
                                int count;
                                bool moreThanFour; // if there are more than 4 crews, append ... to the end of the list
                                if (userCrews.count < 4) {
                                    moreThanFour = false;
                                    count = userCrews.count;
                                } else {
                                    moreThanFour = true;
                                    count = 4;
                                }
                                // make a string list of the crew names
                                for (int i = 0; i < count; i++)
                                {
                                    listOfNames += userCrews.names[i];
                                    if (i < count - 1) listOfNames += ", ";
                                }
                                if (moreThanFour) listOfNames += "...";
                                split[split.Length - 2] = listOfNames;

                                temp = "";
                                // stitch back together
                                if (!String.IsNullOrEmpty(prefix)) temp += prefix;
                                for (int i = 1; i < split.Length; i++)
                                {
                                    //Debug.Log(split[i]);
                                    temp += split[i] + " ";
                                }
                                if (!String.IsNullOrEmpty(suffix)) temp += suffix;

                                break;

                            case ReqType.GETTAGCOUNT:
                                // format: Member of: %CREWNAME%
                                // convert the response data to a class with fields
                                GetTagCount tagCount = JsonUtility.FromJson<GetTagCount>(req.downloadHandler.text);
                                //Debug.Log(tagCount.ToString());

                                split = temp.Split(toReplace);
                                //Debug.Log(split.Length);

                                split[1] = String.Format("{0}", tagCount.count);

                                // stitch back together
                                temp = "";
                                if (!String.IsNullOrEmpty(prefix)) temp += prefix;
                                temp += split[1] + " ";
                                if (!String.IsNullOrEmpty(suffix)) temp += suffix;

                                break;

                            case ReqType.GETPOINTS:
                                // format: % Splat points
                                GetPoints points = JsonUtility.FromJson<GetPoints>(req.downloadHandler.text);

                                temp = String.Format("{0} ", points.points);
                                if (!String.IsNullOrEmpty(suffix)) temp += suffix;
                                break;
                            case ReqType.GENERIC:
                                // format: message: %
                                Generic message = JsonUtility.FromJson<Generic>(req.downloadHandler.text);

                                if (!String.IsNullOrEmpty(prefix)) temp += prefix;
                                temp += message.ToString();

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
