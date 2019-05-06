using Gaze;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class DialogController : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private ModelIndicator modelIndicator;

    [Inject]
    private CloseButtonIndicator closeButtonIndicator;

    [Inject]
    private AppBarIndicator appBarIndicator;

    [Inject]
    private TapService tapService;

    public float distanceToCamera = 2.5f;

    private void Start()
    {
        this.gameObject.SetActive(false);
        this.tapService.Register(this.OnTap);
    }

    private void Update()
    {
        this.PlaceInFrontOfCamera();
        this.AdjustRotationToCameraPosition();
    }

    private void PlaceInFrontOfCamera()
    {
        Vector3 forwardWithoutY = Camera.main.transform.forward;
        forwardWithoutY.y = 0;
        this.transform.position = Camera.main.transform.position + this.distanceToCamera * forwardWithoutY;
    }

    private void AdjustRotationToCameraPosition()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.closeButtonIndicator.gameObject)
        {
            this.Close();
        }
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
        this.modelIndicator.gameObject.SetActive(true);
        this.appBarIndicator.gameObject.SetActive(true);
    }
}
