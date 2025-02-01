using HeaderInjectorViaProxy;
using HeaderInjectorViaProxy.Core;

AppSettings.RunWithWriteLog(() => MainAction(args), AfterError);

static void MainAction(string[] args)
{
    ConsoleApp.AllocConsole();
    ProxyConsole.PrintAppInfo();

    Console.WriteLine("\n\n");

    ProxyConsole.Run(args);
}

static void AfterError()
{
    Console.WriteLine("Something went wrong!");
    Console.Read();
}