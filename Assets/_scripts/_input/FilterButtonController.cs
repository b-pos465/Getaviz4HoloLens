using UnityEngine;
using Zenject;
using static HoloToolkit.Unity.UX.AppBar;

public class FilterButtonController : MonoBehaviour
{
    private static readonly string ICON = "ObjectCollectionScatter";
    private static readonly string NAME = "Filter";

    [Inject]
    private ModelIndicator modelIndicator;

    [Inject]
    private FilterDialogIndicator filterDialogIndicator;

    private ButtonTemplate buttonTemplate;

    private void Awake()
    {
        this.buttonTemplate = new ButtonTemplate(
                        ButtonTypeEnum.Custom,
                        NAME,
                        ICON,
                        NAME,
                        0,
                        0);
    }

    public void OnTap()
    {
        this.modelIndicator.gameObject.SetActive(false);
        this.filterDialogIndicator.gameObject.SetActive(true);
    }

    public ButtonTemplate ProvideTemplate()
    {
        return this.buttonTemplate;
    }
}
