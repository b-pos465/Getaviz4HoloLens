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
        this.Container.Bind<DragRecognizer>().FromComponentInHierarchy().AsSingle();

        this.Container.Bind<ModelIndicator>().FromComponentInHierarchy().AsSingle();
    }
}