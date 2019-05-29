using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System.Collections;

public class LoadingTextController : MonoBehaviour
{
    [Inject]
    private SpatialMappingRootIndicator spatialMappingRootIndicator;

    [Inject]
    private CursorIndicator cursorIndicator;

    private Text text;

    private void Start()
    {
        this.text = this.GetComponentInChildren<Text>();
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
        this.cursorIndicator.gameObject.SetActive(true);
    }
}
