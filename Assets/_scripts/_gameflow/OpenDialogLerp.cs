using System.Collections;
using UnityEngine;
using Zenject;

public class OpenDialogLerp : MonoBehaviour
{
    [Inject]
    private EntityNameOnHoverController entityNameOnHoverController;

    [Inject]
    private MenuBarController menuBarController;

    [Inject]
    private ModelIndicator modelIndicator;

    [Header("Animation parameters")]
    public float durationInSeconds = 0.5f;
    public float distanceBehindModel = 1.0f;

    [Tooltip("Set this to true if the dialog shall be opened from the 'Menu Bar'.")]
    public bool animationStartsAtMenuBar = false;

    private void OnEnable()
    {
        if (this.animationStartsAtMenuBar)
        {
            this.StartCoroutine(this.Animate(this.menuBarController.gameObject.transform.position));
            return;
        }

        if (this.entityNameOnHoverController.IsAllocatedToAClass())
        {
            this.StartCoroutine(this.Animate(this.entityNameOnHoverController.GetCurrentEntity().transform.position));
        }
    }

    private IEnumerator Animate(Vector3 startPosition)
    {
        Vector3 fromCameraToModelWithoutY = this.modelIndicator.transform.position - Camera.main.transform.position;
        fromCameraToModelWithoutY.y = 0;
        fromCameraToModelWithoutY = fromCameraToModelWithoutY.normalized;

        Vector3 targetPosition = this.modelIndicator.transform.position + this.distanceBehindModel * fromCameraToModelWithoutY;

        float progressAsPercentage = 0f;

        while (progressAsPercentage < 1f)
        {
            float fromZeroToOne = (Mathf.Cos(Mathf.PI * progressAsPercentage / 2f + Mathf.PI)) + 1;

            this.transform.localScale = 0.1f * Vector3.one + 0.9f * fromZeroToOne * Vector3.one;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, progressAsPercentage);


            progressAsPercentage += Time.deltaTime * (1f / this.durationInSeconds);

            yield return null;
        }

        this.transform.localScale = Vector3.one;
        this.transform.position = targetPosition;
    }
}
