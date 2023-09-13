using System.Collections;
using System.Collections.Generic;
using System;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine;

public class ARDrawManager : MonoBehaviour
{
    //
    // FIELDS 
    //
    [SerializeField]
    public Camera Camera;

    private IARSession _session;
    
    [EnumFlagAttribute]
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;

    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private int cornerVertices = 5;
    
    [SerializeField]
    private int endCapVertices = 5;
    [SerializeField]
    private bool ifSimplify;
    [SerializeField]
    private float lineWidth;

    private LineRenderer prevLR;        // previous line
    private LineRenderer lineRender;    // new line

    private List<LineRenderer> lines = new List<LineRenderer>();    // list of all lines on canvas (in world space)
    private int posCount = 0;                                       // counts how many positions there are in lines
    private Vector3 distanceToPoint = Vector3.zero;                 // distance between old position and new position

    private bool CanDraw{ get; set; }

    //
    //  METHODS
    //

    // Start is called before the first frame update
    void Start()
    {
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
        Debug.Log("Draw Manager Started");
    }
    // Called when AR Session Initializes
    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        _session = args.Session;
        _session.Deinitialized += OnSessionDeinitialized;
    }
    // Called on AR Session DE-Initialize
    private void OnSessionDeinitialized(ARSessionDeinitializedArgs args)
    {
        _session = null;
    }
    
    private void OnDestroy()
    {
        ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;

        _session = null;
    }

    // Update is called once per frame
    void Update()
    {
        CanDraw = true;
        if (_session == null)
        {
            Debug.Log("Draw Manager Failed -- Session NULL");
            return;
        }

        // if touch input, draw
        if (PlatformAgnosticInput.touchCount > 0)
        {
            DrawOnTouch();
        }
        else 
        {
            prevLR = null;
        }
    }

    // Draw At Touch Location
    public void DrawOnTouch() {
        if (!CanDraw) return;

        // get touch location and test for bad input
        var touch = PlatformAgnosticInput.GetTouch(0);
        Vector3 hitPosition = GetPosition(touch);
        if (hitPosition == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            return;

        // At a new touch, add a line renderer
        if (touch.phase == TouchPhase.Began) {
            AddNewLineRenderer(hitPosition);
        }   // update the line as touch moves
        else if (touch.phase == TouchPhase.Moved) {
            UpdateLine(hitPosition);
        }   // end line at end of touch
        else if (touch.phase == TouchPhase.Ended)
            prevLR = null;
    }

    // Add New Line at Position
    private void AddNewLineRenderer(Vector3 position) {
        // set up line
        posCount = 2;
        GameObject temp = new GameObject($"LineRenderer_{lines.Count}");
        temp.transform.parent = transform ?? Camera.transform;
        temp.transform.position = position;
        LineRenderer tempLineRenderer = temp.AddComponent<LineRenderer>();
        SetLine(tempLineRenderer);
        UnityEngine.Debug.Log("SetLine() called");
        tempLineRenderer.useWorldSpace = true;
        tempLineRenderer.positionCount = posCount;
        tempLineRenderer.SetPosition(0, position);
        tempLineRenderer.SetPosition(1, position);

        // update references to current and previous line renderer
        lineRender = tempLineRenderer;
        prevLR = lineRender;

        // add line renderer to the list of lines
        lines.Add(tempLineRenderer);

        Debug.Log("Successful line creation");
    }
    
    // update current line
    private void UpdateLine(Vector3 position) {
        if (distanceToPoint == null)
            distanceToPoint = position;
        // if the distance to the new position is great enough, add a new point at this position
        if (distanceToPoint != null && Mathf.Abs(Vector3.Distance(distanceToPoint, position)) >= 0.1) {
            distanceToPoint = position;
            AddPoint(distanceToPoint);
        }
    }
    
    // Add point to current line at this position
    private void AddPoint(Vector3 position) {
        posCount++;
        lineRender.positionCount = posCount;
        lineRender.SetPosition(posCount - 1, position);
        if (ifSimplify) lineRender.Simplify(0.1f);
    }

    // Allow or deny drawing
    public void AllowDraw (bool isAllowed) {
        CanDraw = isAllowed;
    }

    // Set current line attributes
    private void SetLine(LineRenderer currentLine) {
        UnityEngine.Debug.Log("Original widths: " + currentLine.startWidth + ", " + currentLine.endWidth);
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        UnityEngine.Debug.Log("New widths: " + currentLine.startWidth + ", " + currentLine.endWidth);
        currentLine.numCornerVertices = cornerVertices;
        currentLine.numCapVertices = endCapVertices;
        if (ifSimplify) currentLine.Simplify(0.1f);
        currentLine.startColor = defaultColor;
        currentLine.endColor = defaultColor;
    }

    // get current touch position
    private Vector3 GetPosition(Touch touch) {
        var currentFrame = _session.CurrentFrame;
        if (currentFrame == null)
        {
            throw new InvalidOperationException("Current Frame does not exist.");
        }

        var results = currentFrame.HitTest
        (
            Camera.pixelWidth,
            Camera.pixelHeight,
            touch.position,
            HitTestType
        );

        int count = results.Count;
        //Debug.Log("Hit test results: " + count);

        if (count <= 0)
           return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        // Get the closest result
        var result = results[0];

        var fresult = result.WorldTransform.ToPosition();

        return fresult;
    }
}
