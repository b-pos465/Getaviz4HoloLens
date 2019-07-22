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
    private LoadingTextController loadingTextController;

    [Inject]
    private ModelStateController modelStateController;

    [Inject]
    private MetaphorPlacer metaphorPlacer;

    [Inject]
    private ModelIndicator modelIndicator;

    [Inject]
    private MenuBarController menuBarController;

    [Inject]
    private EntityNameOnHoverController entityNameOnHoverController;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    [Inject]
    private AutoCompleteController autoCompleteController;

    [Inject]
    private TutorialProgressBarController tutorialProgressBarController;

    [Header("Text to Speech")]
    public AudioSource[] textToSpeechList = new AudioSource[15];

    public float secondsBeforeATaskGetsRepeated = 8f;

    private delegate bool TutorialPredicate();
    private delegate void TutorialAction();

    private Coroutine speechCoroutine;

    private void Start()
    {
        this.StartCoroutine(this.WaitUntilSpatialMappingScanFinished());
    }

    private IEnumerator WaitUntilSpatialMappingScanFinished()
    {
        while (this.loadingTextController.gameObject.activeSelf)
        {
            yield return null;
        }

        this.GazeAtTheTableStep();
    }

    private void GazeAtTheTableStep()
    {
        this.PlayEveryFewSeconds(this.textToSpeechList[0]);

        this.metaphorPlacer.PlacementEnabled = false;
        this.metaphorPlacer.gameObject.SetActive(true);

        this.StartCoroutine(this.WaitForSuccessfullGaze());
    }

    private void PlayEveryFewSeconds(AudioSource audioSource)
    {
        if (this.speechCoroutine != null)
        {
            this.StopCoroutine(this.speechCoroutine);
        }

        this.speechCoroutine = this.StartCoroutine(this.PlayAsynchronousEveryFewSeconds(audioSource));
    }

    private IEnumerator PlayAsynchronousEveryFewSeconds(AudioSource audioSource)
    {
        while (true)
        {
            audioSource.Play();

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            yield return new WaitForSeconds(this.secondsBeforeATaskGetsRepeated);
        }
    }

    private IEnumerator WaitForSuccessfullGaze()
    {
        while (true)
        {
            if (this.modelStateController.ModelState == ModelState.PLACEMENT_VISIBLE)
            {
                this.tutorialProgressBarController.EnableProgressBar();

                bool holdWithoutInterruption = true;
                float time = 0f;

                while (time < 2.5f)
                {
                    if (this.modelStateController.ModelState != ModelState.PLACEMENT_VISIBLE)
                    {
                        this.tutorialProgressBarController.DisableProgressBar();
                        holdWithoutInterruption = false;
                        break;
                    }

                    time += Time.deltaTime;

                    yield return null;
                }

                if (holdWithoutInterruption)
                {
                    // Wait for a few more second to make sure we don't collide wiht the success sound from the tutorial progress bar.
                    yield return new WaitForSeconds(2f);

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

        this.PlayEveryFewSeconds(this.textToSpeechList[1]);
        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.INTERACTABLE,
                this.ClickOrSayTransformStep));
    }

    private void ClickOrSayTransformStep()
    {
        this.modelStateController.SwitchState(ModelState.TUTORIAL_TRANSFORM_ONLY);
        this.PlayEveryFewSeconds(this.textToSpeechList[2]);

        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.TRANSFORM,
                this.RotateTheModelStep));
    }

    private void RotateTheModelStep()
    {
        this.menuBarController.gameObject.SetActive(false);

        this.PlayEveryFewSeconds(this.textToSpeechList[3]);
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
        this.PlayEveryFewSeconds(this.textToSpeechList[4]);
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
        this.PlayEveryFewSeconds(this.textToSpeechList[5]);
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
        this.menuBarController.gameObject.SetActive(true);

        this.PlayEveryFewSeconds(this.textToSpeechList[6]);
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
        this.PlayEveryFewSeconds(this.textToSpeechList[7]);
        this.StartCoroutine(this.GazeAtAnEntityForAtLeastTwoSeconds());
    }

    private IEnumerator GazeAtAnEntityForAtLeastTwoSeconds()
    {
        while (true)
        {
            if (this.entityNameOnHoverController.IsShowingEntityName())
            {
                this.tutorialProgressBarController.EnableProgressBar();

                bool holdWithoutInterruption = true;
                float time = 0f;

                Entity startEntity = this.entityNameOnHoverController.GetCurrentEntity();

                while (time < 2f)
                {
                    if (!this.entityNameOnHoverController.IsShowingEntityName() || this.entityNameOnHoverController.GetCurrentEntity() != startEntity)
                    {
                        this.tutorialProgressBarController.DisableProgressBar();
                        holdWithoutInterruption = false;
                        break;
                    }

                    time += Time.deltaTime;
                    yield return null;
                }

                if (holdWithoutInterruption)
                {
                    // Wait for a few more second to make sure we don't collide wiht the success sound from the tutorial progress bar.
                    yield return new WaitForSeconds(1f);

                    this.StartCoroutine(this.WaitForOtherAudioSources(this.TapOnAClassStep));
                    break;
                }
            }
            yield return null;
        }
    }

    private void TapOnAClassStep()
    {
        this.PlayEveryFewSeconds(this.textToSpeechList[8]);
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
        this.PlayEveryFewSeconds(this.textToSpeechList[9]);
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
        this.PlayEveryFewSeconds(this.textToSpeechList[10]);
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
        this.PlayEveryFewSeconds(this.textToSpeechList[11]);
        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.INTERACTABLE,
                this.OpenTheFilterDialogStep
                    ));
    }

    private void OpenTheFilterDialogStep()
    {
        this.PlayEveryFewSeconds(this.textToSpeechList[12]);
        this.modelStateController.SwitchState(ModelState.TUTORIAL_FILTER_ONLY);

        this.StartCoroutine(
            this.WaitForPredicate(
                () => this.modelStateController.ModelState == ModelState.FILTER,
                this.NavigateThroughTheFilteringInterface
                ));
    }

    private void NavigateThroughTheFilteringInterface()
    {
        this.PlayEveryFewSeconds(this.textToSpeechList[13]);
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
        this.StopCoroutine(this.speechCoroutine);
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
