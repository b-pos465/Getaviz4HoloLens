using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


class GetavizScrollRect : ScrollRect
{
    private readonly float animateChangePerFrame = 0.05f;
    private float verticalNormalizedTargetPosition = 1f;

    private bool coroutineIsRunning = false;

    public override void OnDrag(PointerEventData eventData)
    {
        if (!this.coroutineIsRunning)
        {
            this.StartCoroutine(this.AnimateScroll());
        }

        this.verticalNormalizedTargetPosition -= eventData.scrollDelta.y;
        this.verticalNormalizedTargetPosition = Mathf.Clamp01(this.verticalNormalizedTargetPosition);
    }

    private IEnumerator AnimateScroll()
    {
        this.coroutineIsRunning = true;

        while (true)
        {
            if ((Mathf.Abs(this.verticalNormalizedTargetPosition - this.verticalNormalizedPosition) > this.animateChangePerFrame))
            {
                Canvas.ForceUpdateCanvases();
                if (this.verticalNormalizedTargetPosition > this.verticalNormalizedPosition)
                {
                    this.verticalNormalizedPosition += this.animateChangePerFrame;
                }
                else
                {
                    this.verticalNormalizedPosition -= this.animateChangePerFrame;
                }
            } 
            yield return null;
        }
    }
}
