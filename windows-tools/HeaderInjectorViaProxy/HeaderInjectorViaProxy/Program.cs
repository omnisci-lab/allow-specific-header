using HeaderInjectorViaProxy;
using HeaderInjectorViaProxy.Core;

AppSettings.RunWithWriteLog(
    () =>
    {
        ProxyConsole.PrintAppInfo();

        Console.WriteLine("\n\n");

        ProxyConsole.Run(args);
    },
    () =>
    {
        Console.WriteLine("Something went wrong!");
        Console.Read();
    }
);