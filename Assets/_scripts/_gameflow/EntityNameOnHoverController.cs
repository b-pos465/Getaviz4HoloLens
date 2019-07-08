using Gaze;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class EntityNameOnHoverController : MonoBehaviour
{
    [Inject]
    RayCaster rayCaster;

    [Header("Font Distance Scaling")]
    public int minFontSize = 15;
    public int maxFontSize = 35;

    public float lowerDistanceThreshold = 0.1f;
    public float upperDistanceThreshold = 2f;

    [Header("Offsets")]
    public Vector3 packageOffset = new Vector3(0.0f, 0.03f, 0.0f);
    public Vector3 nonPackageOffset = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("Strategy")]
    public Strategy strategy = Strategy.RELATIVE_TO_ENTITY;

    private Text text;
    private Canvas canvas;

    private Entity lastEntityAllocatedTo = null;

    private void Start()
    {
        this.text = this.GetComponentInChildren<Text>();
        this.canvas = this.GetComponent<Canvas>();
    }

    private void Update()
    {
        this.canvas.enabled = false;

        if (this.rayCaster.Target != null)
        {
            Entity entity = this.rayCaster.Target.GetComponent<Entity>();
            if (entity != null)
            {
                this.SetPosition(entity);
                this.SetLabel(entity);
                this.lastEntityAllocatedTo = entity;
            }
            else
            {
                this.Deallocate();
            }
        }

        this.AdjustRotationToCameraPosition();
        this.AdjustFontSize();
    }

    private void AdjustRotationToCameraPosition()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void SetPosition(Entity entity)
    {
        this.canvas.enabled = true;
        Vector3 newPosition = this.transform.position;

        if (entity.IsPackage())
        {
            newPosition = this.rayCaster.HitPoint + this.packageOffset;
        }
        else if (this.strategy == Strategy.RELATIVE_TO_POINTER)
        {
            newPosition = this.rayCaster.HitPoint + this.nonPackageOffset;
            newPosition.x = entity.transform.position.x;
            newPosition.z = entity.transform.position.z;
        }
        else if (this.strategy == Strategy.RELATIVE_TO_ENTITY)
        {
            newPosition = entity.gameObject.transform.position + (entity.gameObject.GetComponent<Renderer>().bounds.extents.y + 0.025f) * Vector3.up + this.nonPackageOffset;
        }
        else if (this.strategy == Strategy.STAY_ON_ENTER_HITPOINT)
        {
            if (entity != this.lastEntityAllocatedTo)
            {
                newPosition = this.rayCaster.HitPoint + this.nonPackageOffset;
            }
        }

        this.transform.position = newPosition;
    }

    private void SetLabel(Entity entity)
    {
        string prefix = entity.IsClass() ? "Class" : "Package";
        string label = prefix + ": " + entity.name;
        this.text.text = label;
    }

    private void AdjustFontSize()
    {
        float distance = (this.transform.position - Camera.main.transform.position).sqrMagnitude;

        float lerp = (distance - this.lowerDistanceThreshold) / this.upperDistanceThreshold;
        this.text.fontSize = (int)Mathf.Lerp(this.minFontSize, this.maxFontSize, lerp);
    }

    private void Deallocate()
    {
        this.lastEntityAllocatedTo = null;
    }

    public bool IsShowingEntityName ()
    {
        return this.canvas.enabled;
    }

    public Entity GetCurrentEntity()
    {
        return this.lastEntityAllocatedTo;
    }

    public enum Strategy
    {
        RELATIVE_TO_ENTITY,
        RELATIVE_TO_POINTER,
        STAY_ON_ENTER_HITPOINT
    }
}
