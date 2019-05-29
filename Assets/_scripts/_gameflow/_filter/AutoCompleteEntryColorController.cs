using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(EventTrigger))]
class AutoCompleteEntryColorController : MonoBehaviour
{
    [Header("Colors for button state")]
    public Color defaultColor = new Color(1, 1, 1, 114f / 255f);
    public Color hoverColor = new Color(152f / 255f, 152f / 255f, 152f / 255f, 114f / 255f);

    private Image image;

    private void Start()
    {
        this.image = this.GetComponent<Image>();
        this.image.color = this.defaultColor;

        EventTrigger eventTrigger = this.GetComponent<EventTrigger>();
        this.AddPointerEnterEvent(eventTrigger);
        this.AddPointerExitEvent(eventTrigger);
    }

    private void AddPointerEnterEvent(EventTrigger eventTrigger)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { this.OnPointerEnter((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);
    }

    private void AddPointerExitEvent(EventTrigger eventTrigger)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { this.OnPointerExit((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.image.color = this.hoverColor;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.image.color = this.defaultColor;
    }
}