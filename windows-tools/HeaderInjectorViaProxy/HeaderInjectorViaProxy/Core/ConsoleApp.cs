using System.Runtime.InteropServices;

namespace HeaderInjectorViaProxy.Core;

public static class ConsoleApp
{
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();
}
