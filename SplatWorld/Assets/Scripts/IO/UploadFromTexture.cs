using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    string url;

    // Events
    UnityAction<string> OnErrorAction;
    UnityAction<string> OnCompleteAction;

    // initialize imageuploader gameobject
    public static UploadFromTexture Initialize ()
    {
        return new GameObject("ImageUploader").AddComponent<UploadFromTexture>();
    }

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

        // add form to request
        WWW w = new WWW(url, form);

        yield return w;

        // handle response
        if (w.error != null)
        {
            if (OnErrorAction != null)
            {
                OnErrorAction(w.error);
            }
        } else
        {
            if (OnCompleteAction != null)
            {
                OnCompleteAction(w.text);
            }
        }

        w.Dispose();
        Destroy(this.gameObject);
    }


}
