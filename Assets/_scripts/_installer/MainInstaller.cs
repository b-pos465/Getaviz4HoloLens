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

        this.Container.Bind<MeshRootIndicator>().FromComponentInHierarchy().AsSingle();
    }
}