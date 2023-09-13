using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrawer : MonoBehaviour
{
    private Mesh mesh;
    private Vector3 lastMousePos;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public float lineThickness;
    public float minDistance;

    public Transform debugVisual1;
    public Transform debugVisual2;


    private void Awake()
    {
        lineThickness = 0.01f;
        minDistance = 0.01f;
    }

    private void Update()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //transform.position = worldPosition;

        // if the mouse button just went down, make a new mesh
        if (Input.GetMouseButtonDown(0))
        {
            mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            // vertices
            vertices[0] = worldPosition;
            vertices[1] = worldPosition;
            vertices[2] = worldPosition;
            vertices[3] = worldPosition;

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

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.MarkDynamic();

            GetComponent<MeshFilter>().mesh = mesh;

            lastMousePos = worldPosition;
        }
        // if the mouse button is currently being pressed
        if (Input.GetMouseButton(0))
        {
            // if the new position is far enough away from the last position, extend mesh
            if (Vector3.Distance(worldPosition, lastMousePos) > minDistance)
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

                Vector3 mouseForwardVector = (worldPosition - lastMousePos).normalized;
                Vector3 normal2D = new Vector3(0, 0, -1f);
                Vector3 newVertexUp = worldPosition + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
                Vector3 newVertexDown = worldPosition + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

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

                lastMousePos = worldPosition;
            }
        }
    }
}
