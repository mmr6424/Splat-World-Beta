using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities;

public class PaintCanvasRenderer : MonoBehaviour
{
    
    [SerializeField]
    private Camera Camera; // The camera used to render the scene. Used to get the center of the screen.

    [SerializeField]
    private GameObject cursorObject; // The object we will place to represent the cursor!
    [SerializeField]
    public Sprite canvasSprite;

    private GameObject eiselObj;
    private Canvas eisel; // The canvas for the draw tool to draw on!

    private GameObject _spawnedcursorObject; // A reference to the spawned cursor in the center of the screen.

    private IARSession _session;

    private void Start()
    {
        ARSessionFactory.SessionInitialized += _SessionInitialized;

        eiselObj = new GameObject();
        eiselObj.name = "Eisel";
        eiselObj.AddComponent<Canvas>();

        eisel = eiselObj.GetComponent<Canvas>();
        eiselObj.AddComponent<CanvasScaler>();
        eiselObj.AddComponent<GraphicRaycaster>();
    }

    private void OnDestroy()
    {
        ARSessionFactory.SessionInitialized -= _SessionInitialized;

        var session = _session;
        if (session != null)
        session.FrameUpdated -= _FrameUpdated;

        DestroySpawnedCursor();
    }

    private void DestroySpawnedCursor()
    {
        if (_spawnedcursorObject == null)
        return;

        Destroy(_spawnedcursorObject);
        _spawnedcursorObject = null;
    }

    private void _SessionInitialized(AnyARSessionInitializedArgs args)
    {
        var oldSession = _session;
        if (oldSession != null)
        oldSession.FrameUpdated -= _FrameUpdated;

        var newSession = args.Session;
        _session = newSession;
        newSession.FrameUpdated += _FrameUpdated;
        newSession.Deinitialized += _OnSessionDeinitialized;
    }

    private void _OnSessionDeinitialized(ARSessionDeinitializedArgs args)
    {
        DestroySpawnedCursor();
    }

    private void Update() {

    }

    private void _FrameUpdated(FrameUpdatedArgs args)
    {
        var camera = Camera;
        if (camera == null)
        return;

        var viewportWidth = camera.pixelWidth;
        var viewportHeight = camera.pixelHeight;

        // Hit testing for cursor in the middle of the screen
        var middle = new Vector2(viewportWidth / 2f, viewportHeight / 2f);

        var frame = args.Frame;
        // Perform a hit test and either estimate a horizontal plane, or use an existing plane and its
        // extents!
        var hitTestResults =
        frame.HitTest
        (
            viewportWidth,
            viewportHeight,
            middle,
            ARHitTestResultType.ExistingPlaneUsingExtent |
            ARHitTestResultType.EstimatedHorizontalPlane
        );

        if (hitTestResults.Count == 0)
        return;

        if (_spawnedcursorObject == null)
        _spawnedcursorObject = Instantiate(cursorObject, Vector2.one, Quaternion.identity);

        // Set the cursor object to the hit test result's position
        _spawnedcursorObject.transform.position = hitTestResults[0].WorldTransform.ToPosition();

        // Orient the cursor object to look at the user, but remain flat on the "ground", aka
        // only rotate about the y-axis
        _spawnedcursorObject.transform.LookAt
        (
        new Vector3
        (
            frame.Camera.Transform[0, 3],
            _spawnedcursorObject.transform.position.y,
            frame.Camera.Transform[2, 3]
        )
        );
    }
}