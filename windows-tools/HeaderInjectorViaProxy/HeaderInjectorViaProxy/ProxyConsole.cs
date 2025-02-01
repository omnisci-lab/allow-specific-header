using HeaderInjectorViaProxy.Core;

namespace HeaderInjectorViaProxy;

public static class ProxyConsole
{
    private static Proxy? _proxy = null;
    private static ConsoleArgument? _consoleArgument = null;

    private static bool IsValidArguments = false;

    public static void Run(string[] args)
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(CloseConsole);

        AppSettings.LoadConfiguration();
        _proxy = new Proxy();
        _consoleArgument = new ConsoleArgument(args);

        if (_consoleArgument!.NoArguments)
        {
            _proxy?.Start();
            Console.WriteLine($"Proxy is running on port {AppSettings.ProxyPort}");
            Console.WriteLine("Press Ctrl + C to exit");

            while (true) { Console.Read(); }
        }
        else if (_consoleArgument.Check(ConsoleArgument.Flag("--hidden")))
        {
            ConsoleApp.HiddenConsole();
            _proxy?.Start();

            while (true) { Thread.Sleep(20000); }
        }
        else if (_consoleArgument.Check(ConsoleArgument.Option("--port")))
        {
            if (ParsePort(out int portNumber))
            {
                _proxy?.ChangePort(portNumber);
                _proxy?.Start();

                Console.WriteLine($"Proxy is running on port {portNumber}");
                Console.WriteLine("Press Ctrl + C to exit");

                while (true) { Console.Read(); }
            }
            else
            {
                Console.WriteLine("Port is not valid");
                Console.Read();

                return;
            }
        }
        else if (_consoleArgument.Check(ConsoleArgument.Option("--port"), ConsoleArgument.Flag("--hidden")))
        {
            if (ParsePort(out int portNumber))
            {
                _proxy?.ChangePort(portNumber);
                ConsoleApp.HiddenConsole();
                _proxy?.Start();

                while (true) { Console.Read(); }
            }
            else
            {
                Console.WriteLine("Port is not valid");
                Console.Read();

                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid arguments");
            Console.WriteLine("Usage: HeaderInjectorViaProxy.exe [start] [--hidden]");
            Console.Read();
        }
    }

    static bool ParsePort(out int portNumber)
    {
        string? port = _consoleArgument?.GetOptionArg("--port");
        if (port is null)
        {
            portNumber = -1;
            return false;
        }

        return int.TryParse(port, out portNumber);
    }

    static void CloseConsole(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = false;

        if (IsValidArguments)
        {
            _proxy?.Stop();
            Console.WriteLine("Proxy stopped");
        }

        Console.WriteLine("Exiting... in 5 seconds");
        Thread.Sleep(5000);
        Environment.Exit(0);
    }

    public static void PrintAppInfo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("****************************************");
        Console.WriteLine("*                                      *");
        Console.WriteLine("*    Welcome to HeaderInjectorViaProxy *");
        Console.WriteLine("*             Version 1.0.0            *");
        Console.WriteLine("*                                      *");
        Console.WriteLine("****************************************");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("App Information:");
        Console.ResetColor();
        Console.WriteLine($"- Name: HeaderInjectorViaProxy");
        Console.WriteLine($"- Version: 1.0.0");
        Console.WriteLine($"- Developed By: phanxuanchanh (khothemegiatot)");
        Console.WriteLine($"- Description: A tool for adding custom headers to specific hosts.");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("****************************************");
        Console.WriteLine("*    Thank you for using HeaderInjectorViaProxy *");
        Console.WriteLine("*    For support, visit our website.    *");
        Console.WriteLine("****************************************");
        Console.ResetColor();
    }
}
