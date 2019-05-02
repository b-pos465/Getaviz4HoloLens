using Gaze;
using Logging;
using Model;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class SourceCodeOnClickController : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    [Inject]
    RayCaster rayCaster;

    [Inject]
    MeshRootIndicator meshRootIndicator;

    public float distanceToCamera = 2.5f;

    private Canvas canvas;
    private GestureRecognizer gestureRecognizer;

    void Start()
    {
        this.canvas = GetComponent<Canvas>();
        this.canvas.enabled = false;

        log.Debug("Starting GestureRecognizer ...");
        this.gestureRecognizer = new GestureRecognizer();
        this.gestureRecognizer.TappedEvent += OnAirTap;
        this.gestureRecognizer.StartCapturingGestures();
    }

    private void Update()
    {
        AdjustRotationToCameraPosition();
    }

    private void OnAirTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target.GetComponent<Entity>() != null)
        {
            this.canvas.enabled = true;
            this.meshRootIndicator.gameObject.SetActive(false);

            Vector3 forwardWithoutY = Camera.main.transform.forward;
            forwardWithoutY.y = 0;
            transform.position = Camera.main.transform.position + this.distanceToCamera * forwardWithoutY;
        }
    }

    private void AdjustRotationToCameraPosition()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public void Close()
    {
        this.canvas.enabled = false;
        this.meshRootIndicator.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        log.Debug("Stopping GestureRecognizer ...");
        this.gestureRecognizer.StopCapturingGestures();
    }
}
