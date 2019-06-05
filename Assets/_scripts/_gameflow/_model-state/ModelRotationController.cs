using UnityEngine;

public class ModelRotationController : MonoBehaviour
{
    public float anglePerFrame = 0.2f;

    void Update()
    {
        this.transform.Rotate(0, this.anglePerFrame, 0);
    }
}
