using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRenderingStateController : MonoBehaviour
{
    public bool drawWireframe = false;

    private bool flagInLastFrame = false;

    private RenderingState state;
    private WireframeController[] wireframeController;

    private void SetRenderingState(RenderingState renderingState)
    {
        this.state = renderingState;
        // TODO
    }

    private void Start()
    {
        this.wireframeController = this.GetComponentsInChildren<WireframeController>();
    }

    private void Update()
    {
        if (this.drawWireframe != this.flagInLastFrame)
        {
            this.ToggleWireframeEffect();
            this.flagInLastFrame = this.drawWireframe;
        }
    }

    private void ToggleWireframeEffect()
    {
        this.SetEnabledTo(this.drawWireframe);
    }

    private void SetEnabledTo(bool state)
    {
        foreach (WireframeController wireframeController in this.wireframeController)
        {
            wireframeController.enabled = state;
        }
    }

    public enum RenderingState
    {
        WIREFRAME, SOLID
    }
}
