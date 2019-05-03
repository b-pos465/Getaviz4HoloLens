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

    public Vector3 CumulativeDelta
    {
        get; private set;
    }

    void Start()
    {
        log.Debug("Starting GestureRecognizer ...");
        this.gestureRecognizer = new GestureRecognizer();
        this.gestureRecognizer.NavigationUpdated += this.OnNavigationUpdated;
        this.gestureRecognizer.ManipulationUpdated += this.OnManipulationUpdated;
        this.gestureRecognizer.StartCapturingGestures();
    }

    private void NavigationStarted(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        this.IsDragging = true;
        this.lastHorizontalValue = cumulativeDelta.z;
        log.Debug("Drag started.");
    }
    private void NavigationCompleted(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        this.IsDragging = false;
        log.Debug("Drag finished.");
    }

    void OnNavigationUpdated(NavigationUpdatedEventArgs navigationUpdatedEventArgs)
    {
        //this.CumulativeDelta = navigationUpdatedEventArgs.;
        log.Debug("UPDATE: {}", navigationUpdatedEventArgs.normalizedOffset);
    }

    void OnManipulationUpdated(ManipulationUpdatedEventArgs manipulationUpdatedEventArgs)
    {
        //this.CumulativeDelta = navigationUpdatedEventArgs.;
        log.Debug("MANIPULATION: {}", manipulationUpdatedEventArgs.cumulativeDelta);
    }
}
