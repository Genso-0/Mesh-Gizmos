 
using UnityEngine;

namespace Mesh_Gizmos.Examples
{
    public class DirectGravity : MonoBehaviour
    {
        Rigidbody rb;
        public GameObject sun;  
        Rigidbody rb_gravityCenter;
        Vector3 startPosition;
        Vector3 gravitationalForce;
        MeshGizmos _Gizmos;
        [SerializeField] LayerMask layerMask = 1;
        LayerMask previous_LayerMask = 1;
        LayerMask toBeUsedInSystemsMask { get { return layerMask - 1; } }
        void Start()
        {
            _Gizmos = MeshGizmos.Instance;
            startPosition = transform.position;
            rb = GetComponent<Rigidbody>();
            rb_gravityCenter = sun.GetComponent<Rigidbody>();
            SetGravitationalForce();
            var perpendicularForce = Vector3.Cross(gravitationalForce.normalized, sun.transform.up); 
            rb.AddForce(perpendicularForce * gravitationalForce.magnitude, ForceMode.Impulse);
        }
        void FixedUpdate()
        {
            SetGravitationalForce();
            rb.AddForce(gravitationalForce);
        
        }
        void OnValidate()
        {
            MakeSureWeSelectOnlyOnLayer();
        }
        void Update()
        {
            _Gizmos.DrawRay(transform.position, gravitationalForce, Color.red, toBeUsedInSystemsMask);
            _Gizmos.DrawArrowHead(transform.position + gravitationalForce, gravitationalForce, Color.white, toBeUsedInSystemsMask);
            _Gizmos.DrawRay(transform.position, rb.velocity, Color.blue, toBeUsedInSystemsMask);
            _Gizmos.DrawArrowHead(transform.position + rb.velocity, rb.velocity, Color.white, toBeUsedInSystemsMask);
        }
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == sun)
            {
                transform.position = startPosition;
                var perpendicularForce = Vector3.Cross(gravitationalForce.normalized, transform.up);
                rb.AddForce(perpendicularForce * gravitationalForce.magnitude, ForceMode.Impulse);
            }
        }
        void SetGravitationalForce()
        {
            var dir = sun.transform.position - transform.position;
            var mag = dir.magnitude;
            float acceleration = (9.81f * rb.mass * rb_gravityCenter.mass) / (mag * mag);
            gravitationalForce = dir * Time.deltaTime * acceleration;
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
        void OnDrawGizmos()
        {
            var perpendicularForce = Vector3.Cross(gravitationalForce.normalized, transform.up);
            Debug.DrawRay(transform.position, perpendicularForce * gravitationalForce.magnitude);
        }
    }
}