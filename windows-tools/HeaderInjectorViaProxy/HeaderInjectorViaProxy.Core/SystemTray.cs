using System.Runtime.InteropServices;


namespace HeaderInjectorViaProxy.Core;

public class SystemTray
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct NOTIFYICONDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uID;
        public uint uFlags;
        public uint uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
        public uint dwState;
        public uint dwStateMask;
        public uint uVersion;
    }

    const uint NIF_MESSAGE = 0x01;
    const uint NIF_ICON = 0x02;
    const uint NIF_TIP = 0x04;
    const uint WM_USER = 0x0400;
    const uint WM_LBUTTONDOWN = 0x0201;
    const uint WM_RBUTTONDOWN = 0x0204;
    const uint NIM_ADD = 0x00;
    const uint NIM_MODIFY = 0x01;
    const uint NIM_DELETE = 0x02;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

    [DllImport("shell32.dll", SetLastError = true)]
    static extern bool Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA lpData);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr CreatePopupMenu();

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr GetMessageExtraInfo();

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    static bool consoleVisible = false;

    static IntPtr hwnd;
    static IntPtr hMenu;

    public static void Add()
    {
        //IntPtr hIcon = ExtractIcon(IntPtr.Zero, Process.GetCurrentProcess().MainModule.FileName, 0);
        IntPtr hIcon = ExtractIcon(IntPtr.Zero, "shell32.dll", 0);
        IntPtr hMenu = CreatePopupMenu();

        AppendMenu(hMenu, 0, 1, "Hiển thị");
        AppendMenu(hMenu, 0, 2, "Thoát");

        NOTIFYICONDATA nid = new NOTIFYICONDATA
        {
            cbSize = (uint)Marshal.SizeOf(typeof(NOTIFYICONDATA)),
            hWnd = IntPtr.Zero,
            uID = 1,
            uFlags = NIF_ICON | NIF_TIP | NIF_MESSAGE,
            uCallbackMessage = WM_USER + 1,
            hIcon = hIcon, 
            szTip = "HeaderInjectorViaProxy",
            dwState = 0,
            dwStateMask = 0,
            uVersion = 0
        };

        // Thêm icon vào System Tray
        Shell_NotifyIcon(NIM_ADD, ref nid);

        //while (true)
        //{
        //    // Lắng nghe sự kiện chuột phải
        //    IntPtr msg = GetMessageExtraInfo();
        //    if (msg == (IntPtr)WM_RBUTTONDOWN)
        //    {
        //        // Hiển thị menu chuột phải
        //        // Tính toán vị trí chuột
        //        int mouseX = 0; // Lấy vị trí chuột của bạn
        //        int mouseY = 0;

        //        // Hiển thị menu
        //        TrackPopupMenu(hMenu, 0, mouseX, mouseY, 0, hwnd, IntPtr.Zero);
        //    }
        //}
    }

    public static void Remove()
    {
        NOTIFYICONDATA nid = new NOTIFYICONDATA
        {
            cbSize = (uint)Marshal.SizeOf(typeof(NOTIFYICONDATA)),
            hWnd = IntPtr.Zero,
            uID = 1
        };

        Shell_NotifyIcon(NIM_DELETE, ref nid);
    }
}
