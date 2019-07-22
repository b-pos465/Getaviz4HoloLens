using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.UX;
using Logging;
using System;
using UnityEngine;
using Zenject;

public class ModelStateController : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    [Inject]
    private EntityNameOnHoverIndicator entityNameOnHoverIndicator;

    [Inject]
    private MenuBarController menuBarController;

    [Inject]
    private FilterDialogIndicator filterDialogIndicator;

    [Inject]
    private AboutDialogIndicator infoDialogIndicator;

    [Inject]
    private LegendDialogIndicator legendDialogIndicator;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    private ColorChangeOnHover colorChangeOnHover;
    private BoundingBoxRig boundingBoxRig;
    private HandDraggable handDraggable;
    private SourceCodeDialogOnClick sourceCodeDialogOnClick;
    private ModelColliderDeactivator modelColliderDeactivator;
    private ModelRenderingStateController modelRenderingStateController;
    private ModelRotationController modelRotationController;


    public ModelState ModelState { get; private set; }

    private void Start()
    {
        this.colorChangeOnHover = this.GetComponent<ColorChangeOnHover>();
        this.boundingBoxRig = this.GetComponent<BoundingBoxRig>();
        this.handDraggable = this.GetComponent<HandDraggable>();
        this.sourceCodeDialogOnClick = this.GetComponent<SourceCodeDialogOnClick>();
        this.modelColliderDeactivator = this.GetComponent<ModelColliderDeactivator>();
        this.modelRenderingStateController = this.GetComponent<ModelRenderingStateController>();
        this.modelRotationController = this.GetComponent<ModelRotationController>();

        this.SetInitialState();
    }

    public void SwitchState(ModelState newState)
    {
        if (this.ModelState == newState)
        {
            return;
        }

        if (this.TryingToGetBackToPlacementState(newState))
        {
            throw new ArgumentException(String.Format("I can not change the state back to {0} as the model can only be placed once.", newState));
        }

        log.Debug("Setting model state to {}.", newState);
        this.ModelState = newState;

        if (newState == ModelState.PLACEMENT_INVISIBLE)
        {
            this.modelRenderingStateController.SwitchState(ModelRenderingState.INVISIBLE);
            this.modelRotationController.enabled = false;
        }
        else if (newState == ModelState.PLACEMENT_VISIBLE)
        {
            this.modelRenderingStateController.SwitchState(ModelRenderingState.WIREFRAME);
            this.modelRotationController.enabled = true;
        }
        else if (newState == ModelState.INTERACTABLE)
        {
            this.colorChangeOnHover.enabled = true;
            this.boundingBoxRig.enabled = true;
            this.boundingBoxRig.Deactivate();
            this.handDraggable.enabled = true;
            this.sourceCodeDialogOnClick.enabled = true;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(true);
            this.menuBarController.SwitchState(MenuBarState.DEFAULT);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.TRANSFORM)
        {
            this.boundingBoxRig.Activate();
            this.colorChangeOnHover.enabled = false;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(false);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(true);
            this.menuBarController.SwitchState(MenuBarState.DONE_ONLY);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.FILTER)
        {
            this.colorChangeOnHover.enabled = true;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(false);
            this.filterDialogIndicator.gameObject.SetActive(true);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.SOURCECODE)
        {
            this.colorChangeOnHover.enabled = true;
            this.sourceCodeDialogOnClick.enabled = true;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(false);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(true);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.ABOUT)
        {
            this.colorChangeOnHover.enabled = true;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(false);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(true);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.LEGEND)
        {
            // FIXME
            this.colorChangeOnHover.enabled = true;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(false);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(true);
        }
        else if (newState == ModelState.TUTORIAL_TRANSFORM_ONLY)
        {
            this.colorChangeOnHover.enabled = false;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(false);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(true);
            this.menuBarController.SwitchState(MenuBarState.TUTORIAL_TRANSFORM_ONLY);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.TUTORIAL_GAZE_ONLY)
        {
            this.colorChangeOnHover.enabled = true;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(false);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.TUTORIAL_SOURCECODE_ONLY)
        {
            this.colorChangeOnHover.enabled = true;
            this.sourceCodeDialogOnClick.enabled = true;
            this.entityNameOnHoverIndicator.gameObject.SetActive(true);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(false);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
        else if (newState == ModelState.TUTORIAL_FILTER_ONLY)
        {
            this.colorChangeOnHover.enabled = false;
            this.sourceCodeDialogOnClick.enabled = false;
            this.entityNameOnHoverIndicator.gameObject.SetActive(false);
            this.modelColliderDeactivator.enabled = false;
            this.modelRenderingStateController.SwitchState(ModelRenderingState.SOLID);
            this.modelRotationController.enabled = false;
            this.menuBarController.gameObject.SetActive(true);
            this.menuBarController.SwitchState(MenuBarState.TUTORIAL_FILTER_ONLY);
            this.filterDialogIndicator.gameObject.SetActive(false);
            this.sourceCodeDialogIndicator.gameObject.SetActive(false);
            this.infoDialogIndicator.gameObject.SetActive(false);
            this.legendDialogIndicator.gameObject.SetActive(false);
        }
    }

    private bool TryingToGetBackToPlacementState(ModelState newState)
    {
        return (newState == ModelState.PLACEMENT_VISIBLE && this.ModelState != ModelState.PLACEMENT_INVISIBLE) ||
            (newState == ModelState.PLACEMENT_INVISIBLE && this.ModelState != ModelState.PLACEMENT_VISIBLE);
    }

    private void SetInitialState()
    {
        log.Debug("Setting model state to {}.", ModelState.PLACEMENT_INVISIBLE);
        this.ModelState = ModelState.PLACEMENT_INVISIBLE;

        this.filterDialogIndicator.gameObject.SetActive(false);
        this.sourceCodeDialogIndicator.gameObject.SetActive(false);
        this.infoDialogIndicator.gameObject.SetActive(false);
        this.legendDialogIndicator.gameObject.SetActive(false);

        this.colorChangeOnHover.enabled = false;
        this.boundingBoxRig.enabled = false;
        this.handDraggable.enabled = false;
        this.sourceCodeDialogOnClick.enabled = false;
        this.entityNameOnHoverIndicator.gameObject.SetActive(false);
        this.modelColliderDeactivator.enabled = true;
        this.modelRenderingStateController.SwitchState(ModelRenderingState.INVISIBLE);
    }
}
