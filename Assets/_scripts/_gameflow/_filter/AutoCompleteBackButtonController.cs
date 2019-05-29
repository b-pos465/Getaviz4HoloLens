using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(EventTrigger))]
class AutoCompleteBackButtonController : MonoBehaviour
{
    [Inject]
    private FilterDialogController filterDialogController;

    private void Start()
    {
        this.AddPointerUpEvent();
    }

    private void AddPointerUpEvent()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { this.OnPointerUp((PointerEventData)data); });

        this.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        this.filterDialogController.GoBack();
    }
}

