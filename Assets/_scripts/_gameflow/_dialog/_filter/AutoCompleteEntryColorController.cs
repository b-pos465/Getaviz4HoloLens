using Gaze;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
class AutoCompleteEntryColorController : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    [Header("Colors for button state")]
    public Color defaultColor = new Color(1, 1, 1, 114f / 255f);
    public Color hoverColor = new Color(152f / 255f, 152f / 255f, 152f / 255f, 114f / 255f);

    private Image image;

    private void Start()
    {
        this.image = this.GetComponent<Image>();
        this.image.color = this.defaultColor;

        EventTrigger eventTrigger = this.GetComponent<EventTrigger>();
    }

    private void Update()
    {
        if (!this.rayCaster.Hits)
        {
            return;
        }

        bool isGazingOnThisEntry = this.rayCaster.Target == this.gameObject;

        this.image.color = isGazingOnThisEntry ? this.hoverColor : this.defaultColor;
    }
}