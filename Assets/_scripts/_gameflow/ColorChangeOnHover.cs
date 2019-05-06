using Gaze;
using System.Collections;
using UnityEngine;
using Zenject;

public class ColorChangeOnHover : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    public Color hoverColor;

    [Header("Durations")]
    public float fadeSelectDurationInSeconds = 0.3f;
    public float fadeDeselectDurationInSeconds = 0.2f;

    public bool enableFade = false;

    private Color defaultColor;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.defaultColor = this.meshRenderer.material.color;
    }

    void Update()
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.gameObject)
        {
            if (this.enableFade)
            {
                this.StartCoroutine(this.Fade(Direction.SELECT));
            }
            else
            {
                this.meshRenderer.material.SetColor("_Color", this.hoverColor);
            }
        }
        else if (this.meshRenderer.material.color == this.hoverColor)
        {
            if (this.enableFade)
            {
                this.StartCoroutine(this.Fade(Direction.DESELECT));
            }
            else
            {
                this.meshRenderer.material.SetColor("_Color", this.defaultColor);
            }
        }
    }

    private IEnumerator Fade(Direction direction)
    {
        float fadeDurationInSeconds = direction == Direction.SELECT ? this.fadeSelectDurationInSeconds : this.fadeDeselectDurationInSeconds;
        Color startColor = direction == Direction.SELECT ? this.defaultColor : this.hoverColor;
        Color targetColor = direction == Direction.SELECT ? this.hoverColor : this.defaultColor;

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
        SELECT, DESELECT
    }
}
