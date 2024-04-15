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
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;

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
        var results = currentFrame.HitTest
        (
            Camera.pixelWidth,
            Camera.pixelHeight,
            touch.position,
            HitTestType
        );
        planeOrientation = HitTestType;

        Debug.Log(planeOrientation);

        Vector3 rayDirection = hitPosition - Camera.transform.position;
        // find normal angle of plane
        Physics.Raycast(Camera.transform.position, rayDirection, out hit, Mathf.Infinity);
        Debug.DrawRay(hitPosition, hit.normal);
        Debug.Log(hit.normal);

        easel = Instantiate(quad, hitPosition, Quaternion.identity);
        e_meshRenderer = easel.GetComponent<MeshRenderer>();
        e_meshRenderer.material = easelMaterial;
        Transform e_obj = easel.gameObject.transform;
        //e_obj.Rotate()
        //e_obj.LookAt(hitPosition + rayDirection);
        
        //e_obj.LookAt(hitPosition);
        e_obj.LookAt(hit.point, hit.transform.TransformDirection(Vector3.forward));

        

        easelPlaced = true;
        //e_obj.LookAt()
        // place the plane
        // you can find the enum for hit test type here: https://lightship.dev/docs/archive/ardk/api-documentation/enum_Niantic_ARDK_AR_HitTest_ARHitTestResultType.html
        // if its horizontal, place and set the normal to be up
        //if (planeOrientation == ARHitTestResultType.EstimatedHorizontalPlane)
        //{
        //    easel = Instantiate(quad, hitPosition, Quaternion.identity);
        //    e_meshRenderer = easel.GetComponent<MeshRenderer>();
        //    e_meshRenderer.material = easelMaterial;
        //    Transform e_obj = easel.gameObject.transform;
        //    e_obj.forward = hit.normal;

        //    easelPlaced = true;
        //}
        //// if its vertical, place and set the normal to be perpendicular to gravity
        //else if (planeOrientation == ARHitTestResultType.EstimatedVerticalPlane)
        //{
        //    easel = Instantiate(quad, hitPosition, Quaternion.identity);
        //    e_meshRenderer = easel.GetComponent<MeshRenderer>();
        //    e_meshRenderer.material = easelMaterial;
        //    Transform e_obj = easel.gameObject.transform;
        //    e_obj.forward = hit.normal;

        //    easelPlaced = true;
        //}
        //// if existing plane...
        //else if (planeOrientation == ARHitTestResultType.ExistingPlane)
        //{
        //    easel = Instantiate(quad, hitPosition, Quaternion.identity);

        //    e_meshRenderer = easel.GetComponent<MeshRenderer>();
        //    e_meshRenderer.material = easelMaterial;

        //    Transform e_obj = easel.gameObject.transform;
        //    e_obj.forward = hit.normal;

        //    easelPlaced = true;
        //}
        // otherwise: prompt the user to try again
        //else
        //{
        //    tryAgainUI.SetActive(true);
        //}



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
