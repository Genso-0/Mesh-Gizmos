using Mesh_Gizmos;
using UnityEngine;

public class ExampleGizmoCalls : MonoBehaviour
{
    MeshGizmos _Gizmos;
    public Vector3 position_cubeGizmo;
    public Vector3 position_sphereGizmo;
    public Vector3 position_arrowGizmo;
    void Start()
    {
        _Gizmos = MeshGizmos.Instance;
    }

    void Update()
    {
        _Gizmos.DrawLine(transform.position - (transform.right * .5f), transform.position + transform.forward - (transform.right * .5f), Color.yellow);
        _Gizmos.DrawRay(transform.position - transform.right, transform.forward, Color.green);
        _Gizmos.DrawCube(transform.position + position_cubeGizmo, Vector3.zero, Color.red);
        _Gizmos.DrawSphere(transform.position + position_sphereGizmo, Vector3.zero, Color.green);
        _Gizmos.DrawArrowHead(transform.position + position_arrowGizmo, Vector3.zero, Color.magenta); 

        //Combine Gizmos
       var arrowStart = transform.position - transform.right + (transform.forward * 1.5f);
        var arrowDirection = transform.right;
        _Gizmos.DrawRay(arrowStart, arrowDirection, Color.magenta);
        _Gizmos.DrawArrowHead(arrowStart + arrowDirection, arrowDirection, Color.white);
    }
}
