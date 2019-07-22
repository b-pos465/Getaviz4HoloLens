using Logging;
using Zenject;

public class IndicatorInstaller : MonoInstaller
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public override void InstallBindings()
    {
        log.Debug("Installing Bindings ...");

        this.Container.Bind<ModelIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<FilterDialogIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<SourceCodeDialogIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<AboutDialogIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<LegendDialogIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<SpatialMappingRootIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<CursorIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<EntityNameOnHoverIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<MetaphorPlacerIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<TutorialStateControllerIndicator>().FromComponentInHierarchy().AsSingle();
        this.Container.Bind<SourceCodeScrollViewIndicator>().FromComponentInHierarchy().AsSingle();

        this.Container.Bind<CloseButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<YesButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<NoButtonIndicator>().FromComponentInChildren();

        this.Container.Bind<IconIndicator>().FromComponentInChildren();
    }
}
