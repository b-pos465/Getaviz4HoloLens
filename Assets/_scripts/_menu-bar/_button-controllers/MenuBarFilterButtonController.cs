using Gaze;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class MenuBarFilterButtonController : MonoBehaviour
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
        this.keywordToCommandService.Register(GetavizKeyword.FILTER, this.OnFilterVoiceCommand);
    }

    private void OnFilterVoiceCommand()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.TriggerFilter();
        }
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.gameObject)
        {
            this.TriggerFilter();
        }
    }

    private void TriggerFilter()
    {
        this.buttonClickSoundService.PlayButtonClickSound();
        this.modelStateController.SwitchState(ModelState.FILTER);
    }
}
