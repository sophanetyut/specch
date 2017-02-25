using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace specch
{
    public partial class AddCommand : Form
    {
        Knock_Algorithm KA = new Knock_Algorithm();
        public AddCommand()
        {
            InitializeComponent();
        }


        string[] ConversationCMD = File.ReadAllLines(@"Database\ConversationByUser\ConversationCMD.txt");
        string[] ConversationRESP = File.ReadAllLines(@"Database\ConversationByUser\ConversationRESP.txt");
        //string[] shellCMD = Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
        //string[] shellCMD = File.ReadAllLines(@"Database\ShellCommand\ShellCMD.txt");
        //string[] shellWork = File.ReadAllLines(@"Database\ShellCommand\ShellWork.txt");
        string[] webCommand = File.ReadAllLines(@"Database\WebCommand\WebCommand.txt");
        string[] web_link = File.ReadAllLines(@"Database\WebCommand\website_Link.txt");
        //string[] locationkey = File.ReadAllLines(@"Database\BrowseLocation\LocationKey.txt");
        //string[] locationFile = File.ReadAllLines(@"Database\BrowseLocation\LocationFile.txt");



        private void AddCommand_Load(object sender, EventArgs e)//load from string array to listbox
        {

            for (int i = 0; i < ConversationCMD.Length; i++)
            {
                listBox1.Items.Add(ConversationCMD[i]);
                listBox2.Items.Add(ConversationRESP[i]);
            }

            for (int i = 0; i < webCommand.Length; i++)
            {
                listBox6.Items.Add(webCommand[i]);
                listBox5.Items.Add(web_link[i]);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                TextWriter ConversationCMD = new StreamWriter(@"Database\ConversationByUser\ConversationCMD.txt", true);
                TextWriter ConversationRES = new StreamWriter(@"Database\ConversationByUser\ConversationRESP.txt", true);
                listBox1.Items.Add(textBox1.Text);
                listBox2.Items.Add(textBox2.Text);
                ConversationCMD.WriteLine(textBox1.Text);
                ConversationRES.WriteLine(textBox2.Text);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();

                ConversationCMD.Close();
                ConversationRES.Close();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConversationCMD = File.ReadAllLines(@"Database\ConversationByUser\ConversationCMD.txt");
            ConversationRESP = File.ReadAllLines(@"Database\ConversationByUser\ConversationRESP.txt");

            if (listBox1.SelectedIndex > -1)
            {
                int index = listBox1.SelectedIndex;
                listBox2.Items.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                ConversationCMD = KA.RemoveStringArray(ConversationCMD, index);
                ConversationRESP = KA.RemoveStringArray(ConversationRESP, index);
                TextWriter ConversatioCMD = new StreamWriter(@"Database\ConversationByUser\ConversationCMD.txt");
                TextWriter ConversatioRES = new StreamWriter(@"Database\ConversationByUser\ConversationRESP.txt");
                for (int i = 0; i < ConversationCMD.Length; i++)
                {
                    ConversatioCMD.WriteLine(ConversationCMD[i]);
                    ConversatioRES.WriteLine(ConversationRESP[i]);
                }
                ConversatioCMD.Close();
                ConversatioRES.Close();
                ConversationCMD = null;
                ConversationRESP = null;
            }
        }




        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox5.Text != "")
            {
                TextWriter webCMD = new StreamWriter(@"Database\WebCommand\WebCommand.txt", true);
                TextWriter webLink = new StreamWriter(@"Database\WebCommand\website_Link.txt", true);
                listBox6.Items.Add(textBox6.Text);
                listBox5.Items.Add(textBox5.Text);
                webCMD.WriteLine(textBox6.Text);
                webLink.WriteLine(textBox5.Text);
                textBox6.Clear();
                textBox5.Clear();
                textBox6.Focus();
                webCMD.Close();
                webLink.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webCommand = File.ReadAllLines(@"Database\WebCommand\WebCommand.txt");
            web_link = File.ReadAllLines(@"Database\WebCommand\website_Link.txt");

            if (listBox6.SelectedIndex > -1)
            {
                int index = listBox6.SelectedIndex;
                listBox6.Items.RemoveAt(index);
                listBox5.Items.RemoveAt(index);
                webCommand = KA.RemoveStringArray(webCommand, index);
                web_link = KA.RemoveStringArray(web_link, index);
                TextWriter _webcommand = new StreamWriter(@"Database\WebCommand\WebCommand.txt");
                TextWriter _website = new StreamWriter(@"Database\WebCommand\website_Link.txt");
                for (int i = 0; i < webCommand.Length; i++)
                {
                    _webcommand.WriteLine(webCommand[i]);
                    _website.WriteLine(web_link[i]);
                }
                _webcommand.Close();
                _website.Close();
            }
        }

        private void AddCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.FormState = true;
            
        }
    }
}
