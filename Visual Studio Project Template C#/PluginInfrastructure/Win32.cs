// NPP plugin platform for .Net v0.93.87 by Kasper B. Graversen etc.
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Kbg.NppPluginNET.PluginInfrastructure
{
    public class Win32
    {
        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, out IntPtr lParam);

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, NppMenuCmd lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), new IntPtr((uint)lParam));
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), new IntPtr(lParam));
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, out int lParam)
        {
            IntPtr outVal;
            IntPtr retval = SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), out outVal);
            lParam = outVal.ToInt32();
            return retval;
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, int lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, wParam, new IntPtr(lParam));
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, IntPtr wParam, int lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, wParam, new IntPtr(lParam));
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, IntPtr lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, string lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, int lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), new IntPtr(lParam));
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
        /// If gateways are missing or incomplete, please help extend them and send your code to the project 
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, IntPtr wParam, IntPtr lParam)
        {
            return SendMessage(hWnd, (UInt32)Msg, wParam, lParam);
        }

        /// <summary>
        /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as
        /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.
        /// If gateways are missing or incomplete, please help extend them and send your code to the project
        /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
        /// </summary>
	public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, ref LangType lParam)
        {
            IntPtr outVal;
            IntPtr retval = SendMessage(hWnd, (UInt32)Msg, new IntPtr(wParam), out outVal);
            lParam = (LangType)outVal;
            return retval;
        }

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
