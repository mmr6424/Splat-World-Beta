using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    [SerializeField]
    private EiselData _eiselData = new EiselData();

    public bool SaveEiselAsJson()
    {
        try
        {
            string eisel = JsonUtility.ToJson(_eiselData);
            System.IO.File.WriteAllText(Application.persistentDataPath + $"Eisel_{0}_Data", eisel);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

[System.Serializable]
public class EiselData {
    public string title;
    public string crew;
    public string user;
    public List<LineRenderData> lines;
}

[System.Serializable]
public class LineRenderData {
    public int line_placement;
    public Color color;
    public List<float> points;
}