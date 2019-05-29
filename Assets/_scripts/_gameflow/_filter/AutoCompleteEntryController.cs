using Model.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class AutoCompleteEntryController : MonoBehaviour
{
    [Inject]
    private FilterDialogController filterDialogController;

    [Inject]
    private IconIndicator iconIndicator;

    [Header("Icon sprites")]
    public Texture packageSprite;
    public Texture classSprite;

    private EntityNode entityNode;

    public EntityNode EntityNode
    {
        get
        {
            return this.entityNode;
        }
        set
        {
            this.SetEntityNode(value);
        }
    }

    private Text text;
    private RawImage rawImage;

    private void Awake()
    {
        this.text = this.GetComponentInChildren<Text>();
        this.rawImage = this.iconIndicator.gameObject.GetComponent<RawImage>();
    }

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
        this.filterDialogController.SelectEntry(this.entityNode);
    }

    private void SetEntityNode(EntityNode entityNode)
    {
        this.entityNode = entityNode;
        this.text.text = entityNode.Name;
        this.rawImage.texture = entityNode.IsLeaf() ? this.classSprite : this.packageSprite;

        if (this.entityNode == this.filterDialogController.CurrentEntityNode)
        {
            this.text.fontStyle = FontStyle.Bold;
        }
    }
}
