using Gaze;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class SourceCodeScrollViewController : MonoBehaviour
{
    [Inject]
    private SourceCodeScrollDownButtonIndicator sourceCodeScrollDownButtonIndicator;

    [Inject]
    private SourceCodeScrollUpButtonIndicator SourceCodeScrollUpButtonIndicator;

    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private TapService tapService;

    public float range = 0.5f;
    public float durationInSeconds = 0.5f;

    private ScrollRect scrollRect;

    void Start()
    {
        this.scrollRect = this.GetComponent<ScrollRect>();
        this.tapService.Register(this.OnTap);
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
        return this.rayCaster.Target == this.sourceCodeScrollDownButtonIndicator.gameObject;
    }

    private bool HitsScrollUpButton()
    {
        return this.rayCaster.Target == this.SourceCodeScrollUpButtonIndicator.gameObject;
    }

    private void ScrollDown()
    {
        float startPosition = this.scrollRect.verticalNormalizedPosition;
        float targetPosition = Mathf.Clamp01(startPosition - this.range);

        this.StartCoroutine(this.AnimateScroll(startPosition, targetPosition));
    }

    private void ScrollUp()
    {
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
