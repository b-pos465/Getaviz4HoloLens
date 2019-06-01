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

    private Text text;

    private void Start()
    {
        this.text = this.GetComponentInChildren<Text>();

        log.Debug("Disabling cursor ...");  
        this.cursorIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (this.spatialMappingRootIndicator.gameObject.transform.childCount > 0)
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
            this.text.color = new Color(1f, 1f, 1f, 1f - fromZeroToOne);

            progressAsPercentage += Time.deltaTime * (1f / fadeDurationInSeconds);

            yield return null;
        }

        this.gameObject.SetActive(false);

        log.Debug("Enabling cursor ...");
        this.cursorIndicator.gameObject.SetActive(true);
    }
}
