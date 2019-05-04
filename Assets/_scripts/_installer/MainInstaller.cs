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

        this.Container.Bind<ImportController>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<GazeStabilizer>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<RayCaster>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<TapService>().FromComponentInHierarchy().AsSingle();

        this.Container.Bind<ModelIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<FilterDialogIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<CloseButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<SpatialMappingRootIndicator>().FromComponentInHierarchy().AsSingle();
    }
}