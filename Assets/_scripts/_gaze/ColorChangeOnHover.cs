using Gaze;
using UnityEngine;
using Zenject;

public class ColorChangeOnHover : MonoBehaviour
{
    [Inject]
    private RayCaster rayCaster;

    public Color hoverColor;

    private Color defaultColor;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();
        this.defaultColor = this.meshRenderer.material.color;
    }

    void Update()
    {
        if (this.rayCaster.Hits && this.rayCaster.Target == this.gameObject)
        {
            this.meshRenderer.material.SetColor("_Color", this.hoverColor);
        }
        else
        {
            this.meshRenderer.material.SetColor("_Color", this.defaultColor);
        }
    }
}
