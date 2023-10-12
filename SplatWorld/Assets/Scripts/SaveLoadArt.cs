using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadArt : MonoBehaviour
{

    List<GameObject> loadedArt;
    // Start is called before the first frame update
    void Start()    
    {
        loadedArt = new List<GameObject>();
        LoadArt();
    }

    bool SaveArt()
    {
        // Grab gameObject MeshFilter
        // Convert object to JSON
        // Add JSON to server
        // return true if successful
        // return false if one or more errors occurred
        return false;
    }

    bool LoadArt()
    {
        // Grab JSON from server
        // Convert JSON to objects
        // Add objects to loadedArt list
        // return true if successful
        // return false if one or more errors occurred
        return false;
    }
}
