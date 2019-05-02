using Logging;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class DragRecognizer : MonoBehaviour
{

    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private GestureRecognizer gestureRecognizer;
    private float lastHorizontalValue;

    public bool IsDragging
    {
        get; private set;
    }

    public float HorizontalDelta
    {
        get; private set;
    }

    void Start()
    {
        log.Debug("Starting GestureRecognizer ...");
        this.gestureRecognizer = new GestureRecognizer();
        this.gestureRecognizer.NavigationStartedEvent += NavigationStarted;
        this.gestureRecognizer.NavigationCompletedEvent += NavigationCompleted;
        this.gestureRecognizer.NavigationUpdatedEvent += NavigationUpdated;
        this.gestureRecognizer.NavigationUpdated += OnNavigationUpdated;
        this.gestureRecognizer.StartCapturingGestures();
    }

    private void NavigationStarted(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        this.IsDragging = true;
        this.HorizontalDelta = 0f;
        this.lastHorizontalValue = cumulativeDelta.z;
        log.Debug("Drag started.");
    }
    private void NavigationCompleted(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        this.IsDragging = false;
        this.HorizontalDelta = 0f;
        log.Debug("Drag finished.");
    }
    private void NavigationUpdated(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        this.HorizontalDelta = cumulativeDelta.z - this.lastHorizontalValue;

        //log.Debug("Drag - Horizontal Delta: {}.", this.HorizontalDelta);
        //log.Debug("Drag - cumulativeDelta: {}.", cumulativeDelta);
        this.lastHorizontalValue = cumulativeDelta.z;
    }

    void OnNavigationUpdated(NavigationUpdatedEventArgs navigationUpdatedEventArgs)
    {
        this.HorizontalDelta = navigationUpdatedEventArgs.normalizedOffset.x;
        log.Debug("UPDATE: {}", navigationUpdatedEventArgs.normalizedOffset);
    }
}
