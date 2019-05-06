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


    private void Start()
    {
        this.gameObject.SetActive(false);
        this.tapService.Register(this.OnTap);
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
