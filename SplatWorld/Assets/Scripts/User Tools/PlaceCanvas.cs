// Moss Limpert 
// 04/06/2024
using System;
using System.Collections;
using System.Collections.Generic;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;

using UnityEngine;

/// <summary>
/// Allow the user to tap in AR and place a plane
/// consider the words canvas and easel to be equivalent
/// </summary>
public class PlaceCanvas : MonoBehaviour
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

    bool easelPlaced;
    [SerializeField]
    GameObject quad;
    GameObject easel;
    ARHitTestResultType planeOrientation;
    [SerializeField]
    float planeSize;
   
    [SerializeField]
    GameObject tryAgainUI;



    // Start is called before the first frame update
    void Start()
    {
        easelPlaced = false;
        planeSize = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformAgnosticInput.touchCount > 0 && easelPlaced == false)
        {
            PlaceEasel();
        }
    }

    /// <summary>
    /// Places a canvas at the user's touch location
    /// will start out just being a plane. later I'll sub that out
    /// with the actual easel
    /// </summary>
    void PlaceEasel()
    {
        // get touch location
        var touch = PlatformAgnosticInput.GetTouch(0);
        Vector3 hitPosition = GetPosition(touch);
        if (hitPosition == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
            return;

        // find the orientation of the hit test
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
        planeOrientation = HitTestType;

        // place the plane
        // you can find the enum for hit test type here: https://lightship.dev/docs/archive/ardk/api-documentation/enum_Niantic_ARDK_AR_HitTest_ARHitTestResultType.html
        // if its horizontal, place and set the normal to be up
        if (planeOrientation == ARHitTestResultType.EstimatedHorizontalPlane)
        {
            easel = Instantiate(quad, hitPosition, Quaternion.identity);
            //easel.SetNormalAndPosition(Vector3.up, hitPosition);
            
            easelPlaced = true;
        }
        // if its vertical, place and set the normal to be perpendicular to gravity
        else if (planeOrientation == ARHitTestResultType.EstimatedVerticalPlane)
        {

        }
        // otherwise: prompt the user to try again
        else
        {
            tryAgainUI.SetActive(true);
        }

    }

    // get current touch position - copied from ARLineRenderer.cs
    private Vector3 GetPosition(Touch touch)
    {
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
}
