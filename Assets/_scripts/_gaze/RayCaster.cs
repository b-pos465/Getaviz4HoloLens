using UnityEngine;

namespace Gaze
{
    public class RayCaster : MonoBehaviour
    {
        public float length = 20.0f;

        public bool Hits
        {
            get
            {
                return this.Target != null;
            }
            private set
            {
                // Read-only properties are not available in C# 4: https://stackoverflow.com/questions/35035632/property-with-getter-only-vs-with-getter-and-private-setter
            }
        }

        public GameObject Target { get; private set; }
        public Vector3 HitPoint { get; private set; }
        public Vector3 HitPointNormal { get; private set; }

        void Update()
        {
            RaycastHit hitInfo;
            bool raycastHit = Physics.Raycast(
                                        Camera.main.transform.position,
                                        Camera.main.transform.forward,
                                        out hitInfo,
                                        this.length,
                                        Physics.DefaultRaycastLayers);
            if (raycastHit)
            {
                this.Target = hitInfo.collider.gameObject;
                this.HitPoint = hitInfo.point;
                this.HitPointNormal = hitInfo.normal;
            }
            else
            {
                this.Target = null;
            }
        }
    }
}
