using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using FlowServer.Server.Core;

namespace FlowServer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WriteConsoleMessage("Main", "This is an apple", PrintType.ERROR);
            WriteConsoleMessage("Main", "This is an banana", PrintType.DEBUGING);
            WriteConsoleMessage("Main", "This is an candy", PrintType.WARNING);
        }

        private void MenuItem_File_Quit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WriteConsoleMessage(string agent, string text, PrintType type)
        {
            BeginInvoke((MethodInvoker) delegate {

                int initPoint = textConsole.Text.Length;
                Font baseFont = textConsole.Font;

                textConsole.AppendText(agent);
                textConsole.Select(initPoint, textConsole.Text.Length);
                textConsole.SelectionFont = new Font(baseFont.FontFamily, 12, FontStyle.Bold);

                initPoint = textConsole.Text.Length;
                textConsole.AppendText("\r\n\r\n");
                textConsole.Select(initPoint, textConsole.Text.Length);
                textConsole.SelectionFont = baseFont;

                initPoint = textConsole.Text.Length;
                textConsole.AppendText(text);
                textConsole.Select(initPoint, textConsole.Text.Length);

                switch (type)
                {
                    case PrintType.NORMAL:
                        break;
                    case PrintType.DEBUGING:
                        textConsole.SelectionColor = Color.Blue;
                        break;
                    case PrintType.WARNING:
                        textConsole.SelectionColor = Color.OrangeRed;
                        break;
                    case PrintType.ERROR:
                        textConsole.SelectionColor = Color.Red;
                        break;
                    default:
                        break;
                }
                                
                textConsole.AppendText("\r\n\r\n\r\n");
            });
        }

        private void HandleConsoleMessage()
        {

        }
    }
}
