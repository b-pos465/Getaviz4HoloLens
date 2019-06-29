using Gaze;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class ScrollViewController : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private TapService tapService;

    [Inject]
    private KeywordToCommandService keywordToCommandService;

    [Inject]
    private ButtonClickSoundService buttonClickSoundService;

    [Header("Buttons")]
    public GameObject scrollUpButton;
    public GameObject scrollDownButton;

    [Header("Configuration")]
    public float range = 0.5f;
    public float durationInSeconds = 0.5f;

    private ScrollRect scrollRect;

    void Start()
    {
        this.scrollRect = this.GetComponent<ScrollRect>();
        this.tapService.Register(this.OnTap);
        this.keywordToCommandService.Register(GetavizKeyword.UP, this.OnUpVoiceCommand);
        this.keywordToCommandService.Register(GetavizKeyword.DOWN, this.OnDownVoiceCommand);
    }

    private void OnUpVoiceCommand()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.ScrollUp();
        }
    }

    private void OnDownVoiceCommand()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.ScrollDown();
        }
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (this.rayCaster.Hits)
        {
            if (this.HitsScrollDownButton())
            {
                this.ScrollDown();
            }
            else if (this.HitsScrollUpButton())
            {
                this.ScrollUp();
            }
        }
    }

    private bool HitsScrollDownButton()
    {
        return this.rayCaster.Target == this.scrollDownButton;
    }

    private bool HitsScrollUpButton()
    {
        return this.rayCaster.Target == this.scrollUpButton;
    }

    private void ScrollDown()
    {
        this.buttonClickSoundService.PlayButtonClickSound();
        float startPosition = this.scrollRect.verticalNormalizedPosition;
        float targetPosition = Mathf.Clamp01(startPosition - this.range);

        this.StartCoroutine(this.AnimateScroll(startPosition, targetPosition));
    }

    private void ScrollUp()
    {
        this.buttonClickSoundService.PlayButtonClickSound();
        float startPosition = this.scrollRect.verticalNormalizedPosition;
        float targetPosition = Mathf.Clamp01(startPosition + this.range);

        this.StartCoroutine(this.AnimateScroll(startPosition, targetPosition));
    }

    private IEnumerator AnimateScroll(float startPosition, float targetPositiion)
    {
        float progressAsPercentage = 0f;

        while (progressAsPercentage < 1f)
        {
            float fromZeroToOne = (Mathf.Cos(Mathf.PI * progressAsPercentage + Mathf.PI) + 1f) * 0.5f;

            float currentPosition = Mathf.Lerp(startPosition, targetPositiion, fromZeroToOne);
            Canvas.ForceUpdateCanvases();
            this.scrollRect.verticalNormalizedPosition = currentPosition;

            progressAsPercentage += Time.deltaTime * (1f / this.durationInSeconds);

            yield return null;
        }
    }
}
