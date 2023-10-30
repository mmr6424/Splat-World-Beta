using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

using NativeFilePickerNamespace;
//using SimpleFileb

public class FileSystemUpload : MonoBehaviour
{
    private string fileType;
    string path;
    public RawImage rawImage;
    [SerializeField]
    UploadFromTexture uploadFromTexture;

    void Start()
    {
        fileType = NativeFilePicker.ConvertExtensionToFileType("png"); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        Debug.Log("MIME/UTI is: " + fileType);
        RequestPermissionAsynchronously();
    }

    void Update()
    {
    }

    // Example code doesn't use this function but it is here for reference. It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
        Debug.Log("Permission result: " + permission);
    }


    // opens file explorer, gets a png or jpg
    public void OpenFileExplorer()
    {
#if UNITY_EDITOR
        path = EditorUtility.OpenFilePanel("Show all images (.png)", "", "png");
#endif
#if UNITY_ANDROID
        // if file picker is already being used, dont do anything.
        if (NativeFilePicker.IsFilePickerBusy()) return;

        // Use MIMEs on Android
        string[] fileTypes = new string[] { "image/*" };
#else
    	// Use UTIs on iOS
    	string[] fileTypes = new string[] { "public.image", "public.movie" };
#endif
        byte[] file;

        // Pick image(s) and/or video(s)
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((paths) =>
        {
            // if they didn't pick anything, just tell console we didn't do anything
            if (paths == null) Debug.Log("Operation cancelled");
            // otherwise print the path
            else Debug.Log("Picked file: " + path);

            // import the texture and then set the texture of the ui element using 
            // my upload from texture script
            Texture2D image = ImportTexture(path);
            uploadFromTexture.SetTexture(image);
        }, fileTypes);

        Debug.Log("Permission result: " + permission);
    }

    /// <summary>
    /// Uses file io to import a file chosen with native file picker
    /// and attach the image to a sprite texture
    /// </summary>
    Texture2D ImportTexture(string path)
    {
        byte[] file = File.ReadAllBytes(path);
        Texture2D loadTexture = new Texture2D(1, 1);
        loadTexture.LoadImage(file);
        return loadTexture;
    }

    /// <summary>
    /// Needs to be updated to parameterize
    /// </summary>
    public void ExportFile()
    {
        // Create a dummy text file
        string filePath = Path.Combine(Application.temporaryCachePath, "test.txt");
        File.WriteAllText(filePath, "Hello world!");

        // Export the file
        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(filePath, (success) => Debug.Log("File exported: " + success));

        Debug.Log("Permission result: " + permission);
    }

    // Update is called once per frame
    //IEnumerator GetTexture()
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);


    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        rawImage.texture = texture;

    //    }

    //}
}
