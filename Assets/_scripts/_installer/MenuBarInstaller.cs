using Logging;
using Zenject;

public class MenuBarInstaller : MonoInstaller
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public override void InstallBindings()
    {
        log.Debug("Installing Bindings ...");

        this.Container.Bind<MenuBarController>().FromComponentInHierarchy().AsSingle();

        this.Container.Bind<MenuBarDoneButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<MenuBarFilterButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<MenuBarAboutButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<MenuBarTransformButtonIndicator>().FromComponentInChildren();
        this.Container.Bind<MenuBarLegendButtonIndicator>().FromComponentInChildren();

        this.Container.Bind<MenuBarBackPlateIndicator>().FromComponentInChildren();
        this.Container.Bind<MenuBarButtonContainerIndicator>().FromComponentInChildren();
    }
}
