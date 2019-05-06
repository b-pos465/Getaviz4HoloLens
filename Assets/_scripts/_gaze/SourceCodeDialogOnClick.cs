using Gaze;
using Model;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class SourceCodeDialogOnClick : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private ModelRootIndicator modelRootIndicator;

    [Inject]
    private AppBarIndicator appBarIndicator;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    [Inject]
    private TapService tapService;

    void Start()
    {
        this.tapService.Register(this.OnAirTap);
    }

    private void OnAirTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target.GetComponent<Entity>() != null)
        {
            this.TriggerDialog();
        }
    }

    private void TriggerDialog()
    {
        bool dialogActive = this.sourceCodeDialogIndicator.gameObject.activeSelf;
        bool modelActive = this.modelRootIndicator.gameObject.activeSelf;
        bool appBarActive = this.appBarIndicator.gameObject.activeSelf;

        this.sourceCodeDialogIndicator.gameObject.SetActive(!dialogActive);
        this.modelRootIndicator.gameObject.SetActive(!modelActive);
        this.appBarIndicator.gameObject.SetActive(!appBarActive);
    }

    public void Close()
    {
        this.TriggerDialog();
    }
}
