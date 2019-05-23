using UnityEngine;
using Zenject;

public class AdjustModeController : MonoBehaviour
{
    [Inject]
    private CentralHoverController centralHoverController;

    [Inject]
    private EntityNameOnHoverIndicator entityNameOnHoverIndicator;

    [Inject]
    private SourceCodeDialogOnClick sourceCodeDialogOnClick;

    public void EnableAdjustMode()
    {
        this.entityNameOnHoverIndicator.gameObject.SetActive(false);
        this.sourceCodeDialogOnClick.Disable();
        this.centralHoverController.DisableHoverEffect();
    }

    public void DisableAdjustMode()
    {
        this.entityNameOnHoverIndicator.gameObject.SetActive(true);
        this.sourceCodeDialogOnClick.Enable();
        this.centralHoverController.EnableHoverEffect();
    }
}
