using Gaze;
using Logging;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

namespace SpatialMapping
{
    public class MetaphorPlacer : MonoBehaviour
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Inject]
        private ModelIndicator modelIndicator;

        [Inject]
        private ModelStateController modelStateController;

        [Inject]
        private RayCaster rayCaster;

        [Inject]
        private TapService tapService;

        [Inject]
        private SpatialMappingRootIndicator spatialMappingRootIndicator;

        [Header("Bounds Approach")]
        public Vector3 boundsSize = new Vector3(0.1f, 0.05f, 0.1f);

        [Header("Common")]
        public Strategy strategy = Strategy.BOUNDING_BOX_AVERAGE;
        public GameObject markerPrefab;
        public float tolerance = 0.1f;

        [Header("Debug")]
        public bool verbose = false;

        public bool PlacementEnabled = true;

        private void Start()
        {
            this.tapService.Register(this.OnTap);
        }

        private void OnTap(TappedEventArgs tappedEventArgs)
        {
            if (this.PlacementEnabled)
            {
                this.ShootRayAndPlaceModelIfPossible();
            }
        }

        void ShootRayAndPlaceModelIfPossible()
        {
            if (this.rayCaster.Hits)
            {
                this.modelIndicator.transform.position = this.rayCaster.HitPoint;
                this.modelStateController.SwitchState(ModelState.INTERACTABLE);
                Destroy(this.spatialMappingRootIndicator.gameObject);
                Destroy(this);
            }
        }

        void Update()
        {
            if (this.rayCaster.Hits)
            {
                Vector3 normal = Vector3.zero;

                if (this.strategy == Strategy.HITPOINT_NORMAL)
                {
                    normal = this.rayCaster.HitPointNormal;
                }
                else
                {
                    normal = this.CalculateAverageNormalInBounds(); ;
                }

                if (this.verbose)
                {
                    log.Debug("{}", normal);
                }

                if (this.HitPointNormalPointsUpward(normal))
                {
                    this.modelStateController.SwitchState(ModelState.PLACEMENT_VISIBLE);
                    this.modelIndicator.transform.position = this.rayCaster.HitPoint;
                }
                else
                {
                    this.modelStateController.SwitchState(ModelState.PLACEMENT_INVISIBLE);
                }
            }
        }

        private Vector3 CalculateAverageNormalInBounds()
        {
            Vector3 hitPoint = this.rayCaster.HitPoint;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();

            foreach (MeshFilter meshFilter in this.spatialMappingRootIndicator.GetComponentsInChildren<MeshFilter>())
            {
                if (meshFilter.sharedMesh == null)
                {
                    continue;
                }

                foreach (Vector3 vertex in meshFilter.sharedMesh.vertices)
                {
                    vertices.Add(meshFilter.transform.TransformPoint(vertex));
                }

                meshFilter.sharedMesh.RecalculateNormals();
                normals.AddRange(meshFilter.sharedMesh.vertices);
            }

            Bounds bounds = new Bounds(hitPoint, this.boundsSize);

            Vector3 farestCorner = hitPoint + 0.5f * bounds.size;

            float maxDistance = (farestCorner - hitPoint).magnitude;

            List<int> verticesInsideBoundsAsIndices = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                if ((hitPoint - vertices[i]).magnitude < maxDistance)
                {
                    verticesInsideBoundsAsIndices.Add(i);
                }
            }

            if (this.verbose)
            {
                log.Debug("{}", verticesInsideBoundsAsIndices.Count);
            }

            if (verticesInsideBoundsAsIndices.Count == 0)
            {
                return this.rayCaster.HitPointNormal.normalized;
            }

            Vector3 normal = Vector3.zero;
            foreach (int index in verticesInsideBoundsAsIndices)
            {
                normal += normals[index];
            }
            return normal.normalized;
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
            this.tapService.Unregister(this.OnTap);
        }
    }

    public enum Strategy
    {
        HITPOINT_NORMAL,
        BOUNDING_BOX_AVERAGE
    }
}
