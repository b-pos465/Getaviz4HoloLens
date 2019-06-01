using Logging;
using UnityEngine;

public class ModelHoverController : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private ColorChangeOnHover[] colorChangeOnHovers;


    private void OnEnable()
    {
        this.SetEnabledTo(true);
    }

    private void OnDisable()
    {
        this.SetEnabledTo(false);
    }

    private void SetEnabledTo(bool state)
    {
        this.UpdateReferencesIfNecessary();
        log.Debug("Setting {} ColorChangeOnHover components to state: {}.", this.colorChangeOnHovers.Length, state);

        foreach (ColorChangeOnHover colorChangeOnHover in this.colorChangeOnHovers)
        {
            colorChangeOnHover.enabled = state;
        }
    }

    private void UpdateReferencesIfNecessary()
    {
        if (this.colorChangeOnHovers == null || this.colorChangeOnHovers.Length == 0)
        {
            this.colorChangeOnHovers = this.GetComponentsInChildren<ColorChangeOnHover>();
        }
    }
}
