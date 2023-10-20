// Authors: Moss Limpert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

public class GetFileRequest : MonoBehaviour
{
    //
    // FIELDS
    //

    [SerializeField]
    List<(string fName, string value)> args;
    [SerializeField]
    string uri;
    [SerializeField]
    bool throwEvent;
    [SerializeField]
    GameObject imageToReplace;

    public UnityEvent FileDownloaded;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetData_Coroutine());
    }

    /// <summary>
    /// Asks server for a file
    /// </summary>
    /// <returns></returns>
    IEnumerator GetData_Coroutine()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);

        yield return www.SendWebRequest();
        
        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                if (throwEvent) FileDownloaded.Invoke();
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
                imageToReplace.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                break;
            default:
                Debug.Log(www.error);
                break;
        }
        
    }
}
