using Import;
using Logging;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

namespace spatial_mapping
{
    public class MetaphorPlacer : MonoBehaviour
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Inject]
        public ImportController importController;

        public GameObject markerPrefab;

        public float tolerance = 0.1f;

        private GameObject plane;
        private GestureRecognizer gestureRecognizer;


        private void Start()
        {
            log.Debug("Starting GestureRecognizer ...");
            this.gestureRecognizer = new GestureRecognizer();
            this.gestureRecognizer.TappedEvent += OnAirTap;
            this.gestureRecognizer.StartCapturingGestures();

            this.plane = Instantiate(this.markerPrefab);
            plane.SetActive(false);
        }

        private void OnAirTap(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            if (this.plane == null)
            {
                return;
            }

            ShootRayAndPlaceModelIfPossible(headRay);
        }

        void ShootRayAndPlaceModelIfPossible(Ray headRay)
        {
            RaycastHit hitInfo;
            bool raycastHit = Physics.Raycast(headRay, out hitInfo, 20.0f, Physics.DefaultRaycastLayers);

            if (raycastHit)
            {
                GameObject model = this.importController.Import();
                CenterAndScaleModel(model, hitInfo.point);

                Destroy(this.plane);
                Destroy(this);
            }
        }

        void CenterAndScaleModel(GameObject model, Vector3 center)
        {
            model.transform.localScale = 0.0005f * Vector3.one;
            model.transform.position = center;

            Bounds bounds = new Bounds(model.transform.position, Vector3.zero);
            BoxCollider[] colliders = model.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider collider in colliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            log.Debug("Calculated overall bounds for model: x={}, y={}, z={}", bounds.size.x, bounds.size.y, bounds.size.z);
            Vector3 newPosition = model.transform.position;

            newPosition.x -= bounds.size.x / 2;
            newPosition.z -= bounds.size.z / 2;
            model.transform.position = newPosition;


        }

        void Update()
        {
            RaycastHit hitInfo;
            bool raycastHit = Physics.Raycast(
                                        Camera.main.transform.position,
                                        Camera.main.transform.forward,
                                        out hitInfo,
                                        20.0f,
                                        Physics.DefaultRaycastLayers);
            if (raycastHit)
            {
                if (HitPointNormalPointsUpward(hitInfo))
                {
                    plane.SetActive(true);
                    plane.transform.position = hitInfo.point;
                    plane.transform.up = hitInfo.normal;
                }
                else
                {
                    plane.SetActive(false);
                }
            }
        }

        private bool HitPointNormalPointsUpward(RaycastHit hitInfo)
        {
            bool xIsSmallEnough = Mathf.Abs(hitInfo.normal.x) < this.tolerance;
            bool zIsSmallEnough = Mathf.Abs(hitInfo.normal.z) < this.tolerance;
            bool yIsPositive = hitInfo.normal.y > 0f;

            return xIsSmallEnough && zIsSmallEnough && yIsPositive;
        }

        private void OnDestroy()
        {
            log.Debug("Stopping GestureRecognizer ...");
            this.gestureRecognizer.StopCapturingGestures();
        }
    }
}
