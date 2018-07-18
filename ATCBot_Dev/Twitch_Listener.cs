using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ATCBot_Dev
{
    /// <summary>
    /// Used to poll the channel estblished by <see cref="Twitch_Controller"/> for a desired message.
    /// </summary>
    public class Twitch_Listener
    {
        private static Twitch_Listener instance = new Twitch_Listener();
        /// <summary>
        /// Instance retuns the object of <see cref="Twitch_Listener"/> to be used.
        /// </summary>
        public static Twitch_Listener Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// listen is used as a loop until <see cref="findBang"/> returns anything except null.
        /// </summary>
        /// <returns>Returns a string containing a user name and a registered command.<see cref="findBang"/></returns>
        public string listen(BackgroundWorker sender)
        {
            string[] arg = new string [2];
            do
            {
                arg = findBang();
            } while (arg == null &&  !sender.CancellationPending);
            return arg[0] + ": " + arg[1];
        }
        /// <summary>
        /// findBangs searches the result of <see cref="Twitch_Controller.readMessage"/> for a desired bang(message begining with !).
        /// A bang indicates a command supported by the system that will produce a response in the client interface and/or the Twitch channel.
        /// A String array with two elements is used to contain the user who sent the supported bang and the bang command.
        /// The supported command is then send to <see cref="ATCBOT_Controller.getFlightData(string)"/> to be handled. 
        /// Except in the case of the "atc" command which is handled locally.
        /// In the event no matching bang is found the String array is set to null.
        /// <exception cref="System.ArgumentException">Thrown when a message cannot yet be read from the stream.</exception>
        /// </summary>
        /// <returns>String array with two elements. If the variable is not null the first element is a user name and the command, and the second element is just the command.</returns>
        private string[] findBang()
        {
            string[] args = new string[2];
            string[] bangs = { "!atc", "!alt", "!flight", "!hdg", "!metar", "!plane", "!route", "!speed", "!temp", "!time", "!wind", "!wx" };
            args[0] = Twitch_Controller.Instance.readMessage();
            //ATCBOT_View.Instance.displayText(args[0]); // display all twitch strings
            try
            {
                if (args[0].Contains("!"))
                {
                    args[1] = args[0].Substring(args[0].LastIndexOf('!')).ToLower();
                    if (bangs.Contains(args[1]))
                    {
                        args[0] = args[0].Substring(1, ((args[0].IndexOf("!") - args[0].IndexOf(":")) - 1));
                        if (args[1].Contains("!atc"))
                        {
                            string info = "Welcome to ATC Bot! I can offer info about the current flight with the following commands: !alt, !flight, !hdg, !metar, !plane, !speed, !temp, !time, !wind, !wx.";
                            Twitch_Controller.Instance.sendChatMessage(info);
                        }
                        else { ATCBOT_Controller.Instance.getFlightData(args[1]); }
                    }
                    else { args = null; }
                }
                else { args = null; }
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                string message = e.Message;
                args = null;
            }
            return args;
        }
    }
}
