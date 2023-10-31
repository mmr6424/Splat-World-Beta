using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

using NativeFilePickerNamespace;

public class FileSystemUpload : MonoBehaviour
{
    string path;
    Texture2D image;

    [SerializeField]
    RawImage rawImage;
    [SerializeField]
    UploadFromTexture uploadFromTexture;

    void Start()
    {
#if UNITY_ANDROID
        RequestPermissionAsynchronously();
#endif
    }

    void Update()
    {
    }

    //  It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
        Debug.Log("Permission result: " + permission);
    }


    // opens file explorer, gets a png or jpg
    public void OpenFileExplorer()
    {
        Debug.Log("Inside Open File Explorer");
#if UNITY_EDITOR
        path = EditorUtility.OpenFilePanel("Show all images (.png)", "", "png");

        // import the texture and then set the texture of the ui element using 
        // my upload from texture script
        image = ImportTexture(path);
        rawImage.texture = image;


#elif UNITY_ANDROID

        // if file picker is already being used, dont do anything.
        if (NativeFilePicker.IsFilePickerBusy()) return;

        // Use MIMEs on Android
        string[] fileTypes = new string[] { "image/*" };

        // Pick image(s) and/or video(s)
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            // if they didn't pick anything, just tell console we didn't do anything
            if (path == null) Debug.Log("Operation cancelled");
            // otherwise print the path
            else Debug.Log("Picked file: " + path);

            // import the texture and then set the texture of the ui element using 
            // my upload from texture script
            image = ImportTexture(path);
            rawImage.texture = image;

        }, fileTypes);

        Debug.Log("Permission result: " + permission);

#elif UNITY_IOS

        // if file picker is already being used, dont do anything.
        if (NativeFilePicker.IsFilePickerBusy()) return;

    	// Use UTIs on iOS
    	string[] fileTypes = new string[] { "public.image" };

        // Pick image(s) and/or video(s)
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            // if they didn't pick anything, just tell console we didn't do anything
            if (path == null) Debug.Log("Operation cancelled");
            // otherwise print the path
            else Debug.Log("Picked file: " + path);

            // import the texture and then set the texture of the ui element using 
            // my upload from texture script
            image = ImportTexture(path);
            rawImage.texture = image;

        }, fileTypes);

        Debug.Log("Permission result: " + permission);
#endif

        Debug.Log("after directive if statements");

        uploadFromTexture.SetFieldName("image");
        uploadFromTexture.SetTexture(image);
        uploadFromTexture.Upload();

    }

    /// <summary>
    /// Uses file io to import a file chosen with native file picker
    /// and attach the image to a sprite texture
    /// </summary>
    Texture2D ImportTexture(string path)
    {
        Debug.Log("Inside import texture");
        byte[] file = File.ReadAllBytes(path);
        //Debug.Log(file.ToString());
        Texture2D loadTexture = new Texture2D(1, 1);
        loadTexture.LoadImage(file);
        return loadTexture;
    }

    /// <summary>
    /// Needs to be updated to parameterize
    /// writes to local file system
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
    

}
