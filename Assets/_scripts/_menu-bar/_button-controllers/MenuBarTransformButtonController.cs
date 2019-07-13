using Gaze;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class MenuBarTransformButtonController : MonoBehaviour
{
    [Inject]
    private TapService tapService;

    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private KeywordToCommandService keywordToCommandService;

    [Inject]
    private ButtonClickSoundService buttonClickSoundService;

    [Inject]
    private ModelStateController modelStateController;

    private void Start()
    {
        this.tapService.Register(this.OnTap);
        this.keywordToCommandService.Register(GetavizKeyword.TRANSFORM, this.OnTransformVoiceCommand);
    }

    private void OnTransformVoiceCommand()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.TriggerTransform();
        }
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.gameObject)
        {
            this.TriggerTransform();
        }
    }

    private void TriggerTransform()
    {
        this.buttonClickSoundService.PlayButtonClickSound();

        this.modelStateController.SwitchState(ModelState.TRANSFORM);
    }
}
