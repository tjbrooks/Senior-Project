using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATCBot_Dev
{
    /// <summary>
    /// Twitch_Connect provides a window in which the user can set values need to connect to the Twitch interface.
    /// <seealso cref="Twitch_Controller"/>
    /// </summary>
    public partial class Twitch_Connect : Form
    {
        /// <summary>
        /// LoginInfo is used to store the information a user needs to connect to Twich.
        /// The class is serliazed and deserialized upon opening the window <see cref="Twitch_Connect"/>
        /// </summary>
        [Serializable] public class LoginInfo
        {
            /// <summary>
            /// name is the user name used for Twitch login.
            /// </summary>
            public string name;
            /// <summary>
            /// oAuth is a login crediential provided by Twitch to replace a password https://twitchapps.com/tmi/.
            /// </summary>
            public string oAuth;
            /// <summary>
            /// channel is the desired Twitch channel the user wishes to connect to.
            /// </summary>
            public string channel;
            /// <summary>
            /// Object reference for <see cref="LoginInfo"/>
            /// </summary>
            public static LoginInfo instance = new LoginInfo();
            /// <summary>
            /// Instance provides object of the class to be used.
            /// </summary>
            public static LoginInfo Instance 
            {
                get
                {
                    return instance;
                }
            }
            /// <summary>
            /// LoginInfo constructor is used to attempt to find an existing file on the client users machine,
            /// which will contain the last used values to connect to Twitch. In the event of an error the values are set to blank.
            /// </summary>
            LoginInfo()
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(LoginInfo));
                    //System.IO.StreamReader file = new System.IO.StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ATCBOT.xml");
                    LoginInfo i;
                    using (Stream reader = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ATCBOT.xml", FileMode.Open))
                    {
                        // Call the Deserialize method to restore the object's state.
                        i = (LoginInfo)serializer.Deserialize(reader);
                    }
                    //= (LoginInfo)reader.Deserialize(file);
                    this.name = i.name;
                    this.oAuth = i.oAuth;
                    this.channel = i.channel;
                    //file.Close();
                }catch(Exception e)
                {
                    Console.WriteLine(e.GetType().Name);
                    this.name = "";
                    this.oAuth = "";
                    this.channel = "";
                }
            }
            /// <summary>
            /// writeXML is used to serialize the login values used. This method is called after the connect button is pressed <see cref="connect_Click(object, EventArgs)"/>.
            /// The XML file is stored in %appdata% on the users machine with a file name of ATCBOT.xml.
            /// The file is readable with a simple file editor and intelligable for users to alter if desired.
            /// </summary>
            public void writeXML()
            {
               System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(LoginInfo));

                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//ATCBOT.xml";
                System.IO.FileStream file = System.IO.File.Create(path);

                writer.Serialize(file, LoginInfo.Instance);
                file.Close();
            }
        }
        /// <summary>
        /// Twtich_Connect intializes the window components. Also pulling and setting fields based on login values found by <see cref="LoginInfo"/>
        /// </summary>
        public Twitch_Connect()
        {
            InitializeComponent();
            this.oAuthBox.Text = LoginInfo.Instance.oAuth;
            this.userNameBox.Text = LoginInfo.Instance.name;
            this.channelBox.Text = LoginInfo.Instance.channel;
        }
        /// <summary>
        /// exit_Click is an event handler for a related button that closes the current window without attempting to establish 
        /// a connection or saving any login values in fields.
        /// </summary>
        /// <param name="sender">Default paramater created for UI. Not used in method.</param>
        /// <param name="e">Default paramater created for UI. Not used in method.</param>
        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// connect_Click handles the events for the related button. Current values are set and serialized by <see cref="LoginInfo.writeXML"/>
        /// The entered values are then sent to <see cref="Twitch_Connect"/> and used to establish a connection with Twitch.
        /// </summary>
        /// <param name="sender">Default paramater created for UI. Not used in method.</param>
        /// <param name="e">Default paramater created for UI. Not used in method.</param>
        private void connect_Click(object sender, EventArgs e)
        {
            LoginInfo.Instance.oAuth = this.oAuthBox.Text;
            LoginInfo.Instance.name = this.userNameBox.Text;
            LoginInfo.Instance.channel = this.channelBox.Text;
            Twitch_Controller.Instance.setClient(LoginInfo.Instance.name, LoginInfo.Instance.oAuth, LoginInfo.Instance.channel);
            if (Twitch_Controller.Instance.connected())
            {
                LoginInfo.Instance.writeXML();
                this.exit.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            }
            this.Close();
        }
    }
}
