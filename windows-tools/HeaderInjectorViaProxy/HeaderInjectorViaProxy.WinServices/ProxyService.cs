using HeaderInjectorViaProxy.Core;
using System.ServiceProcess;

namespace HeaderInjectorViaProxy;

public class ProxyService : ServiceBase
{
    private Proxy? _proxy;

    public ProxyService() : base()
    {
        AppSettings.RunWithWriteLog(() =>
        {
            AppSettings.LoadConfiguration();
            _proxy = new Proxy();

            ServiceName = AppSettings.WinServiceName;
        });
    }

    protected override void OnStart(string[] args)
    {
        AppSettings.RunWithWriteLog(() => { _proxy?.Start(); });
        
    }

    protected override void OnStop()
    {
        AppSettings.RunWithWriteLog(() => { _proxy?.Stop(); });
    }
}