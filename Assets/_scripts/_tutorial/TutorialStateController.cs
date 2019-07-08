using Model;
using Model.Tree;
using SpatialMapping;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TutorialStateController : MonoBehaviour
{
    [Inject]
    private ModelStateController modelStateController;

    [Inject]
    private MetaphorPlacer metaphorPlacer;

    [Inject]
    private ModelIndicator modelIndicator;

    [Inject]
    private EntityNameOnHoverController entityNameOnHoverController;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    [Inject]
    private AutoCompleteController autoCompleteController;

    [Header("Text to Speech")]
    public AudioSource[] textToSpeechList = new AudioSource[15];

    private delegate bool TutorialPredicate();
    private delegate void TutorialAction();

    private void Start()
    {
        this.GazeAtTheTableStep();
    }

    private void GazeAtTheTableStep()
    {
        this.textToSpeechList[0].Play();

        this.metaphorPlacer.PlacementEnabled = false;
        this.metaphorPlacer.gameObject.SetActive(true);

        this.StartCoroutine(this.WaitForSuccessfullGaze());
    }

    private IEnumerator WaitForSuccessfullGaze()
    {
        while (true)
        {
            if (this.modelStateController.ModelState == ModelState.PLACEMENT_VISIBLE)
            {
                bool holdWithoutInterruption = true;
                float startTime = Time.time;

                while (Time.time - startTime < 2f)
                {
                    if (this.modelStateController.ModelState != ModelState.PLACEMENT_VISIBLE)
                    {
                        holdWithoutInterruption = false;
                        break;
                    }

                    yield return null;
                }

                if (holdWithoutInterruption)
                {
                    while (this.AtLeastOneAudioSourceIsPlaying())
                    {
                        yield return null;
                    }

                    this.UseAirTapToPlaceStep();
                    break;
                }
            }
            yield return null;
        }
    }

    private void UseAirTapToPlaceStep()
    {
        this.metaphorPlacer.PlacementEnabled = true;
        this.textToSpeechList[1].Play();
        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.INTERACTABLE,
                this.ClickOrSayTransformStep));
    }

    private void ClickOrSayTransformStep()
    {
        this.modelStateController.SwitchState(ModelState.TUTORIAL_TRANSFORM_ONLY);
        this.textToSpeechList[2].Play();

        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.TRANSFORM,
                this.RotateTheModelStep));
    }

    private void RotateTheModelStep()
    {
        this.textToSpeechList[3].Play();
        this.StartCoroutine(this.WaitForSuccessfullRotation());
    }

    private IEnumerator WaitForSuccessfullRotation()
    {
        Quaternion startRotation = this.modelIndicator.transform.rotation;

        while (this.modelIndicator.transform.rotation == startRotation)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.ScaleTheModelStep));
    }

    private void ScaleTheModelStep()
    {
        this.textToSpeechList[4].Play();
        this.StartCoroutine(this.WaitForSuccessfullScale());
    }

    private IEnumerator WaitForSuccessfullScale()
    {
        Vector3 startScale = this.modelIndicator.transform.localScale;

        while (this.modelIndicator.transform.localScale == startScale)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.TapAndHoldStep));
    }

    private void TapAndHoldStep()
    {
        this.textToSpeechList[5].Play();
        this.StartCoroutine(this.WaitForSuccessfullRepositioning());
    }

    private IEnumerator WaitForSuccessfullRepositioning()
    {
        Vector3 startPosition = this.modelIndicator.transform.position;

        while (this.modelIndicator.transform.position == startPosition)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.ClickOrSayDoneStep));
    }

    private void ClickOrSayDoneStep()
    {
        this.textToSpeechList[6].Play();
        this.StartCoroutine(this.WaitForLeavingTheTransformMode());
    }

    private IEnumerator WaitForLeavingTheTransformMode()
    {
        while (this.modelStateController.ModelState != ModelState.INTERACTABLE)
        {
            yield return null;
        }

        this.modelStateController.SwitchState(ModelState.TUTORIAL_GAZE_ONLY);

        this.StartCoroutine(this.WaitForOtherAudioSources(this.GazeAtClassesOrPackagesToInspectTheirNameStep));
    }

    private void GazeAtClassesOrPackagesToInspectTheirNameStep()
    {
        this.textToSpeechList[7].Play();
        this.StartCoroutine(this.GazeAtAnEntityForAtLeastTwoSeconds());
    }

    private IEnumerator GazeAtAnEntityForAtLeastTwoSeconds()
    {
        while (true)
        {
            if (this.entityNameOnHoverController.IsShowingEntityName())
            {
                bool holdWithoutInterruption = true;
                float startTime = Time.time;

                Entity startEntity = this.entityNameOnHoverController.GetCurrentEntity();

                while (Time.time - startTime < 2f)
                {
                    if (!this.entityNameOnHoverController.IsShowingEntityName() || this.entityNameOnHoverController.GetCurrentEntity() != startEntity)
                    {
                        holdWithoutInterruption = false;
                        break;
                    }

                    yield return null;
                }

                if (holdWithoutInterruption)
                {
                    this.StartCoroutine(this.WaitForOtherAudioSources(this.TapOnAClassStep));
                    break;
                }
            }
            yield return null;
        }
    }

    private void TapOnAClassStep()
    {
        this.textToSpeechList[8].Play();
        this.modelStateController.SwitchState(ModelState.TUTORIAL_SOURCECODE_ONLY);

        this.StartCoroutine(this.WaitForSuccessfullTapOnClass());
    }

    private IEnumerator WaitForSuccessfullTapOnClass()
    {
        while (this.modelStateController.ModelState != ModelState.SOURCECODE)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.MoveTheSourceCodeDialogStep));
    }

    private void MoveTheSourceCodeDialogStep()
    {
        this.textToSpeechList[9].Play();
        this.StartCoroutine(this.WaitForSuccessfullMovementOfSourceCodeDialog());
    }

    private IEnumerator WaitForSuccessfullMovementOfSourceCodeDialog()
    {
        Vector3 startPosition = this.sourceCodeDialogIndicator.transform.position;

        while (this.sourceCodeDialogIndicator.transform.position == startPosition)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.NavigateThroughTheSourceCodeStep));
    }

    private void NavigateThroughTheSourceCodeStep()
    {
        this.textToSpeechList[10].Play();
        this.StartCoroutine(this.WaitForSuccessfullSourceCodeNavigation());
    }

    private IEnumerator WaitForSuccessfullSourceCodeNavigation()
    {
        ScrollRect scrollRect = this.sourceCodeDialogIndicator.GetComponentInChildren<ScrollRect>();
        float startOffset = scrollRect.verticalNormalizedPosition;

        while (Mathf.Abs(scrollRect.verticalNormalizedPosition - startOffset) < 0.1f)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.CloseTheSourceCodeDialogStep));
    }

    private void CloseTheSourceCodeDialogStep()
    {
        this.textToSpeechList[11].Play();
        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.INTERACTABLE,
                this.OpenTheFilterDialogStep
                    ));
    }

    private void OpenTheFilterDialogStep()
    {
        this.textToSpeechList[12].Play();
        this.modelStateController.SwitchState(ModelState.TUTORIAL_FILTER_ONLY);

        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.FILTER,
                this.NavigateThroughTheFilteringInterface
                ));
    }

    private void NavigateThroughTheFilteringInterface()
    {
        this.textToSpeechList[13].Play();
        this.StartCoroutine(this.WaitForSuccessfullyUsingAutoCompleteInFilterDialog());
    }

    private IEnumerator WaitForSuccessfullyUsingAutoCompleteInFilterDialog()
    {
        EntityNode startEntityNode = this.autoCompleteController.CurrentEntityNode;

        while (this.autoCompleteController.CurrentEntityNode == startEntityNode)
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(this.FinishTutorial));
    }

    private void FinishTutorial()
    {
        this.textToSpeechList[14].Play();
    }

    private bool AtLeastOneAudioSourceIsPlaying()
    {
        bool atLeasOneIsPlaying = false;

        foreach (AudioSource audioSource in this.textToSpeechList)
        {
            if (audioSource.isPlaying)
            {
                atLeasOneIsPlaying = true;
            }
        }
        return atLeasOneIsPlaying;
    }

    private IEnumerator WaitForPredicate(TutorialPredicate tutorialPredicate, TutorialAction tutorialAction)
    {
        while (!tutorialPredicate())
        {
            yield return null;
        }

        this.StartCoroutine(this.WaitForOtherAudioSources(tutorialAction));
    }

    private IEnumerator WaitForOtherAudioSources(TutorialAction tutorialAction)
    {
        yield return new WaitForSeconds(1f);

        while (this.AtLeastOneAudioSourceIsPlaying())
        {
            yield return null;
        }

        tutorialAction();
    }
}
