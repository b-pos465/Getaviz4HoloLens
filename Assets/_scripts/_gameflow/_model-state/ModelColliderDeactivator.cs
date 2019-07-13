using Logging;
using System.Collections.Generic;
using UnityEngine;

public class ModelColliderDeactivator : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private List<BoxCollider> boxColliders = new List<BoxCollider>();

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
        if (this.boxColliders == null || this.boxColliders.Count == 0)
        {
            this.GetComponentsInChildren<BoxCollider>(this.boxColliders);

            if (this.GetComponent<BoxCollider>() != null)
            {
                this.boxColliders.Remove(this.GetComponent<BoxCollider>());
            }
        }
    }
}
