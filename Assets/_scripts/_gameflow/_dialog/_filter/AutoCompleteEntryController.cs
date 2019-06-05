using Model.Tree;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AutoCompleteEntryController : MonoBehaviour
{
    [Inject]
    private AutoCompleteController autoCompleteController;

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

    private void Update()
    {
        bool isSelected = this.entityNode == this.autoCompleteController.CurrentEntityNode;
        this.text.fontStyle = isSelected ? FontStyle.Bold : FontStyle.Normal;
    }

    private void SetEntityNode(EntityNode entityNode)
    {
        this.entityNode = entityNode;
        this.text.text = entityNode.Name;
        this.rawImage.texture = entityNode.IsLeaf() ? this.classSprite : this.packageSprite;
    }
}
