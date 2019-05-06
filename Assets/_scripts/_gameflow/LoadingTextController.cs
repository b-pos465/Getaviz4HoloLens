using UnityEngine;
using Zenject;

public class LoadingTextController : MonoBehaviour
{
    [Inject]
    private SpatialMappingRootIndicator spatialMappingRootIndicator;

    [Inject]
    private CursorIndicator cursorIndicator;

    private void Start()
    {
        this.cursorIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (this.spatialMappingRootIndicator.gameObject.transform.childCount > 0)
        {
            this.gameObject.SetActive(false);
            this.cursorIndicator.gameObject.SetActive(true);
        }
    }
}
