// Moss Limpert 
// 04/06/2024
using System;
using System.Collections;
using System.Collections.Generic;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Anchors;
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
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlaneUsingExtent;

    bool easelPlaced;
    [SerializeField]
    GameObject quad;
    GameObject easel;
    ARHitTestResultType planeOrientation;
    [SerializeField]
    Material easelMaterial;
    MeshRenderer e_meshRenderer;
    RaycastHit hit;
   
    [SerializeField]
    GameObject tryAgainUI;
    [SerializeField]
    GameObject placeCanvasUI;

    //private readonly Dictionary<Guid, GameObject> planeLookup = new Dictionary<Guid, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        easelPlaced = false;
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
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
        // && tryAgainUI.activeSelf == false && placeCanvasUI.activeSelf == false
        // if we havent placed an easel, and the ui isnt showing, and someone touched, then place it
        if (easelPlaced == false && tryAgainUI.activeInHierarchy == false && placeCanvasUI.activeInHierarchy == false && PlatformAgnosticInput.touchCount > 0)
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
        // you can find the enum for hit test type here: 
        // https://lightship.dev/docs/archive/ardk/api-documentation/enum_Niantic_ARDK_AR_HitTest_ARHitTestResultType.html
        var results = currentFrame.HitTest
        (
            Camera.pixelWidth,
            Camera.pixelHeight,
            touch.position,
            HitTestType
        );
        // get information out of hit test
        var anchor = (IARPlaneAnchor) results[0].Anchor;
        var alignment = anchor.Alignment;
        var geometry = anchor.Geometry;
        //Debug.Log(anchor.Transform);
        var hitTestType = results[0].Type;
        planeOrientation = hitTestType;
        //Debug.Log(planeOrientation);


        // place closer to camera in x and z direction
        Vector3 rayDirection = Camera.transform.position - hitPosition;
        hitPosition = hitPosition + new Vector3(rayDirection.x *0.1f, 0, rayDirection.z *0.1f);

        // create easel
        easel = Instantiate(quad, hitPosition, Quaternion.identity);
        e_meshRenderer = easel.GetComponent<MeshRenderer>();
        e_meshRenderer.material = easelMaterial;
        Transform e_obj = easel.gameObject.transform;

        // rotate towards anchor's normal
        e_obj.rotation = anchor.Transform.rotation;
        e_obj.Rotate(new Vector3(90f, 0f, 0f));
        
        easelPlaced = true;
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

    //
    //https://lightship.dev/docs/archive/ardk/ar_world_tracking/plane_detection.html
    //private void AnchorsAdded(AnchorsArgs args)
    //{
    //    foreach (IARPlaneAnchor anchor in args.Anchors)
    //    {
    //        // If the anchor isn't a plane, don't instantiate a GameObject
    //        if (anchor.AnchorType != AnchorType.Plane)
    //            continue;

    //        // Remember this anchor and its GameObject so we can update its position
    //        // if we receive an update.
    //        planeLookup.Add(anchor.Identifier, Instantiate(quad));
    //        var gameObject = planeLookup[anchor.Identifier];

    //        // Display the plane GameObject in the same position, orientation, and scale as the detected plane
    //        gameObject.transform.position = anchor.Transform.ToPosition();
    //        gameObject.transform.rotation = anchor.Transform.ToRotation();
    //        gameObject.transform.localScale = anchor.Extent;
    //    }
    //}

    //void AnchorsUpdated(AnchorsArgs args)
    //{
    //    foreach (IARPlaneAnchor anchor in args.Anchors)
    //    {
    //        GameObject gameObject;
    //        if (planeLookup.TryGetValue(anchor.Identifier, out gameObject))
    //        {
    //            gameObject.transform.position = anchor.Transform.ToPosition();
    //            gameObject.transform.rotation = anchor.Transform.ToRotation();
    //            gameObject.transform.localScale = anchor.Extent;
    //        }
    //    }
    //}
}