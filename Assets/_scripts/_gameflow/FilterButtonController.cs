using UnityEngine;
using Zenject;
using static HoloToolkit.Unity.UX.AppBar;

public class FilterButtonController : MonoBehaviour
{
    private static readonly string ICON = "ObjectCollectionScatter";
    private static readonly string NAME = "Filter";

    [Inject]
    private ModelStateController modelStateController;

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
        this.modelStateController.SwitchState(ModelState.FILTER);
    }

    public ButtonTemplate ProvideTemplate()
    {
        return this.buttonTemplate;
    }
}
