using Gaze;
using HoloToolkit.Unity.UX;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class MenuBarDoneButtonController : MonoBehaviour
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

    [Inject]
    private BoundingBoxRig boundingBoxRig;

    private void Start()
    {
        this.tapService.Register(this.OnTap);
        this.keywordToCommandService.Register(GetavizKeyword.DONE, this.OnDoneVoiceCommand);
    }

    private void OnDoneVoiceCommand()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.TriggerDone();
        }
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.gameObject)
        {
            this.TriggerDone();
        }
    }

    private void TriggerDone()
    {
        this.buttonClickSoundService.PlayButtonClickSound();
        this.modelStateController.SwitchState(ModelState.INTERACTABLE);
    }
}
