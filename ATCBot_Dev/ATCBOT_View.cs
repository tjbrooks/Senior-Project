using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Add these two statements to all SimConnect clients 
using LockheedMartin.Prepar3D.SimConnect;
using System.Runtime.InteropServices;

/*
 * ATCbot
 * oauth:kn5zrm6hxgumrhf97gk7r3uo9t2yzj
 * caderly_one
 * 
 * ATC_info
 * oauth:68ud963j2plwvncxho0pg1r8hre2rx
 * atc_info
 * 
 * 
*/

namespace ATCBot_Dev
{
    /// <summary>
    /// ATCBOT_View is the main user interface available for the user. Buttons are avaiable to connect/disconnect to Prepared3d and Twitch.
    /// A log view is used to display information relating to Prepared3d and Twitch. 
    /// </summary>
    public partial class ATCBOT_View : Form
    {
        private static ATCBOT_View instance = new ATCBOT_View();
        //private ATCBOT_View instance;

        /// <summary>
        /// ATCBOT_View iniatializes the window and sets the whether select buttons are enabled.
        /// </summary>
        public ATCBOT_View()
        {
            InitializeComponent();
            setButtons(true, false, false);
            setTwitchButton(true, false);
            //displayText("Starting up ATCBOT!");
            instance = this;

        }

        /// <summary>
        /// Instance is used as a refrence for the current ATCBOT_View object.
        /// </summary>
        public static ATCBOT_View Instance
        {
            get
            {
                return instance;
            }
        }


        // User-defined win32 event 
        const int WM_USER_SIMCONNECT = 0x0402;

        static string output = "\n\n\n\n\n\n\n\n\n\n";

        // Response number 
        static int response = 1;
        /// <summary>
        /// Handle recieved simconnect message.
        /// </summary>
        /// <param name="m"></param>
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                if (ATCBOT_Controller.Instance.simconnect != null)
                {
                    ATCBOT_Controller.Instance.simconnect.ReceiveMessage();
                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }
        /// <summary>
        /// setButtons is a method used to set whether buttons pertaining to the simulator are enabled/disabled.
        /// </summary>
        /// <param name="bConnect"></param>
        /// <param name="bGet"></param>
        /// <param name="bDisconnect"></param>
        private void setButtons(bool bConnect, bool bGet, bool bDisconnect)
        {
            buttonConnect.Enabled = bConnect;
            buttonRequestData.Enabled = bGet;
            buttonDisconnect.Enabled = bDisconnect;

        }
        /// <summary>
        /// setTwitchButton is a method used to set whether buttons pertaining to the Twitch are enabled/disabled.
        /// </summary>
        /// <param name="tConnect"></param>
        /// <param name="tDisconnect"></param>
        private void setTwitchButton(bool tConnect, bool tDisconnect)
        {
            conTwitchBtn.Enabled = tConnect;
            disTwitchBtn.Enabled = tDisconnect;
        }

        // Delegate for displaying text cross thread
        delegate void StringDelegate(string text);
        /// <summary>
        /// displayText is used to send a new message to log view. New messages will appear on a new line with a numbered line indicator.
        /// </summary>
        /// <param name="s"></param>
        public void displayText(string s)
        {
            // if call is coming from thread other than that of richResponse
            // the text is sent to a delegated function and which will call displayText on the same thread

            if (this.richResponse.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(displayText);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                // remove first string from output 
                output = output.Substring(output.IndexOf("\n") + 1);

                // add the new string 
                output += "\n" + response++ + ": " + s;

                // display it 
                richResponse.Text = output;
            }

            //output = output.Substring(output.IndexOf("\n") + 1);
            //output += "\n" + response++ + ": " + s;

        }
        /// <summary>
        /// buttonConnect_Click handles the even that the simultor connect button is click <see cref="ATCBOT_Controller.openConnection"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            ATCBOT_Controller.Instance.openConnection();
            //ATCBOT_Controller1.openConnection();
            setButtons(false, true, true);
            /*
               if (ATCBOT_Controller1.simconnect == null)
               {
                   try
                   {
                       // the constructor is similar to SimConnect_Open in the native API 
                       ATCBOT_Controller1.simconnect = new SimConnect("Managed Data Request", this.Handle, WM_USER_SIMCONNECT, null, 0);

                       setButtons(false, true, true);

                       ATCBOT_Controller1.initDataRequests();


                   }
                   catch (COMException ex)
                   {
                       displayText("Unable to connect to Prepar3D:\n\n" + ex.Message);
                   }
               }
               else
               {
                   displayText("Error - try again");
               ATCBOT_Controller1.closeConnection();

                   setButtons(true, false, false);
               }
               */
        }
        /// <summary>
        /// buttonDisconnect_Click handles the event that the disconnect from simulator button is click <see cref="ATCBOT_Controller.closeConnection"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            ATCBOT_Controller.Instance.closeConnection();
            setButtons(true, false, false);
        }

        private void buttonRequestData_Click(object sender, EventArgs e)
        {
            ATCBOT_Controller.Instance.Get_alt();
        }


        /// <summary>
        /// conTwitchBtn_Click handles the event that the twitch connect button is clicked <see cref="Twitch_Connect"/>.
        /// The process of listening to twitch is initiated based on response <see cref="backgroundWorker1_DoWork"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void conTwitchBtn_Click(object sender, EventArgs e)
        {
            Twitch_Connect t = new Twitch_Connect();
            t.ShowDialog();
            if (t.DialogResult == DialogResult.OK)
            {
                ATCBOT_View.Instance.displayText("Connecting to Twitch channel: " + Twitch_Controller.Instance.getChannel());
                setTwitchButton(false, true);
                ATCBOT_View.Instance.backgroundWorker1.RunWorkerAsync();
            }
        }

        delegate void ButtonDelegate(object sender, EventArgs e);
        /// <summary>
        /// disTwitchBtn_Click handles the event that the disconnect from Twitch button is clicked <see cref="Twitch_Controller.leaveChannel"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disTwitchBtn_Click(object sender, EventArgs e)
        {
            if (this.disTwitchBtn.InvokeRequired)
            {
                ButtonDelegate d = new ButtonDelegate(disTwitchBtn_Click);
                this.Invoke(d, new object[] { sender, e });
            }
            Twitch_Controller.Instance.leaveChannel();
            ATCBOT_View.Instance.backgroundWorker1.CancelAsync();
            setTwitchButton(true, false);
        }
        /// <summary>
        /// backgroundWorker1_DoWork is used to open a thread seperate from the UI for asynchronus work.
        /// Waiting for strings from <see cref="Twitch_Listener"/> to be displayed, or stopping when encountering read errors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            while (!backgroundWorker1.CancellationPending)
            {
                try
                {
                    string arg = Twitch_Listener.Instance.listen(backgroundWorker1);
                    ATCBOT_View.Instance.displayText(arg);
                }
                catch (Exception e2)
                {
                    string message = e2.Message;
                    disTwitchBtn_Click(sender, e);
                }
            }
        }
    }
}
