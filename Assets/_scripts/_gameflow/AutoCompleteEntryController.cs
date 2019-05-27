using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
public class AutoCompleteEntryController : MonoBehaviour
{
    [Inject]
    private FilterDialogController filterDialogController;

    public Color defaultColor;
    public Color hoverColor;
    public Color clickColor;

    private Image image;
    private Text text;

    private void Start()
    {
        this.image = this.GetComponent<Image>();
        this.text = this.GetComponentInChildren<Text>();
        this.image.color = this.defaultColor;
    }

    public void OnPointerUp()
    {
        this.filterDialogController.SelectEntry(this.text.text);
    }

    public void OnEnter()
    {
        this.image.color = this.hoverColor;
    }

    public void OnExit()
    {
        this.image.color = this.defaultColor;
    }
}
