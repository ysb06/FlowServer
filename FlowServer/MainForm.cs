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
using FlowServer.Server;
using FlowServer.Server.Connection;

namespace FlowServer
{
    public partial class MainForm : Form
    {
        MainThreadController threadMain;

        private string currentConsoleMessageRequestingAgent = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ConsoleController.Print += WriteConsoleMessage;
            ConsoleController.UpdateConnection += UpdateConnectionList;
            threadMain = new MainThreadController();
          
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

                if(!currentConsoleMessageRequestingAgent.Equals(agent))
                {
                    textConsole.AppendText("\r\n");
                    textConsole.AppendText(agent);
                    textConsole.Select(initPoint, textConsole.Text.Length);
                    textConsole.SelectionFont = new Font(baseFont.FontFamily, 12, FontStyle.Bold);

                    initPoint = textConsole.Text.Length;
                    textConsole.AppendText("\r\n\r\n");
                    textConsole.Select(initPoint, textConsole.Text.Length);
                    textConsole.SelectionFont = baseFont;
                }
                                
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

                if (currentConsoleMessageRequestingAgent.Equals(agent))
                {
                    textConsole.AppendText("\r\n");
                }
                else
                {
                    textConsole.AppendText("\r\n\r\n");
                }

                currentConsoleMessageRequestingAgent = agent;
            });
        }

        private void ButtonInput_Click(object sender, EventArgs e)
        {
            ConsoleController.MessageBroadcast(textInput.Text);
            textInput.Text = "";
        }

        private void TextInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.Enter))
            {
                ConsoleController.MessageBroadcast(textInput.Text);
                textInput.Text = "";
            }
        }

        private void UpdateConnectionList(List<FlowClient> clients)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                connectionList.Items.Clear();

                foreach (FlowClient client in clients)
                {
                    connectionList.Items.Add(client);
                }
            });
        }
    }
}
