using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamically loads a camera, whose bounds are set to the easel
/// </summary>
public class EaselCamera : MonoBehaviour
{
    //
    // FIELDS
    //

    [SerializeField]
    PlaceCanvas easelScript;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    RenderTexture renderTexture;
    GameObject easel;
    Camera easelCam;

    //
    // ACCESSORS 
    //
    public Camera EaselCam
    {
        get { return easelCam; }
    }

    // Start is called before the first frame update
    void Start()
    {
        easel = easelScript.Easel;
        easelCam = new Camera();

        // have camera render here, so we can show it separately in UI
        //https://blog.theknightsofunity.com/implementing-minimap-unity/
        easelCam.targetTexture = renderTexture; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when user hits save, that way the mini cam is
    // accurate to easel
    // https://gamedev.stackexchange.com/questions/178384/how-to-place-a-camera-so-that-it-always-fits-a-plane
    public void PositionCamera()
    {
        easelCam = mainCamera;

        Bounds bounds = easel.GetComponent<Renderer>().bounds;
        Vector3 objectFrontCenter = bounds.center - easel.transform.forward * bounds.extents.z;

        // get the far side of the triangle by going up 
        // from the center, at a 90 degree angle of the cam's forward vector
        Vector3 triangleFarSideUpAxis = Quaternion.AngleAxis(90, easel.transform.right) * transform.forward;
        // calculate up point of the triangle
        const float MARGIN_MULTIPLIER = 1f;
        Vector3 triangleUpPoint = objectFrontCenter + triangleFarSideUpAxis * bounds.extents.y * MARGIN_MULTIPLIER;

        // The angle between the camera and the top point of the triangle is half the field of view
        // the tangent of this angle equals the length of the opposing triangle side over the desired distance
        // between the camera and the object's front
        float desiredDistance = Vector3.Distance(triangleUpPoint, objectFrontCenter) / Mathf.Tan(Mathf.Deg2Rad * easelCam.GetComponent<Camera>().fieldOfView / 2);

        easelCam.transform.position = -easelCam.transform.forward * desiredDistance + objectFrontCenter;
    }
}
