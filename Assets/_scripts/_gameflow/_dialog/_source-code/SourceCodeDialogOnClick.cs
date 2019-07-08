using Gaze;
using Import;
using Logging;
using Model;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Zenject;

public class SourceCodeDialogOnClick : MonoBehaviour
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    [Inject]
    private RayCaster rayCaster;

    [Inject]
    private ModelStateController modelStateController;

    [Inject]
    private AppBarIndicator appBarIndicator;

    [Inject]
    private SourceCodeDialogIndicator sourceCodeDialogIndicator;

    [Inject]
    private TapService tapService;

    [Inject]
    private ButtonClickSoundService buttonClickSoundService;

    [Inject]
    private SourceCodeSnippetInstantiator sourceCodeSnippetInstantiator;

    [Inject]
    private SourceCodeClassNameController sourceCodeClassNameController;

    [Inject]
    private SourceCodeReader sourceCodeReader;

    [Inject]
    private SourceCodeScrollViewIndicator sourceCodeScrollViewIndicator;

    private void Start()
    {
        this.tapService.Register(this.OnTap);
    }

    private void OnTap(TappedEventArgs tappedEventArgs)
    {
        if (!this.enabled)
        {
            return;
        }

        if (this.rayCaster.Hits)
        {
            Entity entity = this.rayCaster.Target.GetComponent<Entity>();
            if (entity != null && entity.type == "FAMIX.Class")
            {
                this.buttonClickSoundService.PlayButtonClickSound();
                this.modelStateController.SwitchState(ModelState.SOURCECODE);

                this.UpdateSourceCode(entity);
            }
        }
    }

    private void UpdateSourceCode(Entity entity)
    {
        log.Debug("Setting source code for entity: {}", entity.qualifiedName);

        string sourceCode = this.sourceCodeReader.ReadClass(entity.qualifiedName);
        this.sourceCodeSnippetInstantiator.InstantiateSourceCodeSnippets(sourceCode);

        int linesOfCodeThatFitInTheViewport = 28;
        int linesOfCodeInTotal = sourceCode.Split('\n').Length;
        this.sourceCodeScrollViewIndicator.GetComponent<ScrollViewController>().range = (float)linesOfCodeThatFitInTheViewport / (float)linesOfCodeInTotal;

        this.sourceCodeClassNameController.UpdateClassName(entity);
    }
}
