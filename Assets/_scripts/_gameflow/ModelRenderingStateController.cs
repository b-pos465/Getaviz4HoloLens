using Logging;
using UnityEngine;

public class ModelRenderingStateController : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private WireframeController[] wireframeControllers;
    private MeshRenderer[] meshRenderers;

    public void SwitchState(ModelRenderingState modelRenderingState)
    {
        this.UpdateReferencesIfNecessary();

        if (modelRenderingState == ModelRenderingState.INVISIBLE)
        {
            this.SetWireframesTo(false);
            this.SetMeshRenderersTo(false);
        }
        else if (modelRenderingState == ModelRenderingState.WIREFRAME)
        {
            this.SetWireframesTo(true);
            this.SetMeshRenderersTo(false);
        }
        else if (modelRenderingState == ModelRenderingState.SOLID)
        {
            this.SetWireframesTo(false);
            this.SetMeshRenderersTo(true);
        }
    }

    private void SetWireframesTo(bool state)
    {
        log.Debug("Setting {} WireframeControllers to state: {}.", this.wireframeControllers.Length, state);

        foreach (WireframeController wireframeController in this.wireframeControllers)
        {
            wireframeController.enabled = state;
        }
    }

    private void SetMeshRenderersTo(bool state)
    {
        log.Debug("Setting {} MeshRenderers to state: {}.", this.meshRenderers.Length, state);

        foreach (MeshRenderer meshRenderer in this.meshRenderers)
        {
            meshRenderer.enabled = state;
        }
    }

    private void UpdateReferencesIfNecessary()
    {
        if (this.wireframeControllers == null || this.wireframeControllers.Length == 0)
        {
            this.wireframeControllers = this.GetComponentsInChildren<WireframeController>();
        }

        if (this.meshRenderers == null || this.meshRenderers.Length == 0)
        {
            this.meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        }
    }
}

public enum ModelRenderingState
{
    INVISIBLE, WIREFRAME, SOLID
}
