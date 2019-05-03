using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class RotationController : MonoBehaviour
{
    [Inject]
    DragRecognizer dragRecognizer;

    public float torque = 1f;

    private new Rigidbody rigidbody;

    private void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (this.dragRecognizer.IsDragging)
        {
            //this.rigidbody.AddTorque(this.transform.up * this.torque * this.dragRecognizer.CumulativeDelta);
        }
    }
}
