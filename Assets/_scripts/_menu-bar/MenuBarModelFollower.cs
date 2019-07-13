using System.Collections;
using UnityEngine;
using Zenject;

public class MenuBarModelFollower : MonoBehaviour
{
    [Inject]
    private ModelIndicator modelIndicator;

    public float distanceToModel = 0.2f;
    public float animationSpeed = 0.4f;

    private Vector3 targetPosition = Vector3.zero;

    private void OnEnable()
    {
        if (this.modelIndicator != null)
        {
            this.transform.position = this.CalculateNewPosition();
        }

        this.StartCoroutine(this.AsynchronousFollowModel());
    }

    private void OnDisable()
    {
        this.StopAllCoroutines();
    }

    private void Update()
    {
        this.targetPosition = this.CalculateNewPosition();
    }

    private Vector3 CalculateNewPosition()
    {
        Vector2 positionWithMinimalDistance = this.CalculatePositionWithMinimalDistance();

        Vector2 vectorFromModelCenterToNewPosition = (positionWithMinimalDistance - new Vector2(this.modelIndicator.transform.position.x, this.modelIndicator.transform.position.z)).normalized;
        Vector2 newPositionWithAPadding = positionWithMinimalDistance + this.distanceToModel * vectorFromModelCenterToNewPosition;

        return new Vector3(newPositionWithAPadding.x, this.modelIndicator.transform.position.y, newPositionWithAPadding.y);
    }

    private Vector2 CalculatePositionWithMinimalDistance()
    {
        Vector2 cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);

        Vector2[] possiblePositions = this.CalculatePossiblePositions();

        Vector2 positionWithMinimalDistance = possiblePositions[0];

        for (int i = 1; i < possiblePositions.Length; i++)
        {
            float bestDistanceSoFar = (cameraPosition - positionWithMinimalDistance).sqrMagnitude;
            float newDistance = (cameraPosition - possiblePositions[i]).sqrMagnitude;
            if (newDistance < bestDistanceSoFar)
            {
                positionWithMinimalDistance = possiblePositions[i];
            }
        }

        return positionWithMinimalDistance;
    }

    // For the placement of the "Menu Bar" the y value is ignored.
    private Vector2[] CalculatePossiblePositions()
    {
        BoxCollider b = this.modelIndicator.GetComponent<BoxCollider>();

        Vector3[] upperCorners = new Vector3[4];
        upperCorners[0] = this.modelIndicator.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f);
        upperCorners[1] = this.modelIndicator.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f);
        upperCorners[2] = this.modelIndicator.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f);
        upperCorners[3] = this.modelIndicator.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f);

        Vector3[] possiblePositions = new Vector3[4];
        possiblePositions[0] = Vector3.Lerp(upperCorners[0], upperCorners[1], 0.5f);
        possiblePositions[1] = Vector3.Lerp(upperCorners[1], upperCorners[2], 0.5f);
        possiblePositions[2] = Vector3.Lerp(upperCorners[2], upperCorners[3], 0.5f);
        possiblePositions[3] = Vector3.Lerp(upperCorners[3], upperCorners[0], 0.5f);

        Vector2[] possiblePositionsWithoutY = new Vector2[4];

        for (int i = 0; i < possiblePositions.Length; i++)
        {
            possiblePositionsWithoutY[i] = new Vector2(possiblePositions[i].x, possiblePositions[i].z);
        }

        return possiblePositionsWithoutY;
    }

    private IEnumerator AsynchronousFollowModel()
    {
        while (true)
        {
            Vector3 difference = this.targetPosition - this.transform.position;

            if (difference.sqrMagnitude > 0.00001f)
            {
                this.transform.position += difference * this.animationSpeed;
            }

            yield return new WaitForSeconds(0.016f);
        }
    }
}
