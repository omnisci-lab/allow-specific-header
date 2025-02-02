using HeaderInjectorViaProxy;
using System.ServiceProcess;

ServiceBase[] servicesToRun = new ServiceBase[]
{
    new ProxyService()
};

ServiceBase.Run(servicesToRun);
