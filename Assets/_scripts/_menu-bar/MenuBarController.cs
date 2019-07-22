using UnityEngine;
using Zenject;

public class MenuBarController : MonoBehaviour
{
    [Inject]
    private MenuBarDoneButtonIndicator menuBarDoneButtonIndicator;

    [Inject]
    private MenuBarFilterButtonIndicator menuBarFilterButtonIndicator;

    [Inject]
    private MenuBarAboutButtonIndicator menuBarAboutButtonIndicator;

    [Inject]
    private MenuBarLegendButtonIndicator menuBarLegendButtonIndicator;

    [Inject]
    private MenuBarTransformButtonIndicator menuBarTransformButtonIndicator;

    [Inject]
    private MenuBarBackPlateIndicator menuBarBackPlateIndicator;

    [Inject]
    private MenuBarButtonContainerIndicator menuBarButtonContainerIndicator;

    private MenuBarState menuBarState;

    private readonly int DEFAULT_BUTTON_COUNT = 4;

    void Start()
    {
        this.SwitchState(MenuBarState.DEFAULT);
        this.gameObject.SetActive(false);
    }

    public void SwitchState(MenuBarState menuBarState)
    {
        switch (menuBarState)
        {
            case MenuBarState.DEFAULT:
                this.menuBarDoneButtonIndicator.gameObject.SetActive(false);
                this.menuBarFilterButtonIndicator.gameObject.SetActive(true);
                this.menuBarTransformButtonIndicator.gameObject.SetActive(true);
                this.menuBarLegendButtonIndicator.gameObject.SetActive(true);
                this.menuBarAboutButtonIndicator.gameObject.SetActive(true);
               
                break;

            case MenuBarState.TUTORIAL_FILTER_ONLY:
                this.menuBarDoneButtonIndicator.gameObject.SetActive(false);
                this.menuBarFilterButtonIndicator.gameObject.SetActive(true);
                this.menuBarTransformButtonIndicator.gameObject.SetActive(false);
                this.menuBarLegendButtonIndicator.gameObject.SetActive(false);
                this.menuBarAboutButtonIndicator.gameObject.SetActive(false);
                break;

            case MenuBarState.TUTORIAL_TRANSFORM_ONLY:
                this.menuBarDoneButtonIndicator.gameObject.SetActive(false);
                this.menuBarFilterButtonIndicator.gameObject.SetActive(false);
                this.menuBarTransformButtonIndicator.gameObject.SetActive(true);
                this.menuBarLegendButtonIndicator.gameObject.SetActive(false);
                this.menuBarAboutButtonIndicator.gameObject.SetActive(false);
                break;

            case MenuBarState.DONE_ONLY:
                this.menuBarDoneButtonIndicator.gameObject.SetActive(true);
                this.menuBarFilterButtonIndicator.gameObject.SetActive(false);
                this.menuBarTransformButtonIndicator.gameObject.SetActive(false);
                this.menuBarLegendButtonIndicator.gameObject.SetActive(false);
                this.menuBarAboutButtonIndicator.gameObject.SetActive(false);
                break;
        }

        this.AdjustButtonPosition(menuBarState);
        this.AdjustButtonContainerPositions(menuBarState);
        this.AdjustBackplate(menuBarState);
    }

    private void AdjustButtonPosition(MenuBarState menuBarState)
    {
        float xWidthPerButton = 0.1f;
        float currentXOffset = 0f;

        foreach (Transform buttonTransform in this.menuBarButtonContainerIndicator.transform)
        {
            if (buttonTransform.gameObject.activeSelf)
            {
                buttonTransform.gameObject.transform.localPosition = new Vector3(currentXOffset, 0, 0);
                currentXOffset += xWidthPerButton;
            }
        }
    }

    private void AdjustBackplate(MenuBarState menuBarState)
    {
        float xScalePerButton = 0.1f;
        int buttonCount = menuBarState == MenuBarState.DEFAULT ? this.DEFAULT_BUTTON_COUNT : 1;

        Vector3 scale = this.menuBarBackPlateIndicator.transform.localScale;
        scale.x = xScalePerButton * buttonCount;
        this.menuBarBackPlateIndicator.transform.localScale = scale;
    }

    private void AdjustButtonContainerPositions(MenuBarState menuBarState)
    {
        int buttonCount = menuBarState == MenuBarState.DEFAULT ? this.DEFAULT_BUTTON_COUNT : 1;

        this.menuBarButtonContainerIndicator.transform.localPosition = new Vector3(-(buttonCount - 1) * 0.05f, 0, 0);
    }
}
