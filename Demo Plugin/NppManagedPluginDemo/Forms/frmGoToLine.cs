using System;
using System.Windows.Forms;

namespace NppPluginNET
{
    partial class frmGoToLine : Form
    {
        public frmGoToLine()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int line;
            if (!int.TryParse(textBox1.Text, out line)) return;
            IntPtr curScintilla = PluginBase.GetCurrentScintilla();
			Win32.SendMessage(curScintilla, SciMsg.SCI_ENSUREVISIBLE, line - 1, 0);
            Win32.SendMessage(curScintilla, SciMsg.SCI_GOTOLINE, line - 1, 0);
            Win32.SendMessage(curScintilla, SciMsg.SCI_GRABFOCUS, 0, 0);
        }

        private void frmGoToLine_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Return) || (e.Alt && (e.KeyCode == Keys.G)))
            {
                button1.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Escape)
            {
                Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GRABFOCUS, 0, 0);
            }
            else if (e.KeyCode == Keys.Tab)
            {
                Control next = GetNextControl((Control)sender, !e.Shift);
                while ((next == null) || (!next.TabStop)) next = GetNextControl(next, !e.Shift);
                next.Focus();
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)
                && (e.KeyChar != '\b')
                && (e.KeyChar != '\t')) 
                e.Handled = true;
        }
        
        void FrmGoToLineVisibleChanged(object sender, EventArgs e)
        {
        	if (!Visible)
        	{
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                                  PluginBase._funcItems.Items[PluginBase.idFrmGotToLine]._cmdID, 0);
        	}
        }
    }
}
