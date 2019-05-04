using Gaze;
using Import;
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
        private ImportController importController;

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

        private GameObject plane;

        private GameObject boundingBox;


        private void Start()
        {
            this.plane = Instantiate(this.markerPrefab);
            this.plane.SetActive(false);
            this.tapService.Register(this.OnTap);

            this.boundingBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(this.boundingBox.GetComponent<BoxCollider>());
        }

        private void OnTap(TappedEventArgs tappedEventArgs)
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
                Vector3 normal = Vector3.zero;

                if (this.strategy == Strategy.HITPOINT_NORMAL)
                {
                    normal = this.rayCaster.HitPointNormal;
                }
                else
                {
                    normal = this.CalculateAverageNormalInBounds(); ;
                }

                log.Debug("{}", normal);

                if (this.HitPointNormalPointsUpward(normal))
                {
                    this.plane.SetActive(true);
                    this.plane.transform.position = this.rayCaster.HitPoint;
                    this.plane.transform.up = this.transform.up;
                }
                else
                {
                    this.plane.SetActive(false);
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

            this.boundingBox.transform.position = bounds.center;
            this.boundingBox.transform.localScale = bounds.size;
            this.boundingBox.GetComponent<Renderer>().enabled = false;


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

            log.Debug("{}", verticesInsideBoundsAsIndices.Count);

            if (verticesInsideBoundsAsIndices.Count == 0)
            {
                return this.rayCaster.HitPointNormal.normalized;
            }
            else if (verticesInsideBoundsAsIndices.Count > 10)
            {
                //return Vector3.up;
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
    }

    public enum Strategy
    {
        HITPOINT_NORMAL,
        BOUNDING_BOX_AVERAGE
    }
}
