using Gaze;
using Import;
using Logging;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

namespace SpatialMapping
{
    public class MetaphorPlacer : MonoBehaviour
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Inject]
        private ImportController importController;

        [Inject]
        private RayCaster rayCaster;

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

        // We use the 'Ray' from the TapEvent to make sure that it is the right one.
        // There could be a delay.
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

            log.Debug("Calculated overall bounds for model: x={}, y={}, z={}.", bounds.size.x, bounds.size.y, bounds.size.z);
            Vector3 newPosition = model.transform.position;

            newPosition.x -= bounds.size.x / 2;
            newPosition.z -= bounds.size.z / 2;
            model.transform.position = newPosition;
        }

        void Update()
        {
            if (this.rayCaster.Hits)
            {
                if (HitPointNormalPointsUpward(this.rayCaster.HitPointNormal))
                {
                    plane.SetActive(true);
                    plane.transform.position = this.rayCaster.HitPoint;
                    plane.transform.up = this.rayCaster.HitPointNormal;
                }
                else
                {
                    plane.SetActive(false);
                }
            }
        }

        private bool HitPointNormalPointsUpward(Vector3 hitPointNormal)
        {
            bool xIsSmallEnough = Mathf.Abs(hitPointNormal.x) < this.tolerance;
            bool zIsSmallEnough = Mathf.Abs(hitPointNormal.z) < this.tolerance;
            bool yIsPositive = hitPointNormal.y > 0f;

            return xIsSmallEnough && zIsSmallEnough && yIsPositive;
        }

        private void OnDestroy()
        {
            log.Debug("Stopping GestureRecognizer ...");
            this.gestureRecognizer.StopCapturingGestures();
        }
    }
}
