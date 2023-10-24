using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FileSystemUpload : MonoBehaviour
{
    string path;
    public RawImage rawImage;

    // opens file explorer
    public void OpenFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Show all images (.png)", "", "png");
    }

    // Update is called once per frame
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);


        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = texture;

        }

    }
}
