using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
public class TutorialProgressBarController : MonoBehaviour
{
    [Inject]
    private CursorIndicator cursorIndicator;

    public AudioSource successSound;

    private CanvasGroup canvasGroup;
    private Slider slider;

    private void Start()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        this.slider = this.GetComponentInChildren<Slider>();

        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.AdjustPosition();
        this.AdjustRotationToCameraPosition();
    }

    private void AdjustPosition()
    {
        this.transform.position = this.cursorIndicator.transform.position + -0.5f * Camera.main.transform.forward + new Vector3(0, 0.1f, 0);
    }

    private void AdjustRotationToCameraPosition()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(new Vector3(0, 180, 0));
    }

    public void EnableProgressBar()
    {
        this.gameObject.SetActive(true);
        this.StopAllCoroutines();
        this.StartCoroutine(this.StartAsyncProgress());
    }

    public void DisableProgressBar()
    {
        this.StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    private IEnumerator StartAsyncProgress()
    {
        this.slider.value = 0f;
        this.canvasGroup.alpha = 0f;

        yield return new WaitForSeconds(0.5f);

        this.canvasGroup.alpha = 1f;

        float progressDurationInSeconds = 2f;
        float progress = 0f;

        while (progress < 1f)
        {
            this.slider.value = progress;
            progress += Time.deltaTime * (1f / progressDurationInSeconds);

            yield return null;
        }

        this.slider.value = 1f;

        this.successSound.Play();
        this.StartCoroutine(this.FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float fadeDurationInSeconds = 0.3f;
        float progressAsPercentage = 0f;

        while (progressAsPercentage < 1f)
        {
            float fromZeroToOne = (Mathf.Cos(Mathf.PI * progressAsPercentage + Mathf.PI) + 1f) * 0.5f;
            this.canvasGroup.alpha = 1f - fromZeroToOne;

            progressAsPercentage += Time.deltaTime * (1f / fadeDurationInSeconds);

            yield return null;
        }

        this.canvasGroup.alpha = 0f;

        while (this.successSound.isPlaying)
        {
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
