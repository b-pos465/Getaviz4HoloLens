using SpatialMapping;
using System.Collections;
using UnityEngine;
using Zenject;

public class TutorialStateController : MonoBehaviour
{
    [Inject]
    private ModelStateController modelStateController;

    [Inject]
    private MetaphorPlacer metaphorPlacer;

    [Inject]
    private ModelIndicator modelIndicator;

    [Header("Text to Speech")]
    public AudioSource[] textToSpeechList = new AudioSource[15];

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
        this.StartCoroutine(this.WaitForSuccessfullPlacement());
    }

    private IEnumerator WaitForSuccessfullPlacement()
    {
        while (this.modelStateController.ModelState != ModelState.INTERACTABLE)
        {
            yield return null;
        }

        while (this.AtLeastOneAudioSourceIsPlaying())
        {
            yield return null;
        }

        this.ClickOrSayTransformStep();
    }

    private void ClickOrSayTransformStep()
    {
        this.modelStateController.SwitchState(ModelState.TUTORIAL_TRANSFORM_ONLY);
        this.textToSpeechList[2].Play();

        this.StartCoroutine(this.WaitForTransformToOpen());
    }

    private IEnumerator WaitForTransformToOpen()
    {
        while (this.modelStateController.ModelState != ModelState.TRANSFORM)
        {
            yield return null;
        }

        while (this.AtLeastOneAudioSourceIsPlaying())
        {
            yield return null;
        }

        this.RotateTheModelStep();
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

        yield return new WaitForSeconds(1f);

        while (this.AtLeastOneAudioSourceIsPlaying())
        {
            yield return null;
        }

        this.ScaleTheModelStep();
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

        yield return new WaitForSeconds(1f);

        while (this.AtLeastOneAudioSourceIsPlaying())
        {
            yield return null;
        }

        this.TapAndHoldStep();
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

        yield return new WaitForSeconds(1f);

        while (this.AtLeastOneAudioSourceIsPlaying())
        {
            yield return null;
        }

        this.ClickOrSayDone();
    }

    private void ClickOrSayDone()
    {

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
}
