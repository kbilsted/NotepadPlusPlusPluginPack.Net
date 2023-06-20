﻿// NPP plugin platform for .Net v0.91.57 by Kasper B. Graversen etc.
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Kbg.NppPluginNET.PluginInfrastructure;
using static Kbg.NppPluginNET.PluginInfrastructure.Win32;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Integration layer as the demo app uses the pluginfiles as soft-links files.
    /// This is different to normal plugins that would use the project template and get the files directly.
    /// </summary>
    class Main
    {
        static internal void CommandMenuInit()
        {
            Kbg.Demo.Namespace.Main.CommandMenuInit();
        }

        static internal void PluginCleanUp()
        {
            Kbg.Demo.Namespace.Main.PluginCleanUp();
        }

        static internal void SetToolBarIcon()
        {
            Kbg.Demo.Namespace.Main.SetToolBarIcon();
        }

        public static void OnNotification(ScNotification notification)
        {
            if (notification.Header.Code == (uint)SciMsg.SCN_CHARADDED)
            {
                Demo.Namespace.Main.doInsertHtmlCloseTag((char)notification.Character);
            }
            // dark mode (de-)activated
            if (notification.Header.Code == (uint)NppMsg.NPPN_DARKMODECHANGED)
            {
                INotepadPPGateway notepad = new NotepadPPGateway();
                Demo.Namespace.Main.ToggleDarkMode(Demo.Namespace.Main.frmGoToLine, notepad.IsDarkModeEnabled());
            }
        }

        internal static string PluginName { get { return Kbg.Demo.Namespace.Main.PluginName; }}
    }
}

namespace Kbg.Demo.Namespace
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "NppManagedPluginDemo";
        static string iniFilePath = null;
        static string sectionName = "Insert Extension";
        static string keyName = "doCloseTag";
        static bool doCloseTag = false;
        static string sessionFilePath = @"C:\text.session";
        static internal frmGoToLine frmGoToLine = null;
        static internal int idFrmGotToLine = -1;

        // toolbar icons
        static Bitmap tbBmp = Properties.Resources.star;
        static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        static Icon tbIco = Properties.Resources.star_black_ico;
        static Icon tbIcoDM = Properties.Resources.star_white_ico;
        static Icon tbIcon = null;

        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static INotepadPPGateway notepad = new NotepadPPGateway();
        #endregion

        #region " Startup/CleanUp "

        static internal void CommandMenuInit()
        {
            // Initialization of your plugin commands
            // You should fill your plugins commands here
 
            //
            // Firstly we get the parameters from your plugin config file (if any)
            //

            // get path of plugin configuration
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();

            // if config path doesn't exist, we create it
            if (!Directory.Exists(iniFilePath))
            {
                Directory.CreateDirectory(iniFilePath);
            }

            // make your plugin config file full file path name
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");

            // get the parameter value from plugin config
            doCloseTag = (Win32.GetPrivateProfileInt(sectionName, keyName, 0, iniFilePath) != 0);

            // with function :
            // SetCommand(int index,                            // zero based number to indicate the order of command
            //            string commandName,                   // the command name that you want to see in plugin menu
            //            NppFuncItemDelegate functionPointer,  // the symbol of function (function pointer) associated with this command. The body should be defined below. See Step 4.
            //            ShortcutKey *shortcut,                // optional. Define a shortcut to trigger this command
            //            bool check0nInit                      // optional. Make this menu item be checked visually
            //            );
            PluginBase.SetCommand(0, "Hello Notepad++", hello);
            PluginBase.SetCommand(1, "Hello (with FX)", helloFX);
            PluginBase.SetCommand(2, "What is Notepad++?", WhatIsNpp);

            // Here you insert a separator
            PluginBase.SetCommand(3, "---", null);

            // Shortcut :
            // Following makes the command bind to the shortcut Alt-F
            PluginBase.SetCommand(4, "Current Full Path", insertCurrentFullPath, new ShortcutKey(false, true, false, Keys.F));
            PluginBase.SetCommand(5, "Current File Name", insertCurrentFileName);
            PluginBase.SetCommand(6, "Current Directory", insertCurrentDirectory);
            PluginBase.SetCommand(7, "Date && Time - short format", insertShortDateTime);
            PluginBase.SetCommand(8, "Date && Time - long format", insertLongDateTime);

            PluginBase.SetCommand(9, "Close HTML/XML tag automatically", checkInsertHtmlCloseTag, new ShortcutKey(false, true, false, Keys.Q), doCloseTag);

            PluginBase.SetCommand(10, "---", null);

            PluginBase.SetCommand(11, "Get File Names Demo", getFileNamesDemo);
            PluginBase.SetCommand(12, "Get Session File Names Demo", getSessionFileNamesDemo);
            PluginBase.SetCommand(13, "Save Current Session Demo", saveCurrentSessionDemo);

            PluginBase.SetCommand(14, "---", null);

            PluginBase.SetCommand(15, "Dockable Dialog Demo", DockableDlgDemo); idFrmGotToLine = 15;

            PluginBase.SetCommand(16, "---", null);

            PluginBase.SetCommand(17, "Print Scroll and Row Information", PrintScrollInformation);
            
            PluginBase.SetCommand(18, "Use NanInf class for -inf, inf, nan!!", PrintNanInf);
        }

        /// <summary>
        /// 
        /// </summary>
        static void PrintScrollInformation()
        {
            ScrollInfo scrollInfo = editor.GetScrollInfo(ScrollInfoMask.SIF_RANGE | ScrollInfoMask.SIF_TRACKPOS | ScrollInfoMask.SIF_PAGE, ScrollInfoBar.SB_VERT);
            var scrollRatio = (double)scrollInfo.nTrackPos / (scrollInfo.nMax - scrollInfo.nPage);
            var scrollPercentage = Math.Min(scrollRatio, 1) * 100;
            editor.ReplaceSel($@"The maximum row in the current document was {scrollInfo.nMax+1}.
A maximum of {scrollInfo.nPage} rows is visible at a time.
The current scroll ratio is {Math.Round(scrollPercentage, 2)}%.
");
        }

        static internal void SetToolBarIcon()
        {
            // create struct
            toolbarIcons tbIcons = new toolbarIcons();
			
            // add bmp icon
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            tbIcons.hToolbarIcon = tbIco.Handle;            // icon with black lines
            tbIcons.hToolbarIconDarkMode = tbIcoDM.Handle;  // icon with light grey lines

            // convert to c++ pointer
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);

            // call Notepad++ api
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON_FORDARKMODE, PluginBase._funcItems.Items[idFrmGotToLine]._cmdID, pTbIcons);

            // release pointer
            Marshal.FreeHGlobal(pTbIcons);
        }

        static internal void PluginCleanUp()
        {
            Win32.WritePrivateProfileString(sectionName, keyName, doCloseTag ? "1" : "0", iniFilePath);
        }

        /// <summary>
        /// Apply dark mode (or re-apply light mode) to the controls of any form.<br></br>
        /// This method currently supports colorizing the following types of controls:<br></br>
        /// - Buttons<br></br>
        /// - Labels<br></br>
        /// - LinkLabels<br></br>
        /// - ComboBoxes<br></br>
        /// - CheckBoxes<br></br>
        /// - ListBoxes<br></br>
        /// - TreeViews<br></br>
        /// Feel free to add more as needed.<br></br>
        /// TODO: Figure out best way to customize border colors of controls.
        /// https://stackoverflow.com/questions/1445472/how-to-change-the-form-border-color-c
        /// may be a lead.
        /// </summary>
        /// <param name="form">a Windows Form</param>
        /// <param name="isDark">is Notepad++ dark mode on?</param>
        static internal void ToggleDarkMode(Form form, bool isDark)
        {
            if (form == null)
                return;
            IntPtr themePtr = notepad.GetDarkModeColors();
            if (isDark && themePtr == IntPtr.Zero)
                return;
            var theme = (DarkModeColors)Marshal.PtrToStructure(themePtr, typeof(DarkModeColors));
            foreach (Form childForm in form.OwnedForms)
            {
                // allow possibility that some forms will have other child forms
                // JsonTools does this in a couple of places
                ToggleDarkMode(childForm, isDark);
            }
            if (isDark)
            {
                form.BackColor = NppDarkMode.BGRToColor(theme.Background);
                form.ForeColor = NppDarkMode.BGRToColor(theme.Text);
            }
            else
            {
                form.ResetForeColor();
                form.ResetBackColor();
            }
            foreach (Control ctrl in form.Controls)
            {
                if (isDark)
                {
                    // this doesn't actually make disabled controls have different colors
                    // windows forms don't make it easy for the user to choose the
                    // color of a disabled control. See https://stackoverflow.com/questions/136129/windows-forms-how-do-you-change-the-font-color-for-a-disabled-label
                    var textTheme = ctrl.Enabled ? theme.Text : theme.DisabledText;
                    if (ctrl is Button btn)
                    {
                        btn.BackColor = NppDarkMode.BGRToColor(theme.SofterBackground);
                        btn.ForeColor = NppDarkMode.BGRToColor(textTheme);
                    }
                    else if (ctrl is LinkLabel llbl)
                    {
                        llbl.BackColor = NppDarkMode.BGRToColor(theme.ErrorBackground);
                        llbl.ForeColor = NppDarkMode.BGRToColor(theme.DarkerText);
                        llbl.LinkColor = NppDarkMode.BGRToColor(theme.LinkText);
                        llbl.ActiveLinkColor = NppDarkMode.BGRToColor(theme.Text);
                        llbl.VisitedLinkColor = NppDarkMode.BGRToColor(theme.DarkerText);
                    }
                    // other common text-based controls
                    else if (ctrl is TextBox
                        || ctrl is Label
                        || ctrl is ListBox
                        || ctrl is ComboBox)
                    {
                        ctrl.BackColor = NppDarkMode.BGRToColor(theme.PureBackground);
                        ctrl.ForeColor = NppDarkMode.BGRToColor(textTheme);
                    }
                    else if (ctrl is TreeView tv)
                    {
                        tv.BackColor = NppDarkMode.BGRToColor(theme.HotBackground);
                        tv.ForeColor = NppDarkMode.BGRToColor(textTheme);
                    }
                    else
                    {
                        // other controls I haven't thought of yet
                        ctrl.BackColor = NppDarkMode.BGRToColor(theme.SofterBackground);
                        ctrl.ForeColor = NppDarkMode.BGRToColor(textTheme);
                    }
                }
                else // normal light mode
                {
                    ctrl.ResetForeColor();
                    ctrl.ResetBackColor();
                    if (ctrl is LinkLabel llbl)
                    {
                        llbl.LinkColor = Color.Blue;
                        llbl.ActiveLinkColor = Color.Red;
                        llbl.VisitedLinkColor = Color.Purple;
                    }
                }
            }
            Marshal.FreeHGlobal(themePtr);
        }
        #endregion

        #region " Menu functions "
        static void hello()
        {
            notepad.FileNew();
            editor.SetText("Hello, Notepad++...from.NET!");
            var rest = editor.GetLine(0);
            editor.SetText(rest+rest+rest);
        }

        static void helloFX()
        {
            hello();
            new Thread(callbackHelloFX).Start();
        }

        static void callbackHelloFX()
        {
            int currentZoomLevel = editor.GetZoom();
            int i = currentZoomLevel;
            for (int j = 0 ; j < 4 ; j++)
            {    
                for ( ; i >= -10; i--)
                {
                    editor.SetZoom(i);
                    Thread.Sleep(30);
                }
                Thread.Sleep(100);
                for ( ; i <= 20 ; i++)
                {
                    Thread.Sleep(30);
                    editor.SetZoom(i);
                }
                Thread.Sleep(100);
            }
            for ( ; i >= currentZoomLevel ; i--)
            {
                Thread.Sleep(30);
                editor.SetZoom(i);
            }
        }

        static void WhatIsNpp()
        {
            string text2display = "Notepad++ is a free (as in \"free speech\" and also as in \"free beer\") " +
                "source code editor and Notepad replacement that supports several languages.\n" +
                "Running in the MS Windows environment, its use is governed by GPL License.\n\n" +
                "Based on a powerful editing component Scintilla, Notepad++ is written in C++ and " +
                "uses pure Win32 API and STL which ensures a higher execution speed and smaller program size.\n" +
                "By optimizing as many routines as possible without losing user friendliness, Notepad++ is trying " +
                "to reduce the world carbon dioxide emissions. When using less CPU power, the PC can throttle down " +
                "and reduce power consumption, resulting in a greener environment.";
            new Thread(new ParameterizedThreadStart(callbackWhatIsNpp)).Start(text2display);
        }

        static void callbackWhatIsNpp(object data)
        {
            string text2display = (string)data;
            notepad.FileNew();

            Random srand = new Random(DateTime.Now.Millisecond);
            int rangeMin = 0;
            int rangeMax = 250;
            for (int i = 0; i < text2display.Length; i++)
            {
                Thread.Sleep(srand.Next(rangeMin, rangeMax) + 30);
                editor.AppendTextAndMoveCursor(text2display[i].ToString());
            }
        }

        static void insertCurrentFullPath()
        {
            insertCurrentPath(NppMsg.FULL_CURRENT_PATH);
        }
        static void insertCurrentFileName()
        {
            insertCurrentPath(NppMsg.FILE_NAME);
        }
        static void insertCurrentDirectory()
        {
            insertCurrentPath(NppMsg.CURRENT_DIRECTORY);
        }
        static void insertCurrentPath(NppMsg which)
        {
            NppMsg msg = NppMsg.NPPM_GETFULLCURRENTPATH;
            if (which == NppMsg.FILE_NAME)
                msg = NppMsg.NPPM_GETFILENAME;
            else if (which == NppMsg.CURRENT_DIRECTORY)
                msg = NppMsg.NPPM_GETCURRENTDIRECTORY;

            StringBuilder path = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) msg, 0, path);

            editor.ReplaceSel(path.ToString());
        }

        static void insertShortDateTime()
        {
            insertDateTime(false);
        }
        static void insertLongDateTime()
        {
            insertDateTime(true);
        }
        static void insertDateTime(bool longFormat)
        {
            string dateTime = string.Format("{0} {1}", DateTime.Now.ToShortTimeString(), longFormat ? DateTime.Now.ToLongDateString() : DateTime.Now.ToShortDateString());
            editor.ReplaceSel(dateTime);
        }

        static void checkInsertHtmlCloseTag()
        {
            PluginBase.CheckMenuItemToggle(9, ref doCloseTag); // 9 = menu item index
        }

        static Regex regex = new Regex(@"[\._\-:\w]", RegexOptions.Compiled);

        static internal void doInsertHtmlCloseTag(char newChar)
        {
            LangType docType = LangType.L_TEXT;
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETCURRENTLANGTYPE, 0, ref docType);
            bool isDocTypeHTML = (docType == LangType.L_HTML || docType == LangType.L_XML || docType == LangType.L_PHP);

            if (!doCloseTag || !isDocTypeHTML)
                return;

            if (newChar != '>')
                return;

            int bufCapacity = 512;
            var pos = editor.GetCurrentPos();
            int currentPos = pos;
            int beginPos = currentPos - (bufCapacity - 1);
            int startPos = (beginPos > 0) ? beginPos : 0;
            int size = currentPos - startPos;

            if (size < 3)
                return;

            using (TextRange tr = new TextRange(startPos, currentPos, bufCapacity))
            {
                editor.GetTextRange(tr);
                string buf = tr.lpstrText;

                if (buf[size - 2] == '/')
                    return;

                int pCur = size - 2;
                while ((pCur > 0) && (buf[pCur] != '<') && (buf[pCur] != '>'))
                    pCur--;

                if (buf[pCur] == '<')
                {
                    pCur++;

                    var insertString = new StringBuilder("</");

                    while (regex.IsMatch(buf[pCur].ToString()))
                    {
                        insertString.Append(buf[pCur]);
                        pCur++;
                    }
                    insertString.Append('>');

                    if (insertString.Length > 3)
                    {
                        editor.BeginUndoAction();
                        editor.ReplaceSel(insertString.ToString());
                        editor.SetSel(pos, pos);
                        editor.EndUndoAction();
                    }
                }
            }
        }

        static void getFileNamesDemo()
        {
            int nbFile = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETNBOPENFILES, 0, 0);
            MessageBox.Show(nbFile.ToString(), "Number of opened files:");

            using (ClikeStringArray cStrArray = new ClikeStringArray(nbFile, Win32.MAX_PATH))
            {
                if (Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETOPENFILENAMES, cStrArray.NativePointer, nbFile) != IntPtr.Zero)
                    foreach (string file in cStrArray.ManagedStringsUnicode) MessageBox.Show(file);
            }
        }
        static void getSessionFileNamesDemo()
        {
            int nbFile = (int)Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETNBSESSIONFILES, 0, sessionFilePath);

            if (nbFile < 1)
            {
                MessageBox.Show("Please modify \"sessionFilePath\" in \"Demo.cs\" in order to point to a valid session file", "Error");
                return;
            }
            MessageBox.Show(nbFile.ToString(), "Number of session files:");

            using (ClikeStringArray cStrArray = new ClikeStringArray(nbFile, Win32.MAX_PATH))
            {
                if (Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETSESSIONFILES, cStrArray.NativePointer, sessionFilePath) != IntPtr.Zero)
                    foreach (string file in cStrArray.ManagedStringsUnicode) MessageBox.Show(file);
            }
        }
        static void saveCurrentSessionDemo()
        {
            string sessionPath = Marshal.PtrToStringUni(Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SAVECURRENTSESSION, 0, sessionFilePath));
            if (!string.IsNullOrEmpty(sessionPath))
                MessageBox.Show(sessionPath, "Saved Session File :", MessageBoxButtons.OK);
        }

        static void DockableDlgDemo()
        {
            // Dockable Dialog Demo
            // 
            // This demonstration shows you how to do a dockable dialog.
            // You can create your own non dockable dialog - in this case you don't nedd this demonstration.
            if (frmGoToLine == null)
            {
                frmGoToLine = new frmGoToLine(editor);

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }
                
                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = frmGoToLine.Handle;
                _nppTbData.pszName = "Go To Line #";
                // the dlgDlg should be the index of funcItem where the current function pointer is in
                // this case is 15.. so the initial value of funcItem[15]._cmdID - not the updated internal one !
                _nppTbData.dlgID = idFrmGotToLine;
                // define the default docking behaviour
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                // Following message will toogle both menu item state and toolbar button
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[idFrmGotToLine]._cmdID, 1);
            }
            else
            {
                if (!frmGoToLine.Visible)
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMSHOW, 0, frmGoToLine.Handle);
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[idFrmGotToLine]._cmdID, 1);
                }
                else
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMHIDE, 0, frmGoToLine.Handle);
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[idFrmGotToLine]._cmdID, 0);
                }
            }
            ToggleDarkMode(frmGoToLine, notepad.IsDarkModeEnabled());
            frmGoToLine.textBox1.Focus();
        }

        static void PrintNanInf()
        {
            bool neginf_correct = double.IsNegativeInfinity(NanInf.neginf);
            bool inf_correct = double.IsPositiveInfinity(NanInf.inf);
            bool nan_correct = double.IsNaN(NanInf.nan);
            string naninf = $@"-infinity == NanInf.neginf: {neginf_correct}
infinity == NanInf.inf: {inf_correct}
NaN == NanInf.nan: {nan_correct}
If you want these constants in your plugin, you can find them in the NanInf class in PluginInfrastructure.
DO NOT USE double.PositiveInfinity, double.NegativeInfinity, or double.NaN.
You will get a compiler error if you do.";
            notepad.FileNew();
            editor.AppendTextAndMoveCursor(naninf);
        }
        #endregion
    }
}   
