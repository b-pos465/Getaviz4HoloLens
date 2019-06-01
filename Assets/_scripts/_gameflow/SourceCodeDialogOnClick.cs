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
    private ModelStateController modelStateController;

    [Inject]
    private AppBarIndicator appBarIndicator;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    [Inject]
    private TapService tapService;

    void Start()
    {
        this.tapService.Register(this.OnTap);
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits)
        {
            Entity entity = this.rayCaster.Target.GetComponent<Entity>();
            if (entity != null && entity.type == "FAMIX.Class")
            {
                this.modelStateController.SwitchState(ModelState.SOURCECODE);
            }
        }
    }
}
