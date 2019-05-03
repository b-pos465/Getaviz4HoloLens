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
            this.gestureRecognizer.Tapped += this.OnAirTap;
            this.gestureRecognizer.StartCapturingGestures();

            this.plane = Instantiate(this.markerPrefab);
            this.plane.SetActive(false);
        }

        private void OnAirTap(TappedEventArgs tappedEventArgs)
        {
            if (this.plane == null)
            {
                return;
            }

            this.ShootRayAndPlaceModelIfPossible();
        }

        void ShootRayAndPlaceModelIfPossible()
        {
            if (this.rayCaster.Hits)
            {
                GameObject model = this.importController.Import();
                this.ScaleModel(model, this.rayCaster.HitPoint);

                Destroy(this.plane);
                Destroy(this);
            }
        }

        void ScaleModel(GameObject model, Vector3 center)
        {
            model.transform.localScale = 0.0005f * Vector3.one;
            model.transform.position = center;
        }

        void Update()
        {
            if (this.rayCaster.Hits)
            {
                if (this.HitPointNormalPointsUpward(this.rayCaster.HitPointNormal))
                {
                    this.plane.SetActive(true);
                    this.plane.transform.position = this.rayCaster.HitPoint;
                    this.plane.transform.up = this.rayCaster.HitPointNormal;
                }
                else
                {
                    this.plane.SetActive(false);
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
