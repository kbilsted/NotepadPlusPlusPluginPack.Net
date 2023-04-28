using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kbg.Demo.Namespace
{
    public partial class DarkModeTestForm : Form
    {
        public DarkModeTestForm()
        {
            InitializeComponent();
            var notepad = new NotepadPPGateway();
            Main.ToggleDarkMode(this, notepad.IsDarkModeEnabled());
            comboBox1.SelectedIndex = 0;
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = !linkLabel1.LinkVisited;
        }
    }
}
