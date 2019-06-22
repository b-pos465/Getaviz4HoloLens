using Gaze;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class DialogController : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private CloseButtonIndicator closeButtonIndicator;

    [Inject]
    private ModelStateController modelStateController;

    [Inject]
    private TapService tapService;

    [Inject]
    private KeywordToCommandService keywordToCommandService;


    private void Start()
    {
        this.tapService.Register(this.OnTap);
        this.keywordToCommandService.Register(GetavizKeyword.CLOSE, this.OnCloseVoiceCommand);
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.closeButtonIndicator.gameObject)
        {
            this.Close();
        }
    }

    private void OnCloseVoiceCommand()
    {
        this.Close();
    }

    private void Close()
    {
        this.modelStateController.SwitchState(ModelState.INTERACTABLE);
    }
}
