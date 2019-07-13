using Gaze;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class MenuBarInfoButtonController : MonoBehaviour
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
        this.keywordToCommandService.Register(GetavizKeyword.INFO, this.OnInfoVoiceCommand);
    }

    private void OnInfoVoiceCommand()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.TriggerInfo();
        }
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.gameObject)
        {
            this.TriggerInfo();
        }
    }

    private void TriggerInfo()
    {
        this.buttonClickSoundService.PlayButtonClickSound();
    }
}
