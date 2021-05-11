
using System;
using UnityEngine;
namespace Mesh_Gizmos
{
    public class ExampleGizmoCalls : MonoBehaviour
    {
        MeshGizmos _Gizmos;
        public Vector3 position_cubeGizmo;
        public Vector3 position_sphereGizmo;
        public Vector3 position_arrowGizmo;
        [SerializeField] LayerMask layerMask = 1;
        LayerMask previous_LayerMask;
        LayerMask toBeUsedInSystemsMask { get { return layerMask - 1; } }
        void Start()
        {
            _Gizmos = MeshGizmos.Instance;
        }
        void OnValidate()
        {
            MakeSureWeSelectOnlyOnLayer();
        }

        private void MakeSureWeSelectOnlyOnLayer()
        {
            if (CountOnBits(layerMask) > 1)
            {
                layerMask &= ~previous_LayerMask;
            } 
            previous_LayerMask = layerMask;
        }

        public int CountOnBits(int x)
        {
            int count = 0;
            while (x != 0)
            {
                if ((x & 1) != 0) count++;
                x = x >> 1;
            }
            return count;
        }

     
        void Update()
        {
            _Gizmos.DrawLine(transform.position - (transform.right * .5f), transform.position + transform.forward - (transform.right * .5f), Color.yellow, toBeUsedInSystemsMask);
            _Gizmos.DrawRay(transform.position - transform.right, transform.forward, Color.green, toBeUsedInSystemsMask);
            _Gizmos.DrawCube(transform.position + position_cubeGizmo, Vector3.zero, Color.red, toBeUsedInSystemsMask);
            _Gizmos.DrawSphere(transform.position + position_sphereGizmo, Vector3.zero, Color.green, toBeUsedInSystemsMask);
            _Gizmos.DrawArrowHead(transform.position + position_arrowGizmo, Vector3.zero, Color.magenta, toBeUsedInSystemsMask);

            //Combine Gizmos
            var arrowStart = transform.position - transform.right + (transform.forward * 1.5f);
            var arrowDirection = transform.right;
            _Gizmos.DrawRay(arrowStart, arrowDirection, Color.cyan, toBeUsedInSystemsMask);
            _Gizmos.DrawArrowHead(arrowStart + arrowDirection, arrowDirection, Color.white, toBeUsedInSystemsMask);
        }
    }
}