using Logging;
using UnityEngine;

public class ModelColliderDeactivator : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private BoxCollider[] boxColliders;

    private void OnEnable()
    {
        this.SetEnabledTo(false);
    }

    private void OnDisable()
    {
        this.SetEnabledTo(true);
    }

    private void SetEnabledTo(bool state)
    {
        log.Debug("Setting all colliders to state: {}.", state);
        this.UpdateColliderReferenceIfNecessary();

        foreach (BoxCollider boxCollider in this.boxColliders)
        {
            boxCollider.enabled = state;
        }
    }

    private void UpdateColliderReferenceIfNecessary()
    {
        if (this.boxColliders == null || this.boxColliders.Length == 0)
        {
            this.boxColliders = this.GetComponentsInChildren<BoxCollider>();
        }
    }
}
