using Microsoft.Extensions.Configuration;

namespace HeaderInjectorViaProxy.Core;

public static class AppSettings
{
    public static string ProxyAddress { get; set; } = default!;
    public static int ProxyPort { get; set; }
    public static string WinServiceName { get; set; } = default!;


    public static List<string>? Hosts { get; set; }
    public static List<Header>? Headers { get; set; }

    public static void LoadConfiguration()
    {
        string appsettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory).AddJsonFile(appsettingsPath, optional: false, reloadOnChange: true)
            .Build();

        IConfigurationSection proxyConfigSection = config.GetSection("Proxy");
        if (proxyConfigSection.Exists())
        {
            ProxyAddress = proxyConfigSection.GetSection("Address").Value ?? throw new NullReferenceException("Address cannot be null");
            ProxyPort = int.Parse(proxyConfigSection.GetSection("Port").Value!);
            WinServiceName = proxyConfigSection.GetSection("WinServiceName").Value ?? throw new NullReferenceException("WinServiceName cannot be null");
        }

        IConfigurationSection hostsConfigSection = config.GetSection("Hosts");
        if (hostsConfigSection.Exists())
        {
            List<string>? hosts = hostsConfigSection?.Get<List<string>>();

            if (hosts is null || hosts?.Count == 0)
                throw new Exception("Hosts cannot be empty");

            Hosts = hosts;
        }

        IConfigurationSection headersConfigSection = config.GetSection("Headers");
        if (headersConfigSection.Exists())
        {
            List<Header>? headers = headersConfigSection?.Get<List<Header>>();

            if (headers is null || headers?.Count == 0)
                throw new Exception("Headers cannot be empty");

            Headers = headers;
        }
    }

    public static void RunWithWriteLog(Action action, Action? actionAfterError = null)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ServiceLog.txt");
            File.AppendAllText(logPath, $"{ex.Message}\n");

            if (actionAfterError is not null)
                actionAfterError();
        }
    }
}
