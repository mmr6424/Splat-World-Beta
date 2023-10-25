using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
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