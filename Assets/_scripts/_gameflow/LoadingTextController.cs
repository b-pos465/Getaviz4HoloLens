using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System.Collections;
using Logging;

public class LoadingTextController : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    [Inject]
    private SpatialMappingRootIndicator spatialMappingRootIndicator;

    [Inject]
    private CursorIndicator cursorIndicator;

    [Inject]
    private MetaphorPlacerIndicator metaphorPlacerIndicator;

    public float minimumVisibilityTimeInSeconds = 3f;

    private CanvasGroup canvasGroup;

    // This is used to makesure that the text is at least visible for a few seconds.
    private float startTime;

    private void Start()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();

        log.Debug("Disabling cursor ...");
        this.cursorIndicator.gameObject.SetActive(false);

        this.startTime = Time.time;
    }

    void Update()
    {
        if (this.spatialMappingRootIndicator.gameObject.transform.childCount > 0 && Time.time - this.startTime > this.minimumVisibilityTimeInSeconds)
        {
            this.StartCoroutine(this.FadeOut());
        }
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

        this.gameObject.SetActive(false);

        log.Debug("Enabling cursor ...");
        this.cursorIndicator.gameObject.SetActive(true);
        this.metaphorPlacerIndicator.gameObject.SetActive(true);
    }
}
