using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlowServer.Server.FlowConsole;
using FlowServer.Server.Connection;

namespace FlowServer
{
    public partial class MainForm : Form
    {
        private MainSocketManager threadMain;

        private string currentConsoleMessageRequestingAgent = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ConsoleController.Print += WriteConsoleMessage;
            ConsoleController.UpdateConnection += UpdateConnectionList;
            ConsoleController.Broadcast += ConsoleController_Broadcast;
            threadMain = new MainSocketManager();
          
        }

        private void ConsoleController_Broadcast(string message, string[] convertedArgs)
        {
            ConsoleController.Log("> " + message);
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
                        textConsole.SelectionColor = Color.Orange;
                        break;
                    case PrintType.ERROR:
                        textConsole.SelectionColor = Color.Red;
                        break;
                    default:
                        break;
                }

                textConsole.AppendText("\r\n");

                currentConsoleMessageRequestingAgent = agent;
                textConsole.ScrollToCaret();
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

        private void UpdateConnectionList(List<IFlowClient> clients)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                listBoxConnection.Items.Clear();

                foreach (FlowClient client in clients)
                {
                    listBoxConnection.Items.Add(client);
                }

                if(listBoxConnection.Items.Count > 0)
                {
                    listBoxConnection.SelectedIndex = 0;
                }
            });
        }

        private void ButtonClientSend_Click(object sender, EventArgs e)
        {
            if(listBoxConnection.SelectedIndex < 0)
            {
                textSendClient.Text = "";
                return;
            }
            ConsoleController.MessageBroadcast("send " + listBoxConnection.Items[listBoxConnection.SelectedIndex] + " " + textSendClient.Text);
            textSendClient.Text = "";
        }

        private void TextSendClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                ButtonClientSend_Click(sender, e);
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (listBoxConnection.SelectedIndex < 0)
            {
                return;
            }

            ConsoleController.MessageBroadcast("disconnect " + listBoxConnection.Items[listBoxConnection.SelectedIndex]);
        }
    }
}
