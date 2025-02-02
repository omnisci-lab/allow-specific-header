using System.Net;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace HeaderInjectorViaProxy.Core;

public class Proxy
{
    private ProxyServer _proxyServer;
    private ExplicitProxyEndPoint _explicitEndPoint;

    public Proxy()
    {
        _proxyServer = new ProxyServer();
        _explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Parse(AppSettings.ProxyAddress), AppSettings.ProxyPort, true);
    }

    public void ChangePort(int port)
    {
        _explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Parse(AppSettings.ProxyAddress), port, true);
    }

    public void Start()
    {
        _proxyServer.AddEndPoint(_explicitEndPoint);
        _proxyServer.BeforeRequest += OnRequest;
        _proxyServer.Start();
    }

    public void Stop()
    {
        _proxyServer.BeforeRequest -= OnRequest;
        _proxyServer.Stop();
    }

    private Task OnRequest(object sender, SessionEventArgs e)
    {
        if (AppSettings.Hosts!.Any(x => e.HttpClient.Request.Host != null && e.HttpClient.Request.Host.Contains(x)))
        {
            foreach (Header header in AppSettings.Headers!)
                e.HttpClient.Request.Headers.AddHeader(header.Name, header.Value);
        }

        return Task.CompletedTask;
    }
}
