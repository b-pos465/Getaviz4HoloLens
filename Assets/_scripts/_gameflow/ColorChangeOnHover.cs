using Gaze;
using Model;
using UnityEngine;
using Zenject;

public class ColorChangeOnHover : MonoBehaviour
{
    private static readonly string COLOR_STRING = "_Color";

    [Inject]
    private RayCaster rayCaster;

    public Color hoverColor;


    private GameObject currentTarget;
    private Color defaultColor;

    private void Update()
    {
        if (this.HitsNewEntity())
        {
            this.ResetCurrentTargetColor();
            this.SetColorForNewEntityAndCacheReferences();
        }
        else if (this.HitsNoEntity())
        {
            if (this.currentTarget != null)
            {
                float distanceCameraToCurrentTarget = Vector3.Distance(Camera.main.transform.position, this.currentTarget.transform.position);
                Vector3 approximatedRayPoint = Camera.main.transform.position + this.rayCaster.Direction * distanceCameraToCurrentTarget;
                Vector3 closesPointOnCurrentTarget = this.currentTarget.GetComponent<BoxCollider>().ClosestPoint(approximatedRayPoint);
                float approximatedDistanceBetweenCurrentTargetAndRay = Vector3.Distance(closesPointOnCurrentTarget, approximatedRayPoint);

                if (approximatedDistanceBetweenCurrentTargetAndRay > 0.03f)
                {
                    this.ResetCurrentTargetColor();
                }
            }
        }
    }

    private bool HitsNewEntity()
    {
        return this.rayCaster.Hits && this.rayCaster.Target.GetComponent<Entity>() != null && this.rayCaster.Target != this.currentTarget;
    }

    private bool HitsNoEntity()
    {
        return (this.rayCaster.Hits && this.rayCaster.Target.GetComponent<Entity>() == null) || !this.rayCaster.Hits;
    }

    private void OnDisable()
    {
        this.ResetCurrentTargetColor();
    }

    private void ResetCurrentTargetColor()
    {
        if (this.currentTarget != null)
        {
            MeshRenderer oldMeshRenderer = this.currentTarget.GetComponent<MeshRenderer>();
            oldMeshRenderer.material.SetColor(COLOR_STRING, this.defaultColor);
            this.currentTarget = null;
        }
    }

    private void SetColorForNewEntityAndCacheReferences()
    {
        MeshRenderer meshRenderer = this.rayCaster.Target.GetComponent<MeshRenderer>();
        this.defaultColor = meshRenderer.material.GetColor(COLOR_STRING);
        meshRenderer.material.SetColor(COLOR_STRING, this.hoverColor);

        this.currentTarget = this.rayCaster.Target;
    }
}
