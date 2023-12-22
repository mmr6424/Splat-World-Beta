// Authors: Moss Limpert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;

/// <summary>
/// Type of object to replace with the image retrieved by get request
/// </summary>
public enum TypeToReplace
{
    IMAGE,
    SPRITERENDERER
}

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
    [SerializeField]
    Texture2D texture;
    [SerializeField]
    Texture t;
    [SerializeField]
    int userId;

    // making it usable in different scenarios
    [SerializeField]
    TypeToReplace loadFileInto;
    

    Sprite toReplace;

    public UnityEvent FileDownloaded;

    // Start is called before the first frame update
    void Start()
    {
        throwEvent = false;
        StartCoroutine(GetFile_Coroutine());
        
    }

    /// <summary>
    /// Asks server for a file
    /// </summary>
    /// <returns></returns>
    IEnumerator GetFile_Coroutine()
    {
        // compose url

        string fullUrl = uri + "?download-name=" + "pfp-" +userId + "&id=" + userId;

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(fullUrl, false);
        www.SetRequestHeader("Accept", "image/*");
        yield return www.SendWebRequest();

        //Debug.Log(www.result);

        

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                
                t = DownloadHandlerTexture.GetContent(www);

                //Debug.Log(DownloadHandlerTexture.error)
                //texture = ((DownloadHandlerTexture)www.downloadHandler).texture as Texture2D;
                //byte[] picture = 
                //Debug.Log(texture.ToString());
                texture = t as Texture2D;

                // fix oownldoad handler


                //if (throwEvent) FileDownloaded.Invoke();

                //Debug.Log(www.downloadHandler.isDone);
                //imageToReplace = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                switch (loadFileInto)
                {
                    case TypeToReplace.IMAGE:
                        t = DownloadHandlerTexture.GetContent(www);

                        if (t == null) {
                            Debug.Log("Content Null");
                            break;
                        }

                        texture = t as Texture2D;

                        //Sprite newSprite 
                        imageToReplace.GetComponent<Image>().sprite = Sprite.Create(
                            texture,
                            new Rect(
                                0,
                                0,
                                texture.width,
                                texture.height
                            ),
                            new Vector2(0.5f, 0.5f));

                        // add mask
                        SpriteMask mask = imageToReplace.GetComponent<SpriteMask>();

                        // make mask not interact with other sprites
                        break;
                    case TypeToReplace.SPRITERENDERER:
                        t = DownloadHandlerTexture.GetContent(www);

                        if (t == null)
                        {
                            Debug.Log("Content Null");
                            break;
                        }

                        texture = t as Texture2D;

                        imageToReplace.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
                            texture, 
                            new Rect(
                                0, 
                                0, 
                                texture.width, 
                                texture.height
                            ), 
                            new Vector2(0.5f, 0.5f));
                        break;
                    default:
                        t = DownloadHandlerTexture.GetContent(www);

                        if (t == null)
                        {
                            Debug.Log("Content Null");
                            break;
                        }

                        texture = t as Texture2D;

                        imageToReplace.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
                            texture,
                            new Rect(
                                0,
                                0,
                                texture.width,
                                texture.height
                            ),
                            new Vector2(0.5f, 0.5f));
                        break;
                }
                break;
            default:
                Debug.Log(www.error);
                break;
        }
        
    }

    
}
