/*
 * Rose M. Rushton
 * Last Edit: 04/17/2024
 */
using System.Collections.Generic;
using System;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine;

public enum Tool
{
    Brush,
    Can,
    Chalk
}
public class ARLineRenderer : MonoBehaviour
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
    private int cornerVertices;
    
    [SerializeField]
    private int endCapVertices;
    [SerializeField]
    private bool ifSimplify;
    [SerializeField]
    private float startLineWidth = 0.02f;
    [SerializeField] 
    private float endLineWidth = 0.02f;

    [SerializeField] private Material mBrush, mCan, mChalk;

    private LineRenderer prevLR;        // previous line
    private LineRenderer lineRender;    // new line

    private List<LineRenderer> lines = new List<LineRenderer>();    // list of all lines on canvas (in world space)
    private int posCount = 0;                                       // counts how many positions there are in lines
    private Vector3 distanceToPoint = Vector3.zero;                 // distance between old position and new position

    private bool CanDraw{ get; set; }
    
    private Tool currentTool;

    //
    //  METHODS
    //

    // Start is called before the first frame update
    void Start()
    {
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
        AddNewLineRenderer(Vector3.zero);
        CanDraw = true;
        ChangeToolType(0);
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
        //CanDraw = true;
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
    }

    // Draw At Touch Location
    public void DrawOnTouch() {
        if (!CanDraw) return;

        // get touch location and test for bad input
        var touch = PlatformAgnosticInput.GetTouch(0);
        Vector3 hitPosition = GetPosition(touch);
        if (hitPosition == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            return;
        // Currently causes an issue wherein it creates an extra line renderer upon changing settings, not fixed yet
        if (lineRender == prevLR)
            AddNewLineRenderer(hitPosition);
        // At a new touch, add a line renderer
        if (touch.phase == TouchPhase.Began) {
            //AddNewLineRenderer(hitPosition);
            AddFirstPoint(hitPosition);
        }   // update the line as touch moves
        else if (touch.phase == TouchPhase.Moved) {
            UpdateLine(hitPosition);
        }   // end line at end of touch
        else if (touch.phase == TouchPhase.Ended)
        {
            prevLR = lineRender;
            //AddNewLineRenderer(hitPosition);
        }
    }

    // Add New Line at Position
    private void AddNewLineRenderer(Vector3 position) {
        // set up line
        posCount = 2;
        // Create a new obj to attach LineRenderer to
        GameObject temp = new GameObject($"LineRenderer_{lines.Count}");
        // Set the transform parent to current object script is attached to OR camera if current object cannot be attached
        temp.transform.parent = Camera.transform;
        // set start position
        temp.transform.position = position;
        // add the LR
        LineRenderer tempLineRenderer = temp.AddComponent<LineRenderer>();
        // Settings for the line
        SetLine(tempLineRenderer);
        Debug.Log("SetLine() called");
        // Make sure the lineRenderer is using world space
        tempLineRenderer.useWorldSpace = true;
        
        // Add two start points in order to draw line on click
       /* tempLineRenderer.positionCount = posCount;
        tempLineRenderer.SetPosition(0, position);
        tempLineRenderer.SetPosition(1, position);*/
        tempLineRenderer.transform.LookAt(Camera.transform);

        // update references to current and previous line renderer
        prevLR = lineRender;
        lineRender = tempLineRenderer;

        // add line renderer to the list of lines
        lines.Add(lineRender);

        Debug.Log("Successful line creation");
    }
    
    // update current line
    private void UpdateLine(Vector3 position) {
        // if this is the first click
        if (distanceToPoint == Vector3.zero)
        {
            distanceToPoint = position;
            // this multiplication ensures that the object will not clip into the wall at any points
            //distanceToPoint.z *= 1.01f; // clips the line into the wall sometimes -- do not use
            Debug.Log("Updating Line - distanceToPoint = position");
        }
        // if the distance to the new position is great enough, add a new point at this position
        if (distanceToPoint != Vector3.zero && Mathf.Abs(Vector3.Distance(distanceToPoint, position)) >= 0.01) {
            distanceToPoint = position;
            // see above
            //distanceToPoint.z *= 1.01f; // clips the line into the wall sometimes -- do not use
            // Add the new point to the line renderer
            AddPoint(distanceToPoint);
        }
    }

    private void AddFirstPoint(Vector3 position)
    {
        // Add two start points in order to draw line on click
        lineRender.positionCount = posCount;
        lineRender.SetPosition(0, position);
        lineRender.SetPosition(1, position);
        lineRender.transform.LookAt(Camera.transform);
        if (ifSimplify) lineRender.Simplify(0.1f);
        SetLine(lineRender);
    }
    
    // Add point to current line at this position
    private void AddPoint(Vector3 position) {
        posCount++; // increment counter
        lineRender.positionCount = posCount;
        lineRender.SetPosition(posCount - 1, position);
        lineRender.transform.LookAt(Camera.transform);
        if (ifSimplify) lineRender.Simplify(0.1f);
        Debug.Log("Adding Point");
    }

    // Allow or deny drawing
    public void AllowDraw (bool isAllowed) {
        CanDraw = isAllowed;
    }

    public void ChangeToolType(int type)
    {
        currentTool = (Tool)type;
    }

    // Set current line attributes
    private void SetLine(LineRenderer currentLine) {
        Debug.Log("Original widths: " + currentLine.startWidth + ", " + currentLine.endWidth);
        // Set the width of the line. This should pretty much always be between 0 and 0.1 for a reasonable line size!
        currentLine.startWidth = startLineWidth;
        currentLine.endWidth = endLineWidth;
        Debug.Log("New widths: " + currentLine.startWidth + ", " + currentLine.endWidth);
        // Smoothing
        currentLine.numCornerVertices = cornerVertices;
        currentLine.numCapVertices = endCapVertices;
        //if (ifSimplify) currentLine.Simplify(0.1f);
        // Sets it to important basically
        currentLine.sortingOrder = 1;
        // Creates a new material!!!
        //currentLine.material = new Material(Shader.Find("Sprites/Default"));
        switch (currentTool)
        {
            default:
                currentLine.material = new Material(Shader.Find("Sprites/Default"));
                Console.WriteLine("TOOL WAS NOT PROPERLY SELECTED!");
                break;
            case Tool.Brush:
                currentLine.material = mBrush;
                break;
            case Tool.Can:
                currentLine.material = mCan;
                break;
            case Tool.Chalk:
                currentLine.material = mChalk;
                break;
        }
        // Set the colors
        currentLine.material.color = defaultColor;
        currentLine.startColor = defaultColor;
        currentLine.endColor = defaultColor;
        currentLine.alignment = LineAlignment.TransformZ; // REQUIRED TO TURN OFF BILLBOARDING
    }

    // get current touch position
    private Vector3 GetPosition(Touch touch) {
        var currentFrame = _session.CurrentFrame;
        if (currentFrame == null)
        {
            throw new InvalidOperationException("Current Frame does not exist.");
        }

        // Grab the hit-test
        var results = currentFrame.HitTest
        (
            Camera.pixelWidth,
            Camera.pixelHeight,
            touch.position,
            HitTestType
        );

        int count = results.Count; // how many touches we got
        //Debug.Log("Hit test results: " + count);

        if (count <= 0)
           return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        // Get the closest result
        var result = results[0];

        // Transform to the world space
        var fresult = result.WorldTransform.ToPosition();

        return fresult;
    }


    //////////////////////////////////////////////
    /// <-- CHANGE LINE ATTRIBUTES SECTION --> ///
    //////////////////////////////////////////////
    public void ChangeColor(Color color) {
        defaultColor = color;
        //SetLine(lineRender);
    }

    public void ChangeLineWidth(float size)
    {
        startLineWidth = size;
        endLineWidth = size;
        //SetLine(lineRender);
    }
}
