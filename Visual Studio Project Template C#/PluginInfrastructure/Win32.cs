// NPP plugin platform for .Net v0.92.76 by Kasper B. Graversen etc.
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Kbg.NppPluginNET.PluginInfrastructure
{
    public class Win32
    {
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, NppMenuCmd lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, IntPtr lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, out int lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, IntPtr wParam, int lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, ref LangType lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);


        // TODO KBG Experimental
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, IntPtr wParam, int lParam);


        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, IntPtr lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, string lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, int lParam);

        public const int MAX_PATH = 260;
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public const int MF_BYCOMMAND = 0;
        public const int MF_CHECKED = 8;
        public const int MF_UNCHECKED = 0;

        [DllImport("user32")]
        public static extern IntPtr GetMenu(IntPtr hWnd);
        [DllImport("user32")]
        public static extern int CheckMenuItem(IntPtr hmenu, int uIDCheckItem, int uCheck);

        public const int WM_CREATE = 1;

        [DllImport("user32")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("kernel32")]
        public static extern void OutputDebugString(string lpOutputString);
    }
}