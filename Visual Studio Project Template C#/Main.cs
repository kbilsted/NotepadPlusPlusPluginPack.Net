using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "$safeprojectname$";
        static string iniFilePath = null;
        static bool someSetting = false;
        static frmMyDlg frmMyDlg = null;
        static int idMyDlg = -1;

        // toolbar icons
        static Bitmap tbBmp_color = Properties.Resources.star;     // standard icon small color
        static Icon tbIco_black = Properties.Resources.star_black; // Fluent UI icon black
        static Icon tbIco_white = Properties.Resources.star_white; // Fluent UI icon white
        static Icon tbIcon = null;

        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);

            PluginBase.SetCommand(0, "MyMenuCommand", myMenuFunction, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "MyDockableDialog", myDockableDialog); idMyDlg = 1;
        }

        internal static void SetToolBarIcon()
        {
            // create struct
            toolbarIcons tbIcons = new toolbarIcons();

            // add bmp icon
            tbIcons.hToolbarBmp = tbBmp_color.GetHbitmap();
            tbIcons.hToolbarIcon = tbIco_black.Handle;            // icon with black lines
            tbIcons.hToolbarIconDarkMode = tbIco_white.Handle;  // icon with light grey lines

            // convert to c++ pointer
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);

            // call Notepad++ api
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON_FORDARKMODE, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);

            // release pointer
            Marshal.FreeHGlobal(pTbIcons);
        }
		
        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }


        internal static void myMenuFunction()
        {
            MessageBox.Show("Hello N++!");
        }

        internal static void myDockableDialog()
        {
            if (frmMyDlg == null)
            {
                // show dialog for first time
                frmMyDlg = new frmMyDlg();

                // icon
                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_color, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                // dockable window struct data
                var _nppTbData = new NppTbData
                {
                    hClient = frmMyDlg.Handle,
                    pszName = "My dockable dialog",
                    dlgID = idMyDlg,
                    uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR,
                    hIconTab = (uint)tbIcon.Handle,
                    pszModuleName = PluginName
                };

                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            }
            else
            {
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMSHOW, 0, frmMyDlg.Handle);
            }
        }
    }
}