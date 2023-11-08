// Authors: Moss Limpert
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UploadFromTexture : MonoBehaviour
{
    //https://www.youtube.com/watch?v=5AUAmZ1002E&t=20s
    public enum ImageType
    {
        PNG,
        JPG
    }

    //
    // FIELDS
    //

    Texture2D imageTexture;
    string fieldName;
    static string username = "moss";
    string fileName = $"testFromUnity";
    ImageType imageType = ImageType.PNG;

    [SerializeField]
    List<InputField> input;                 // input fields. VERY IMPORTANT:
                                            // naming of input fields MUST match json 
                                            // field titles
    List<(string fName, Text value)> args;  // list of tuples...
    [SerializeField]
    string url;
    [SerializeField]
    InputField output;

    // Events
    UnityAction<string> OnErrorAction;
    UnityAction<string> OnCompleteAction;

    // initialize imageuploader gameobject
    //public static UploadFromTexture Initialize ()
    //{
    //    return new GameObject("ImageUploader").AddComponent<UploadFromTexture>();
    //}

    //
    // SETTERS
    //

    // url setter
    public UploadFromTexture SetUrl (string serverUrl)
    {
        this.url = serverUrl;
        return this;
    }

    // texture setter
    public UploadFromTexture SetTexture (Texture2D texture)
    {
        this.imageTexture = texture;
        return this;
    }

    // filename setter
    public UploadFromTexture SetFieldName (string fieldName)
    {
        this.fieldName = fieldName;
        return this;
    }
    
    // type setter
    public UploadFromTexture SetType (ImageType type)
    {
        this.imageType = type;
        return this;
    }

    // onerror setter
    public UploadFromTexture OnError (UnityAction<string> action)
    {
        this.OnErrorAction = action;
        return this;
    }

    // oncomplete setter
    public UploadFromTexture OnComplete (UnityAction<string> action)
    {
        this.OnCompleteAction = action;
        return this;
    }

    // start
    public void Start()
    {
        args = new List<(string fName, Text value)>();

        // fill args (list of tuples)
        for (int i = 0; i < input.Count; i++)
        {
            args.Add((input[i].name, input[i].textComponent));
        }
    }

    /// <summary>
    /// Uploads the image
    /// </summary>
    public void Upload ()
    {
        // input checking
        if (url == null)
        {
            Debug.LogError("no url assigned");
        }

        StopAllCoroutines();
        StartCoroutine(StartUploading());
    }

    // get copy of texture
    Texture2D GetTextureCopy (Texture2D source)
    {
        // create render texture
        RenderTexture rt = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );

        // copy source texture to new render texture
        Graphics.Blit(source, rt);

        // store active rendertexture, activate new one
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        // create new texture 2d and fill pixels in it from rt, apply changes
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        // activate previous rendertexture, release texture created with get temp
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }

    /// <summary>
    /// Coroutine for uploading texture
    /// </summary>
    /// <returns></returns>
    IEnumerator StartUploading ()
    {
        WWWForm form = new WWWForm();
        byte[] textureBytes = null;

        // get a copy, because we cannot access og currently
        Texture2D imgTextureCopy = GetTextureCopy(imageTexture);

        // encode bytes based on imagetype
        switch (imageType)
        {
            case ImageType.PNG:
                textureBytes = imgTextureCopy.EncodeToPNG();
                break;
            case ImageType.JPG:
                textureBytes = imgTextureCopy.EncodeToJPG();
                break;
        }

        // file extension
        string extension = imageType.ToString().ToLower();

        // ADD IMAGE TO FORM
        form.AddBinaryData(fieldName, textureBytes, fileName + "." + extension, "image/" + extension);

        // add form fields from arguments
        if (args.Count > 0)
        {
            for (int i = 0; i < args.Count; i++)
            {
                form.AddField(args[i].fName, args[i].value.text.ToString());
                //Debug.Log(args[i].fName + "," + args[i].value.text.ToString());
            }
        }

        // add boundiary for multer?
        //form.AddBinaryData("test", new byte[0]);

        // declare request
        UnityWebRequest w;

        // https://forum.unity.com/threads/unitywebrequest-post-multipart-form-data-doesnt-append-files-to-itself.627916/
        using (w = UnityWebRequest.Post(url, form)) {
            // add headers
            //w.SetRequestHeader("Content-Type", "multipart/form-data");
            //w.SetRequestHeader("Content-Disposition", "form-data");


            yield return w.SendWebRequest();

            switch (w.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(String.Format("Connection Error: {0}", w.error));
                    output.text = (String.Format("Connection Error: {0}", w.error));
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(String.Format("Protocol Error: {0}", w.error));
                    output.text = String.Format("Protocol Error: {0}", w.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(String.Format("Data Processing Error: {0}", w.error));
                    output.text = String.Format("Data Processing Error: {0}", w.error);
                    break;
                case UnityWebRequest.Result.Success:
                    output.text = w.downloadHandler.text;
                    // success triggers an event, if you set bool throwEvent = true in inspector
                    
                    break;
                default:
                    output.text = String.Format("{0}", w.responseCode);
                    break;
            }

            w.Dispose();
        }
    }
}
