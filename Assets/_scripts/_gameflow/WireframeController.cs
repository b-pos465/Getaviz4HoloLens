using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(LineRenderer))]
public class WireframeController : MonoBehaviour
{
    private MeshRenderer defaultRenderer;
    private LineRenderer lineRenderer;


    private void OnEnable()
    {
        if (this.lineRenderer == null || this.defaultRenderer == null)
        {
            this.LoadComponents();
        }

        this.SwitchBetweenDefaultAndLineRenderer();
    }
    private void LoadComponents()
    {
        this.defaultRenderer = this.GetComponent<MeshRenderer>();
        this.lineRenderer = this.GetComponent<LineRenderer>();
    }

    private void OnDisable()
    {
        this.SwitchBetweenDefaultAndLineRenderer();
    }

    private void SwitchBetweenDefaultAndLineRenderer()
    {
        this.lineRenderer.enabled = !this.lineRenderer.enabled;
        this.defaultRenderer.enabled = !this.defaultRenderer.enabled;

        if (this.lineRenderer.positionCount != 16)
        {
            this.SetVerticesForLineRenderer();
        }
    }

    private void SetVerticesForLineRenderer()
    {
        Vector3 localSize = Vector3.one;
        Vector3[] corners = new Vector3[16];

        // Right side
        corners[0] = localSize * 0.5f;
        corners[1] = corners[0] + new Vector3(0, -localSize.y, 0);
        corners[2] = corners[1] + new Vector3(0, 0, -localSize.z);
        corners[3] = corners[2] + new Vector3(0, localSize.y, 0);
        corners[4] = corners[0];

        // Back side
        corners[5] = corners[0] + new Vector3(-localSize.x, 0, 0);
        corners[6] = corners[5] + new Vector3(0, -localSize.y, 0);
        corners[7] = corners[1];
        corners[8] = corners[6];

        // Left side
        corners[9] = corners[8] + new Vector3(0, 0, -localSize.z);
        corners[10] = corners[9] + new Vector3(0, localSize.y, 0);
        corners[11] = corners[5];
        corners[12] = corners[10];

        // Front side
        corners[13] = corners[3];
        corners[14] = corners[2];
        corners[15] = corners[9];

        lineRenderer.positionCount = corners.Length;
        lineRenderer.SetPositions(corners);
    }
}
