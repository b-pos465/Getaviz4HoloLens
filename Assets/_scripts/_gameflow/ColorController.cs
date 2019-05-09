using System.Collections;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public Color hoverColor;

    [Header("Durations")]
    public float fadeSelectDurationInSeconds = 0.3f;
    public float fadeDeselectDurationInSeconds = 0.2f;

    public bool enableFade = false;

    private Color defaultColor;
    private MeshRenderer meshRenderer;

    void Start()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.defaultColor = this.meshRenderer.material.color;
    }

    void Update()
    {

    }

    private IEnumerator Fade(Direction direction)
    {
        float fadeDurationInSeconds = direction == Direction.ENTER ? this.fadeSelectDurationInSeconds : this.fadeDeselectDurationInSeconds;
        Color startColor = direction == Direction.ENTER ? this.defaultColor : this.hoverColor;
        Color targetColor = direction == Direction.ENTER ? this.hoverColor : this.defaultColor;

        float progressAsPercentage = 0f;

        while (progressAsPercentage < 1f)
        {
            float fromZeroToOne = (Mathf.Cos(Mathf.PI * progressAsPercentage + Mathf.PI) + 1f) * 0.5f;

            Color currentColor = Color.Lerp(startColor, targetColor, fromZeroToOne);
            this.meshRenderer.material.SetColor("_Color", currentColor);

            progressAsPercentage += Time.deltaTime * (1f / fadeDurationInSeconds);

            yield return null;
        }

        this.meshRenderer.material.SetColor("_Color", targetColor);
    }

    enum Direction
    {
        ENTER, LEAVE
    }
}
