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
    private float lineWidth = 0.05f;

    private LineRenderer prevLR;
    private LineRenderer lineRender;

    private List<LineRenderer> lines = new List<LineRenderer>();
    private int posCount = 0;
    private Vector3 distanceToPoint = Vector3.zero;

    private bool CanDraw{ get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
        Debug.Log("Draw Manager Started");
    }

    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        _session = args.Session;
        _session.Deinitialized += OnSessionDeinitialized;
    }

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

        if (PlatformAgnosticInput.touchCount > 0)
        {
            DrawOnTouch();
        }
        else
        {
            prevLR = null;
        }
    }

    public void DrawOnTouch() {
        if (!CanDraw) return;

        var touch = PlatformAgnosticInput.GetTouch(0);
        Vector3 hitPosition = GetPosition(touch);
        if (hitPosition == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            return;
        if (touch.phase == TouchPhase.Began) {
            AddNewLineRenderer(hitPosition);
        }
        else if (touch.phase == TouchPhase.Moved) {
            UpdateLine(hitPosition);
        }
        else if (touch.phase == TouchPhase.Ended)
            prevLR = null;
        
    }

    private void AddNewLineRenderer(Vector3 position) {
        posCount = 2;
        GameObject temp = new GameObject($"LineRenderer_{lines.Count}");
        temp.transform.parent = Camera.transform ?? transform;
        temp.transform.position = position;
        LineRenderer tempLineRenderer = temp.AddComponent<LineRenderer>();
        SetLine(tempLineRenderer);
        tempLineRenderer.useWorldSpace = true;
        tempLineRenderer.positionCount = posCount;
        tempLineRenderer.SetPosition(0, position);
        tempLineRenderer.SetPosition(1, position);

        lineRender = tempLineRenderer;
        prevLR = lineRender;

        lines.Add(tempLineRenderer);

        Debug.Log("Successful line creation");
    }
    private void UpdateLine(Vector3 position) {
        if (distanceToPoint == null)
            distanceToPoint = position;
        if (distanceToPoint != null && Mathf.Abs(Vector3.Distance(distanceToPoint, position)) >= 0.1) {
            distanceToPoint = position;
            AddPoint(distanceToPoint);
        }
    }

    private void AddPoint(Vector3 position) {
        posCount++;
        lineRender.positionCount = posCount;
        lineRender.SetPosition(posCount - 1, position);
        if (ifSimplify) lineRender.Simplify(0.1f);
    }

    public void AllowDraw (bool isAllowed) {
        CanDraw = isAllowed;
    }

    private void SetLine(LineRenderer currentLine) {
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        currentLine.numCornerVertices = cornerVertices;
        currentLine.numCapVertices = endCapVertices;
        if (ifSimplify) currentLine.Simplify(0.1f);
        currentLine.startColor = defaultColor;
        currentLine.endColor = defaultColor;
    }

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
