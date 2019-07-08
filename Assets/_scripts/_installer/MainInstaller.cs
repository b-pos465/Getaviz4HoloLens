using Gaze;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.UX;
using Import;
using Logging;
using SpatialMapping;
using Zenject;

public class MainInstaller : MonoInstaller
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public override void InstallBindings()
    {
        log.Debug("Installing Bindings ...");

        this.Container.Bind<FlatModelProvider>().AsSingle();
        this.Container.Bind<TreeModelProvider>().AsSingle();
        this.Container.Bind<ModelInstantiator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<MetaphorPlacer>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<GazeStabilizer>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<RayCaster>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<TapService>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<KeywordToCommandService>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<ButtonClickSoundService>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<AppBar>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<EntityNameOnHoverController>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<TutorialProgressBarController>().FromComponentInHierarchy().AsSingle();

        this.Container.Bind<AutoCompleteController>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<ModelStateController>().FromComponentInHierarchy().AsSingle();
    }
}
