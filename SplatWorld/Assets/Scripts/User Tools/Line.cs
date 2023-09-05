using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;

/// 
/// Credit to BMo on Youtube for his drawing tool: https://www.youtube.com/watch?v=D0ENg1dQN64
///

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    List<Vector2> points;

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }

        if (Vector2.Distance(points.Last(), position) > .1f)
        {
            SetPoint(position);
        }

    }
    public void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
