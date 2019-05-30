using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotationController : MonoBehaviour
{
    public float anglePerFrame = Mathf.PI / 2.0f;

    void Update()
    {
        this.transform.Rotate(0, this.anglePerFrame, 0);
    }
}
