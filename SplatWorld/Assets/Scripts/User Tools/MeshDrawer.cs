// Charles Begle and Moss Limpert

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;

/// <summary>
/// Creates a mesh line from touch input
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshDrawer : MonoBehaviour
{
    //
    // FIELDS
    //
    [SerializeField]
    public Camera Camera;
    private Mesh prevMesh;
    private Mesh mesh;
    private Vector3 lastMousePos;
    public Vector3 hitPosition;
    public Vector3 distanceToPoint = Vector3.zero;
    public float lineThickness;
    public float minDistance;

    // AR
    /// The types of hit test results to filter against when performing a hit test.
    [EnumFlagAttribute]
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;
    /// Internal reference to the session, used to get the current frame to hit test against.
    private IARSession _session;

    // Debugging
    public Transform debugVisual1;
    public Transform debugVisual2;

    // Multiple lines
    private List<Mesh> lines = new List<Mesh>();
    [SerializeField]
    private GameObject lineHolder;
    [SerializeField]
    private MeshFilter targetMeshFilter;
    [SerializeField]
    private Material lineMaterial;
    //private List<MeshFilter> meshFilters;
    //private MeshFilter[] meshFilters;

    //
    // METHODS
    //

    private void Awake()
    {
        Debug.Log("Awake");
        lineThickness = 0.01f;
        minDistance = 0.01f;
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
    }

    private void Start()
    {
        prevMesh = null;
        mesh = new Mesh();
        //meshFilters = new List<MeshFilter>();
        
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
        mesh = null;
        prevMesh = null;
    }

    private void Update()
    {
        // if no AR session or no touch, return
        if (_session == null)
        {
            UnityEngine.Debug.Log("Update skipped - no AR Session.");
            return;
        }
        if (PlatformAgnosticInput.touchCount <= 0) return;

        // if touch input, draw
        if (Input.GetMouseButton(0) || PlatformAgnosticInput.touchCount > 0)
        {
            DrawOnTouch();
        }

        //else mesh = null;

        }

    /// <summary>
    /// Draw at touch location
    /// </summary>
    public void DrawOnTouch()
    {
        //Debug.Log("Draw on Touch called");

        // touch positioning
        var touch = PlatformAgnosticInput.GetTouch(0);
        hitPosition = GetPosition(touch);

        // if we have a no-hit condition, return
        if (hitPosition == new Vector3(float.MaxValue, float.MaxValue, float.MaxValue)) return;

        // if the mouse button just went down, make a new mesh
        if (Input.GetMouseButtonDown(0) || touch.phase == TouchPhase.Began)
        {
            StartDraw();
        }
        // if the mouse button is currently being pressed
        if (Input.GetMouseButton(0) || touch.phase == TouchPhase.Moved)
        {
            Draw();
        }
        // if mouse button went up
        else if (Input.GetMouseButtonUp(0) || touch.phase == TouchPhase.Ended)
        {
            // combine this stroke with the rest
            CombineMeshes();
            
            // update references 
            prevMesh = mesh;
        }
    }

    /// <summary>
    /// Combine Line meshes
    /// https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
    /// </summary>
    private void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        mesh.CombineMeshes(combine, false, false);
        targetMeshFilter.mesh = mesh;
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
            UnityEngine.Debug.Log("Get Position Failed - No AR Session");
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
    /// Begins mesh drawing at first input
    /// </summary>
    private void StartDraw()
    {
        //Debug.Log("Start Draw called");
        Mesh tempMesh = new Mesh();
        

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        // vertices
        vertices[0] = hitPosition;
        vertices[1] = hitPosition;
        vertices[2] = hitPosition;
        vertices[3] = hitPosition;

        // uv coordinates
        // texture is plain solid color
        uv[0] = Vector2.zero;
        uv[1] = Vector2.zero;
        uv[2] = Vector2.zero;
        uv[3] = Vector2.zero;

        // indexes of the vertices that make up each polygon
        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 1;

        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        tempMesh.vertices = vertices;
        tempMesh.uv = uv;
        tempMesh.triangles = triangles;
        tempMesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = tempMesh;

        mesh = tempMesh;

        lastMousePos = hitPosition;

        // LIST
        GameObject temp = new GameObject();
        temp.AddComponent<MeshRenderer>().material = lineMaterial;
        MeshFilter tempFilter = temp.AddComponent<MeshFilter>();
        tempFilter.sharedMesh = mesh;
        temp.transform.SetParent(lineHolder.transform, true);

        Debug.Log("Successful mesh creation");
    }

    /// <summary>
    /// Continues mesh drawing while input is still given
    /// </summary>
    private void Draw()
    {
        var currentFrame = _session.CurrentFrame;
        // if no current AR session, return
        if (currentFrame == null) return;

        // if the new position is far enough away from the last position, extend mesh
        if (Vector3.Distance(hitPosition, lastMousePos) > minDistance)
        {
            // allocate new arrays
            Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
            Vector2[] uv = new Vector2[mesh.uv.Length + 2];
            int[] triangles = new int[mesh.triangles.Length + 6];

            // copy arrays over
            mesh.vertices.CopyTo(vertices, 0);
            mesh.uv.CopyTo(uv, 0);
            mesh.triangles.CopyTo(triangles, 0);

            // get indexes for the new array spots
            int vIndex = vertices.Length - 4;
            int vIndex0 = vIndex + 0;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            Vector3 mouseForwardVector = (hitPosition - lastMousePos).normalized;
            // making normal towards cam
            Vector3 normal2D = Camera.transform.forward;
            //normal2D = Vector3.Cross(normal2D, Camera.transform.forward);
            //Vector3 normal2D = new Vector3(0, 0, -1f);
            Vector3 newVertexUp = hitPosition + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
            Vector3 newVertexDown = hitPosition + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;



            //debugVisual1.position = newVertexUp;
            //debugVisual2.position = newVertexDown;

            vertices[vIndex2] = newVertexUp;
            vertices[vIndex3] = newVertexDown;

            uv[vIndex2] = Vector2.zero;
            uv[vIndex3] = Vector2.zero;

            int tIndex = triangles.Length - 6;

            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex2;
            triangles[tIndex + 2] = vIndex1;

            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex2;
            triangles[tIndex + 5] = vIndex3;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            lastMousePos = hitPosition;
        }
    }
}
