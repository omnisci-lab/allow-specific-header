using System.Runtime.InteropServices;

namespace HeaderInjectorViaProxy.Core;

public static class ConsoleApp
{
    [DllImport("kernel32.dll")]
    static extern nint GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(nint hWnd, int nCmdShow);

    const int SW_HIDE = 0;

    public static void HiddenConsole()
    {
        nint handle = GetConsoleWindow();
        if (handle != nint.Zero)
        {
            ShowWindow(handle, SW_HIDE);
        }
    }
}
