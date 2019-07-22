using Gaze;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class TutorialDialogController : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private LoadingTextController loadingTextController;

    [Inject]
    private YesButtonIndicator yesButtonIndicator;

    [Inject]
    private NoButtonIndicator noButtonIndicator;

    [Inject]
    private ButtonClickSoundService buttonClickSoundService;

    [Inject]
    private ModelStateController modelStateController;

    [Inject]
    private TapService tapService;

    [Inject]
    private KeywordToCommandService keywordToCommandService;

    [Inject]
    private TutorialStateControllerIndicator tutorialStateControllerIndicator;


    private void Start()
    {
        this.tapService.Register(this.OnTap);
        this.keywordToCommandService.Register(GetavizKeyword.YES, this.SelectYes);
        this.keywordToCommandService.Register(GetavizKeyword.NO, this.SelectNo);
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (!this.rayCaster.Hits)
        {
            return;
        }

        this.buttonClickSoundService.PlayButtonClickSound();

        if (this.rayCaster.Target == this.yesButtonIndicator.gameObject)
        {
            this.SelectYes();
        }
        else if (this.rayCaster.Target == this.noButtonIndicator.gameObject)
        {
            this.SelectNo();
        }
    }

    private void OnDestroy()
    {
        this.tapService.Unregister(this.OnTap);
        this.keywordToCommandService.Unregister(GetavizKeyword.YES, this.SelectYes);
        this.keywordToCommandService.Unregister(GetavizKeyword.NO, this.SelectNo);
    }

    private void SelectYes()
    {
        this.loadingTextController.gameObject.SetActive(true);
        this.tutorialStateControllerIndicator.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }

    private void SelectNo()
    {
        this.loadingTextController.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }
}
