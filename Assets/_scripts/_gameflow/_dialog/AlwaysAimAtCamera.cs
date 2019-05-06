using UnityEngine;

public class AlwaysAimAtCamera : MonoBehaviour
{
    void Update()
    {
        this.AdjustRotationToCameraPosition();
    }

    private void AdjustRotationToCameraPosition()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(new Vector3(0, 180, 0));
    }
}
