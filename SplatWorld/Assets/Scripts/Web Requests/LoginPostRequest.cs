// Author: Moss Limpert

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class LoginPostRequest : MonoBehaviour
{
    //string username;
    //string password;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    ///// <summary>
    ///// Post Data to the server
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator PostData_Coroutine()
    //{
    //    output.text = "Loading...";

    //    WWWForm form = new WWWForm();

    //    if (args.Count > 0)
    //    {
    //        for (int i = 0; i < args.Count; i++)
    //        {
    //            form.AddField(args[i].fName, args[i].value.text.ToString());
    //            //Debug.Log(args[i].fName + "," + args[i].value.text.ToString());
    //        }
    //    }

    //    UnityWebRequest req;

    //    using (req = UnityWebRequest.Post(uri, form))
    //    {
    //        yield return req.SendWebRequest();

    //        switch (req.result)
    //        {
    //            case UnityWebRequest.Result.ConnectionError:
    //                Debug.LogError(String.Format("Connection Error: {0}", req.error));
    //                output.text = (String.Format("Connection Error: {0}", req.error));
    //                break;
    //            case UnityWebRequest.Result.ProtocolError:
    //                Debug.LogError(String.Format("Protocol Error: {0}", req.error));
    //                output.text = String.Format("Protocol Error: {0}", req.error);
    //                break;
    //            case UnityWebRequest.Result.DataProcessingError:
    //                Debug.LogError(String.Format("Data Processing Error: {0}", req.error));
    //                output.text = String.Format("Data Processing Error: {0}", req.error);
    //                break;
    //            case UnityWebRequest.Result.Success:
    //                output.text = req.downloadHandler.text;
    //                // success triggers an event, if you set bool throwEvent = true in inspector
    //                if (throwEvent)
    //                {
    //                    RequestSucceeded.Invoke();
    //                }
    //                break;
    //            default:
    //                output.text = String.Format("{0}", req.responseCode);
    //                break;
    //        }
    //    }
    //}

    /// <summary>
    /// Starts coroutine to post data to server
    /// </summary>
    //public void PostData() => StartCoroutine(PostData_Coroutine());
}
