
using System.Collections.Generic;
using UnityEngine;
namespace Mesh_Gizmos
{
    public class MeshGizmos : MonoBehaviour
    {
        #region singleton
        private static MeshGizmos m_Instance;
        public static MeshGizmos Instance
        {
            get
            {
                m_Instance = FindObjectOfType<MeshGizmos>();
                if (m_Instance == null)
                {
                    Debug.LogError("Instance not in scene!");
                }
                else if (!m_Instance.initialized)
                {
                    m_Instance.Init();
                }
                return m_Instance;
            }
        }
        public bool initialized { get; private set; }
        #endregion
        public Material material_main;
        public ProceduralMesh_RayGizmo prefab_ray;
        public GameObject prefab_arrowHead;
        public GameObject prefab_cube;
        public GameObject prefab_sphere;
        List<ProceduralMesh_RayGizmo> gizmos_rays;
        List<GameObject> gizmos_arrowHeads;
        List<GameObject> gizmos_cubes;
        List<GameObject> gizmos_spheres;
        Queue<RayCommandData> buffer_rays;
        Queue<ShapeCommandData> buffer_arrowHeads;
        Queue<ShapeCommandData> buffer_cubes;
        Queue<ShapeCommandData> buffer_spheres;

        bool needsClearing;
        public bool active;
        void Init()
        {
            if (!initialized)
            {
                initialized = true;
                gizmos_rays = new List<ProceduralMesh_RayGizmo>();
                gizmos_arrowHeads = new List<GameObject>();
                gizmos_cubes = new List<GameObject>();
                gizmos_spheres = new List<GameObject>();
                buffer_rays = new Queue<RayCommandData>();
                buffer_arrowHeads = new Queue<ShapeCommandData>();
                buffer_cubes = new Queue<ShapeCommandData>();
                buffer_spheres = new Queue<ShapeCommandData>();
            }
        }
        void Update()
        {
            if (initialized)
            {
                if (needsClearing)
                    Clear();
                if (active)
                    Draw();
                else if (buffer_rays.Count > 0)
                    PurgeBuffers();
            }
        }
        void OnApplicationQuit()
        {
            if (initialized)
                PurgeBuffers();
        }
        public void DrawRay(Vector3 origin, Vector3 direction, Color32 color, float radius = 0.01f)
        {
            var start = origin - transform.position;
            buffer_rays.Enqueue(new RayCommandData(start, start + direction, color, radius));
        }
        public void DrawLine(Vector3 origin, Vector3 end, Color32 color, float radius = 0.01f)
        {
            buffer_rays.Enqueue(new RayCommandData(origin - transform.position, end - transform.position, color, radius));
        }
        public void DrawArrowHead(Vector3 position, Vector3 direction, Color32 color, float scale = 0.1f)
        {
            buffer_arrowHeads.Enqueue(new ShapeCommandData(position, direction, color, scale));
        }
        public void DrawCube(Vector3 position, Vector3 direction, Color32 color, float scale = 0.1f)
        {
            buffer_cubes.Enqueue(new ShapeCommandData(position, direction, color, scale));
        }
        public void DrawSphere(Vector3 position, Vector3 direction, Color32 color, float scale = 0.1f)
        {
            buffer_spheres.Enqueue(new ShapeCommandData(position, direction, color, scale));
        }
        private void Draw()
        {
            int index = 0; 
            while (gizmos_rays.Count < buffer_rays.Count)
                AddInstance_Ray();
            while (gizmos_arrowHeads.Count < buffer_arrowHeads.Count)
                AddInstance_ArrowHead();
            while (gizmos_cubes.Count < buffer_cubes.Count)
                AddInstance_Cube();
            while (gizmos_spheres.Count < buffer_spheres.Count)
                AddInstance_Sphere();
            while (buffer_rays.Count > 0)
            {
                buffer_rays.Dequeue().Draw(gizmos_rays[index]);
                index++;
            }
            index = 0;
            while (buffer_arrowHeads.Count > 0)
            {
                buffer_arrowHeads.Dequeue().Draw(gizmos_arrowHeads[index]);
                index++;
            }
            index = 0;
            while (buffer_cubes.Count > 0)
            {
                buffer_cubes.Dequeue().Draw(gizmos_cubes[index]);
                index++;
            }
            index = 0;
            while (buffer_spheres.Count > 0)
            {
                buffer_spheres.Dequeue().Draw(gizmos_spheres[index]);
                index++;
            }
            needsClearing = true;
        }
        private void Clear()
        {
            foreach (var item in gizmos_rays)
                item.gameObject.SetActive(false);
            foreach (var item in gizmos_arrowHeads)
                item.gameObject.SetActive(false);
            foreach (var item in gizmos_cubes)
                item.gameObject.SetActive(false);
            foreach (var item in gizmos_spheres)
                item.gameObject.SetActive(false);
            needsClearing = false;
        }
        private void PurgeBuffers()
        {
            while (buffer_rays.Count > 0) buffer_rays.Dequeue();
            while (buffer_arrowHeads.Count > 0) buffer_arrowHeads.Dequeue();
            while (buffer_cubes.Count > 0) buffer_cubes.Dequeue();
            while (buffer_spheres.Count > 0) buffer_spheres.Dequeue();
        }

        void AddInstance_Ray()
        {
            var instance = Instantiate(prefab_ray, transform);
            instance.material = instance.GetComponent<MeshRenderer>().material; 
            instance.material.CopyPropertiesFromMaterial(material_main);
            gizmos_rays.Add(instance);
        }
        void AddInstance_ArrowHead()
        {
            var instance = Instantiate(prefab_arrowHead, transform);
            instance.GetComponent<MeshRenderer>().material.CopyPropertiesFromMaterial(material_main);
            gizmos_arrowHeads.Add(instance);
        }
        void AddInstance_Cube()
        {
            var instance = Instantiate(prefab_cube, transform);
            instance.GetComponent<MeshRenderer>().material.CopyPropertiesFromMaterial(material_main); 
            gizmos_cubes.Add(instance);
        }
        void AddInstance_Sphere()
        {
            var instance = Instantiate(prefab_sphere, transform);
            instance.GetComponent<MeshRenderer>().material.CopyPropertiesFromMaterial(material_main);
            gizmos_spheres.Add(instance);
        }
        readonly struct RayCommandData
        {
            public RayCommandData(Vector3 startPosition, Vector3 dir, Color32 color, float radius)
            {
                this.startPosition = startPosition;
                this.dir = dir;
                this.color = color;
                this.radius = radius;
            }
            public readonly Vector3 startPosition;
            public readonly Vector3 dir;
            public readonly Color32 color;
            public readonly float radius;

            public void Draw(ProceduralMesh_RayGizmo gizmo)
            {
                gizmo.gameObject.SetActive(true);
                gizmo.Draw(startPosition, dir, color, radius);
            }
        }
        readonly struct ShapeCommandData
        {
            public ShapeCommandData(Vector3 position, Vector3 dir, Color32 color, float scale)
            {
                this.startPosition = position;
                this.dir = dir;
                this.color = color;
                this.scale = scale;
            }
            public readonly Vector3 startPosition;
            public readonly Vector3 dir;
            public readonly Color32 color;
            public readonly float scale;

            public void Draw(GameObject gizmo)
            {
                gizmo.gameObject.SetActive(true);
                gizmo.transform.localScale = Vector3.one * scale;
                gizmo.transform.position = startPosition;
                gizmo.GetComponent<MeshRenderer>().material.color = color;
                if (dir != Vector3.zero)
                {
                    gizmo.transform.rotation = Quaternion.LookRotation(dir);
                }
            }
        }
    }
}