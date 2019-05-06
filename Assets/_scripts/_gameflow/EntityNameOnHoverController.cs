﻿using Gaze;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Canvas))]
public class EntityNameOnHoverController : MonoBehaviour
{
    [Inject]
    RayCaster rayCaster;

    public Vector3 offset = new Vector3(0.0f, 0.01f, 0.0f);

    private Text text;
    private Canvas canvas;

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
                this.AllocateToEntity(entity);
            }
        }

        this.AdjustRotationToCameraPosition();
    }

    private void AdjustRotationToCameraPosition()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(new Vector3(0, 180, 0));
    }

    private void AllocateToEntity(Entity entity)
    {
        this.canvas.enabled = true;

        string prefix = entity.type == "FAMIX.Class" ? "Class" : "Package";
        this.text.text = prefix + ": " + entity.name;

        Vector3 newPosition = entity.gameObject.transform.position + (entity.gameObject.GetComponent<Renderer>().bounds.size.y / 2 + 0.025f) * Vector3.up + this.offset;
        this.transform.position = newPosition;
    }
}
