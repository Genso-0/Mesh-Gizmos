 
using System.Collections.Generic;
using UnityEngine;

namespace Mesh_Gizmos
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ProceduralMesh_RayGizmo : MonoBehaviour
    { 
        [HideInInspector] MeshFilter meshFilter;
        List<Vector3> vertices;
        int[] triangleIndices;
        Vector2[] uv;
        readonly int resolution = 20;  
        [HideInInspector] public Material material;
        void Awake()
        {
            material = GetComponent<MeshRenderer>().material; 
            meshFilter = GetComponent<MeshFilter>();
            GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            var vertCount = (resolution + 2) * 2;
            int indexCount = (((resolution * 3) * 2) + (resolution * 6));
            vertices = new List<Vector3>(vertCount);
            triangleIndices = new int[indexCount];
            uv = new Vector2[vertCount];
        }
        public void Draw(Vector3 start, Vector3 end, Color32 color, float radius)
        {
            BuildMesh_Cylinder(start, end, radius); 
            material.color = color;
        }
        public void BuildMesh_Cylinder(Vector3 start, Vector3 end, float radius)
        {
            vertices.Clear();
            int triIndex = 0;
            BuildCylinderDiscs(start, end, radius, ref triIndex);
            ConnectDiscs(triIndex);
            SetMesh(vertices, triangleIndices, uv);
        }
        public void BuildCylinderDiscs(Vector3 start, Vector3 end, float radius, ref int triIndex)
        {
            GetLocalAxis((end -start).normalized, out Vector3 localRight, out Vector3 localUp);
            float angle = 0.0f;
            float segmentWidth = Mathf.PI * 2f / resolution;
            BuildDisc(localRight, localUp, start, angle, radius, segmentWidth, ref triIndex);
            BuildDisc(localRight, localUp, end, angle, radius, segmentWidth, ref triIndex, true);
        } 
        void BuildDisc(Vector3 localRight, Vector3 localUp, Vector3 offset, float angle, float radius, float segmentWidth, ref int triIndex, bool fliped = false)
        {
            var vertCount = vertices.Count;
            int tri1 = fliped ? 2 : 0;
            int tri2 = 1;
            int tri3 = fliped ? 0 : 2;
            int jCount = fliped ? vertCount - 2 : 0;
            uv[vertCount] = new Vector2(offset.x / radius, offset.y / radius);
            vertices.Add(offset);
            SetDiscVertex(localRight, localUp, offset, segmentWidth, radius, ref angle);

            for (int i = 2; i < resolution + 2; ++i)
            {
                SetDiscVertex(localRight, localUp, offset, segmentWidth, radius, ref angle);

                var j = ((i - 2) + jCount) * 3;
                triangleIndices[j + tri1] = 0 + vertCount;
                triangleIndices[j + tri2] = i - 1 + vertCount;
                triangleIndices[j + tri3] = i + vertCount;
                triIndex += 3;
            }
        }
        void SetDiscVertex(Vector3 axisA, Vector3 axisB, Vector3 offset, float segmentWidth, float radius, ref float angle)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            var point = x * axisA + y * axisB;
            point += offset;
            uv[vertices.Count] = new Vector2(x, y);
            vertices.Add(point);
            angle += segmentWidth;
        }
        void GetLocalAxis(Vector3 forceDirection, out Vector3 localRight, out Vector3 localUp)
        {
            localUp = new Vector3(forceDirection.y, forceDirection.z, forceDirection.x);
            localRight = Vector3.Cross(forceDirection, localUp);
        }
        void ConnectDiscs(int triIndex)
        {
            for (int i = 0; i < resolution; i++)
            {
                triangleIndices[triIndex] = i + 1;
                triangleIndices[triIndex + 1] = i + 2 + resolution + 1;
                triangleIndices[triIndex + 2] = i + 3 + resolution + 1;

                triangleIndices[triIndex + 3] = i + 2;
                triangleIndices[triIndex + 4] = i + 1;
                triangleIndices[triIndex + 5] = i + 3 + resolution + 1;
                triIndex += 6;
            }
        }
        internal void SetMesh(List<Vector3> vertices, int[] tris, Vector2[] uv)
        {
            meshFilter.mesh.Clear();
            meshFilter.mesh.SetVertices(vertices);
            meshFilter.mesh.SetIndices(tris, MeshTopology.Triangles, 0);
            meshFilter.mesh.uv = uv;
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();
        } 
    }
}
