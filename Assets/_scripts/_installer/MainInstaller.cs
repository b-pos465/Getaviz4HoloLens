using Gaze;
using HoloToolkit.Unity.InputModule;
using Import;
using Logging;
using Zenject;

public class MainInstaller : MonoInstaller
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public override void InstallBindings()
    {
        log.Debug("Installing Bindings ...");

        this.Container.Bind<ModelProvider>().AsSingle();
        this.Container.Bind<ModelInstantiator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<GazeStabilizer>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<RayCaster>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<TapService>().FromComponentInHierarchy().AsSingle();

        this.Container.Bind<CentralHoverController>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<SourceCodeDialogOnClick>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<AdjustModeController>().FromComponentSibling();
        this.Container.Bind<SourceCodeScrollViewController>().FromComponentInHierarchy().AsSingle();
    }
}