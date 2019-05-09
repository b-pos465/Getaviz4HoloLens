using UnityEngine;

public class CentralHoverController : MonoBehaviour
{
    private ColorChangeOnHover[] colorChangeOnHovers;

    private void Start()
    {
        this.colorChangeOnHovers = this.GetComponentsInChildren<ColorChangeOnHover>();
    }

    public void EnableHoverEffect()
    {
        this.SetEnabledTo(true);
    }

    public void DisableHoverEffect()
    {
        this.SetEnabledTo(false);
    }

    private void SetEnabledTo(bool state)
    {
        foreach (ColorChangeOnHover colorChangeOnHover in this.colorChangeOnHovers)
        {
            colorChangeOnHover.enabled = state;
        }
    }
}
