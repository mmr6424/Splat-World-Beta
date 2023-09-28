using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;

public class PaintCanvasRenderer : MonoBehaviour
{
    
    [SerializeField]
    private Camera Camera; // The camera used to render the scene. Used to get the center of the screen.

    [SerializeField]
    public Sprite canvasSprite;

    private Vector3 hitPosition;
    private GameObject eiselObj;
    private SpriteRenderer eisel; // The canvas for the draw tool to draw on!

    /// The types of hit test results to filter against when performing a hit test.
    [EnumFlagAttribute]
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;
    private IARSession _session;

    private void Awake()
    {
        Debug.Log("Awake");
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
    }

    private void Start()
    {
        eiselObj = new GameObject();
        eiselObj.name = "Eisel";
        eiselObj.AddComponent<SpriteRenderer>();

        eisel = eiselObj.GetComponent<SpriteRenderer>();
        eisel.transform.parent = eiselObj.transform;
        eisel.sprite = canvasSprite;
        eisel.spriteSortPoint = SpriteSortPoint.Pivot;
    }

    private void Update() 
    {
        if (_session == null)
        {
            Debug.Log("Update skipped - no AR Session.");
            return;
        }
        if (PlatformAgnosticInput.touchCount <= 0) return;
        var touch = PlatformAgnosticInput.GetTouch(0);
        if (touch.phase == TouchPhase.Began) {
            hitPosition = GetPosition(touch);
            eiselObj.transform.position = hitPosition;
            eisel.transform.position = hitPosition;
        }
        if (touch.phase == TouchPhase.Moved) {
            Vector3 movePosition = GetPosition(touch);
            float dist = Vector3.Distance(hitPosition, movePosition);
            eiselObj.transform.localScale = new Vector3(eiselObj.transform.localScale.x, dist, eiselObj.transform.localScale.z);
            eisel.transform.localScale = new Vector3(eiselObj.transform.localScale.x, dist, eiselObj.transform.localScale.z);

            Vector3 midpoint = (hitPosition + movePosition) / 2f;
            eiselObj.transform.position = midpoint;
            eisel.transform.position = midpoint;

            Vector3 rotation = (movePosition - hitPosition);
            eiselObj.transform.up = rotation;
            eisel.transform.up = rotation;
        }
    }
    /// <summary>
    /// Get position at touch
    /// </summary>
    /// <param name="touch">Touch input</param>
    /// <returns></returns>
    private Vector3 GetPosition(Touch touch)
    {
        var currentFrame = _session.CurrentFrame;
        // if no session return v3 with max float
        if (currentFrame == null)
        {
            Debug.Log("Get Position Failed - No AR Session");
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        var results = currentFrame.HitTest(
            Camera.pixelWidth,
            Camera.pixelHeight,
            touch.position,
            HitTestType
        );

        int count = results.Count;
        Debug.Log("Hit test results: " + count);

        // if no hits return v3 with max float
        if (count <= 0)
        {
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        // get closest result
        var result = results[0];

        // rotate hit position towards camera
        var fresult = result.WorldTransform.ToPosition();

        return fresult;
    }

    /// <summary>
    /// AR Session Initialize
    /// </summary>
    /// <param name="args">AR Session Initialized Args</param>
    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        Debug.Log("AR Session Initialized");
        _session = args.Session;
        _session.Deinitialized += OnSessionDeinitialized;
    }

    /// <summary>
    /// AR Session DE-Initialize
    /// </summary>
    /// <param name="args">AR Session DE-Initialized Args</param>
    private void OnSessionDeinitialized(ARSessionDeinitializedArgs args)
    {
        _session = null;
    }
    
    private void OnDestroy()
    {
        ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;
        _session = null;
    }
}