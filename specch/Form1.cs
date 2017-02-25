using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Speech.AudioFormat;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.Synthesis.TtsEngine;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Timers;

namespace specch
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer,Listener;
        SpeechSynthesizer KNOCK;
        Random rnd = new Random();
        OpenFileDialog open = new OpenFileDialog();
        public static bool FormState=true;
        
        string[] FileName=null;

        public Form1()
        {
            InitializeComponent();
            
            listBox1.Visible = false;
            listBox1.Sorted = true;
            axWindowsMediaPlayer1.Visible = false;
            pictureBox1.Visible = false;
            
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Listener = new SpeechRecognitionEngine();
            _recognizer = new SpeechRecognitionEngine();
            KNOCK = new SpeechSynthesizer();

            CMDLoad();//load command in to array by calling the Array method
            loadSpeech();//instanciate the object of speech engine and configure the requirement
            SystemEvents.PowerModeChanged +=new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }
        
        bool pState = true;
        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (SystemInformation.PowerStatus.PowerLineStatus)
            {
                case PowerLineStatus.Offline:
                    if (!pState)
                    {
                        call_Thread("charger unpluged");
                        pState = true;
                    }
                    break;
                case PowerLineStatus.Online:
                    if (pState)
                    {
                        call_Thread("Battery is charging");
                        pState = false;
                    }
                    break;
                case PowerLineStatus.Unknown:
                    call_Thread("I can't recognize this");
                    break;
                default:
                    break;
            }
#if cmt
            /*
            switch (SystemInformation.PowerStatus.BatteryChargeStatus)
            {
                case BatteryChargeStatus.High:
                    call_Thread("Sir, your computer battery is getting high.");
                    break;
                case BatteryChargeStatus.Low:
                    call_Thread("sir, your computer battery is getting low.");
                    break;
                case BatteryChargeStatus.Critical:
                    call_Thread("sir, your computer batter is on critical mode.");
                    break;
                case BatteryChargeStatus.Charging:
                    call_Thread("sir, your computer batter is charging.");
                    break;
                case BatteryChargeStatus.NoSystemBattery:
                    call_Thread("sir, your computer system have no battery.");
                    break;
                case BatteryChargeStatus.Unknown:
                    call_Thread("sir, you computer batter was unknown.");
                    break;
            }
            */
#endif
        }
        
        void loadSpeech()
        {
            _recognizer.SetInputToDefaultAudioDevice();
            //_recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(arr[6].Concat(arr[1].Concat(arr[2].Concat(arr[0]))).ToArray()))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            //KNOCK.SelectVoiceByHints(VoiceGender.Female);

            Listener.SetInputToDefaultAudioDevice();
            Listener.LoadGrammar(new DictationGrammar());
            Listener.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(new string[] {"come back"}))));
            Listener.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Listen_control);
            Listener.RecognizeAsyncStop();
            
        }

        private void Listen_control(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("come back"))
            {
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                Listener.RecognizeAsyncStop();
                call_Thread("sir, i'm back");
            }
        }

        string s;//for pass the result after genereated from voice
        void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)// an event that will be called after there are some voice over the microphone
        {
            s = e.Result.Text;
            getSpeech(s);
        }

        string respon;// for pass the value to speak out of the speaker after search true;  use static string because thread needed.
        void call_Thread(string speak)//Process speak out the text to string
        {
            respon = speak;
            Thread thread = new Thread(child_Thread);
            thread.Start();
        }
        
        private void child_Thread()// thread will call this function
        {
            pictureBox1.Visible = true;
            KNOCK.Speak(respon);
            pictureBox1.Visible = false;
        }
        
        void getSpeech(string s)//After we found the "string s" that got from _recognizer_SpeechRecognized then we will take decision for respon
        {
            Random ran = new Random();
            int ranNum = ran.Next(10);
            switch (s)
            {
                //Developer..................................................................
                case "who is developer":
                    call_Thread("yoth sopanith was giving me a life at third of january 2016");
                    break;
                //Greetings..................................................................
                
                case "hello":
                    if (ranNum < 6) {call_Thread("Hello sir, May i help you."); }
                    else if (ranNum > 5) { call_Thread("hi"); }
                    break;
                case "goodbye":
                case "goodbye knock":
                case "close knock":
                    call_Thread("Goodbye sir");
                    this.Close();
                    break;
                    
                //Condition of day...........................................................
                case "what time is it":
                    DateTime now = DateTime.Now;
                    call_Thread(now.GetDateTimeFormats('t')[0]);
                    break;
                case "what day is it today":
                    call_Thread("Today is" + DateTime.Today.ToString("dddd"));
                    break;
                case "tell me the date":
                case "what todays date":
                    call_Thread(DateTime.Today.ToString("dd mm yy"));
                    break;
                //Open Office.................................................................
                case "open microsoft word":
                    try
                    {
                        System.Diagnostics.Process.Start(@"winword.exe");
                        call_Thread("openning microsoft word");
                    }
                    catch (Exception)
                    {
                        call_Thread("sir, I can not open microsoft word");
                    }
                    break;
                case "open microsoft excel":
                    try
                    {
                        System.Diagnostics.Process.Start(@"excel.exe");
                        call_Thread("openning microsoft excel");
                    }
                    catch (Exception)
                    {
                        call_Thread("sir, I can not open microsoft excel");
                    }                  
                    break;
                case "open microsoft power point":
                    try
                    {
                        System.Diagnostics.Process.Start(@"powerpnt.exe");
                        call_Thread("Openning microsoft power point");
                    }
                    catch (Exception)
                    {
                        call_Thread("sir, I cannot open microsoft powerpnt");
                        throw;
                    }
                    break;
                


                //Other commands..............................................................

                case "go fullscreen":
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    if (axWindowsMediaPlayer1.Visible)
                    {
                        axWindowsMediaPlayer1.fullScreen = true;
                    }   
                    this.TopMost = true;
                    call_Thread("Expanding");
                    break;
                case "exit fullscreen":
                    FormBorderStyle = FormBorderStyle.Sizable;
                    axWindowsMediaPlayer1.fullScreen = false;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;
                    break;
                case "hide body":
                    this.Hide();
                    notifyIcon.BalloonTipText = "I'm minimized to tray";
                    notifyIcon.ShowBalloonTip(1000);
                    call_Thread("i'm hidding");
                    break;
                case "show body":
                    this.Show();
                    call_Thread("show");
                    break;
                case "stop listening":
                    _recognizer.RecognizeAsyncStop();
                    Listener.RecognizeAsync(RecognizeMode.Multiple);
                    call_Thread("I will be back when you call me");
                    break;
                case "show video":
                    axWindowsMediaPlayer1.Show();
                    //axWindowsMediaPlayer1.fullScreen = true;
                    axWindowsMediaPlayer1.stretchToFit = true;
                    //axWindowsMediaPlayer1.Dock = DockStyle.Fill;
                    //pictureBox1.SendToBack();
                    //axWindowsMediaPlayer1.BringToFront();

                    break;
                case "hide video":
                    axWindowsMediaPlayer1.fullScreen = false;
                    axWindowsMediaPlayer1.Hide();
                    break;

                    
                //KEYBOARD COMMANDS............................................................
                case "switch window":
                case "switch":
                    call_Thread("switch");
                    SendKeys.SendWait("%{TAB}");
                    //SendKeys.Send("%{TAB}");
                    break;
                case "save":
                    SendKeys.Send("^s");
                    call_Thread("save");
                    break;
                case "tab":
                    SendKeys.Send("{TAB}");
                    break;
                case "close":
                case "no":
                    SendKeys.Send("%{F4}");
                    call_Thread("yes sir");
                    break;
                case "enter":
                case "ok":
                    SendKeys.Send("{ENTER}");
                    break;
                case "close tab":
                    SendKeys.Send("^{w}");
                    break;
                case "​​​​​​move up":
                    SendKeys.Send("{UP}");
                    call_Thread("left");
                    break;
                case "move up":
                    SendKeys.Send("{DOWN}");
                    call_Thread("down");
                    break;
                case "move left":
                    SendKeys.Send("{LEFT}");
                    call_Thread("left");
                    break;
                case "next slide":
                    SendKeys.Send("{RIGHT}");
                    call_Thread("right");
                    break;
                case "page down":
                    SendKeys.Send("{PGDN}");
                    break;
                case "page up":
                    SendKeys.Send("{PGUP}");
                    break;


                //MUSIC PLAY....................................................................
                case "play music":
                    playMusic();
                    break;
                case "next song":
                    axWindowsMediaPlayer1.Ctlcontrols.next();
                    break;
                case "previous song":
                    axWindowsMediaPlayer1.Ctlcontrols.previous();
                    break;
                case "stop music":
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    break;
                case "play":
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    break;
                case "pause":
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    break;
                case "load music":
                    //loadMusic();
                    break;


                //SHUTDOWN RESTART LOGOFF .......................................................
                case "shutdown":
                    System.Diagnostics.Process.Start("shutdown", "-s -t 100");
                    call_Thread("sir, i will shutdown your computer in next one minute.");
                    break;
                case "restart":
                    System.Diagnostics.Process.Start("shutdown", "-r -t 1000");
                    call_Thread("sir i will restart your computer in next one minute.");
                    break;
                case "sign out my computer":
                    System.Diagnostics.Process.Start("shutdown", "-l -f");
                    break;
                case "lock desktop":
                    call_Thread("yes sir. your computer is locked");
                    System.Diagnostics.Process.Start("rundll32.exe", "user32.dll, LockWorkStation");
                    
                    break;
                case "cancel shutdown":
                case "cancel restart":
                    System.Diagnostics.Process.Start("shutdown", " -a");
                    call_Thread("Canceled");
                    break;

                //User_add_command..........................................................
                case "add command":
                    if (FormState)
                    {
                        AddCommand UserAddcommand = new AddCommand();
                        UserAddcommand.Show();
                        FormState = false;
                        this.Hide();
                    }
                    
                    break;
                case "show command":
                    listBox1.Visible = true;
                    break;
                case "hide command":
                    listBox1.Visible = false;
                    break;
                case "reload key":
                    call_Thread("sir, command uploaded");
                    CMDLoad();
                    _recognizer.UnloadAllGrammars();
                    _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(arr[6].Concat(arr[1].Concat(arr[2].Concat(arr[0]))).ToArray()))));
                    break;

                //Exploter technology =============================================================>
                default:
                    Default();
                    break;
            }
        }
        
        void Default()// after getSpeech() was not found it will call this function to search on the array
        {
            int num = 0;
            bool state = true;
            for (int i = 0; i < 3; i++)
            {
                if (state == true)
                {
                    for (int j = 0; j < arr[i].Length; j++)
                    {
                        if (num == 0 && s == arr[i][j])
                        {
                            call_Thread(arr[3][j]);
                            state = false;
                            break;
                        }
                        else if (num == 1 && s == arr[i][j])
                        {
                            call_Thread("open");
                            System.Diagnostics.Process.Start(arr[4][j]);
                            state = false;
                            break;
                        }
                        else if (num == 2 && s == arr[i][j])
                        {
                            call_Thread("open");
                            System.Diagnostics.Process.Start(arr[5][j]);
                            state = false;
                            break;
                        }
                    }
                    num++;
                }
                else
                {
                    break;
                }
            }
        }

        string[][] arr = new string[7][]; //array string to store data from notpad after formLoad() 
        public void CMDLoad()//initialize the Array of arr[]
        {
            listBox1.Items.Clear();
            arr[0] = File.ReadAllLines(@"Database\ConversationByUser\ConversationCMD.txt");
            //arr[1] = File.ReadAllLines(@"Database\ShellCommand\ShellCMD.txt");
            arr[2] = File.ReadAllLines(@"Database\WebCommand\WebCommand.txt");
            //===========================================================================================================>
            arr[3] = File.ReadAllLines(@"Database\ConversationByUser\ConversationRESP.txt");
            //arr[4] = Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
            arr[4] = Directory.GetFiles(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"));
            //arr[4] = File.ReadAllLines(@"Database\ShellCommand\ShellWork.txt");
            arr[5] = File.ReadAllLines(@"Database\WebCommand\website_Link.txt");
            arr[6] = File.ReadAllLines(@"Database\Command.txt");
            arr[1] = new string[arr[4].Length];
            for (int i = 0; i < arr[4].Length; i++)
            {
                arr[1][i] = "open " + Path.GetFileNameWithoutExtension(arr[4][i]);
                listBox1.Items.Add("open " + Path.GetFileNameWithoutExtension(arr[4][i]));
            }
            foreach (string item in arr[0])
            {
                listBox1.Items.Add(item);
            }
            foreach (string item in arr[2])
            {
                listBox1.Items.Add(item);
            }
            foreach (string item in arr[6])
            {
                listBox1.Items.Add(item);
            }
        }

        
        private void playMusic()// load the music to play after user called "play music"
        {
            open.Multiselect = true;

            WMPLib.IWMPPlaylist playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("NU Playlist");
            WMPLib.IWMPMedia media;
            //   media=axWindowsMediaPlayer1.newMedia()
            //          axWindowsMediaPlayer1.currentPlaylist = playlist;
            if (open.ShowDialog() == DialogResult.OK)
            {
                FileName = open.FileNames;
                foreach (string file in FileName)
                {
                    media = axWindowsMediaPlayer1.newMedia(file);
                    playlist.appendItem(media);
                }
                axWindowsMediaPlayer1.currentPlaylist = playlist;
            }

        }
        
        private void listBox1_MouseClick(object sender, MouseEventArgs e)  //speak the command after the user click on it... 
        {
            call_Thread(listBox1.SelectedItem.ToString());
        }

        private void button1_Click(object sender, EventArgs e)// skip button of the userguide after foamLoad()
        {
            panel1.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
