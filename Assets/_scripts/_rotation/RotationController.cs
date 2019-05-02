using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class RotationController : MonoBehaviour
{
    [Inject]
    DragRecognizer dragRecognizer;

    public float torque = 1f;

    private Rigidbody rigidbody;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (this.dragRecognizer.IsDragging)
        {
            this.rigidbody.AddTorque(transform.up * this.torque * this.dragRecognizer.HorizontalDelta);
        }
    }
}
