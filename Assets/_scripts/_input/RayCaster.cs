using HoloToolkit.Unity.InputModule;
using UnityEngine;
using Zenject;

namespace Gaze
{
    public class RayCaster : MonoBehaviour
    {
        /*
         * The 'HoloToolkit' provides a gaze stabilizer which helps the user to focus on a certain point.
         */
        [Inject]
        GazeStabilizer gazeStabilizer;

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
                                        this.gazeStabilizer.StableRay,
                                        out hitInfo,
                                        this.length,
                                        Physics.AllLayers);
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
