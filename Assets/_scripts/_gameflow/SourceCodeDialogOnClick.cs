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
    private ModelIndicator modelIndicator;

    [Inject]
    private AppBarIndicator appBarIndicator;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    [Inject]
    private TapService tapService;

    private bool active = true;

    void Start()
    {
        this.tapService.Register(this.OnTap);
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (!this.active)
        {
            return;
        }

        if (this.rayCaster.Hits)
        {
            Entity entity = this.rayCaster.Target.GetComponent<Entity>();
            if (entity != null && entity.type == "FAMIX.Class")
            {
                this.TriggerDialog();
            }
        }
    }

    private void TriggerDialog()
    {
        bool dialogActive = this.sourceCodeDialogIndicator.gameObject.activeSelf;
        bool modelActive = this.modelIndicator.gameObject.activeSelf;
        bool appBarActive = this.appBarIndicator.gameObject.activeSelf;

        this.sourceCodeDialogIndicator.gameObject.SetActive(!dialogActive);
        this.modelIndicator.gameObject.SetActive(!modelActive);
        this.appBarIndicator.gameObject.SetActive(!appBarActive);
    }

    public void Close()
    {
        this.TriggerDialog();
    }

    public void Enable()
    {
        this.active = true;
    }

    public void Disable()
    {
        this.active = false;
    }
}
