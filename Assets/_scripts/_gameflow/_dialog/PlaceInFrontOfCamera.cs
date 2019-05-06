using UnityEngine;

public class PlaceInFrontOfCamera : MonoBehaviour
{
    public bool moveWithCamera = true;
    public bool keepYStable = true;
    public float distanceToCamera = 2.5f;

    private void OnEnable()
    {
        this.PlaceInFront();
    }

    private void Update()
    {
        if (this.moveWithCamera)
        {
            this.PlaceInFront();
        }
    }

    private void PlaceInFront()
    {
        Vector3 forward = Camera.main.transform.forward;

        if (this.keepYStable)
        {
            forward.y = 0;
        }

        this.transform.position = Camera.main.transform.position + this.distanceToCamera * forward;
    }
}
