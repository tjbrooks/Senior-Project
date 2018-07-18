using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATCBot_Dev
{
    /// <summary>
    /// Twitch_Controller is used to connect and interact with the Twitch chat.
    /// Communicating with Twitch chat channels is done through IRC(Internet Relay Chat),
    /// which is the underlying system used by Twitch to operate chat channels.
    /// </summary>
    public class Twitch_Controller
    {
        private const string ip = "irc.twitch.tv";
        private const int port = 6667;
        private string userName;
        private string nickName;
        private string channel;
        private TimeSpan lastMessage;
        private int messagesInSpan;
        private static Twitch_Controller instance = new Twitch_Controller();

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        /// <summary>
        /// Instance is used to access the <see cref="Twitch_Controller"/> object being used.
        /// </summary>
        public static Twitch_Controller Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// Twitch_Controller is an empty constructor. The variables needed for a base <see cref="Twitch_Controller"/> are intatiated at creation.
        /// </summary>
        Twitch_Controller()
        {

        }
        /// <summary>
        /// setClient allows for the <see cref="Twitch_Controller"/> to establish a connection to a specific Twitch channel requiring a <paramref name="userName"/> and <paramref name="password"/>.
        /// After setting variables and connecting to input/output streams the spefic <paramref name="channel"/> will be attempted <see cref="joinChannel(string)"/>. 
        /// </summary>
        /// <param name="userName">Is the login id of the Twitch user.</param>
        /// <param name="password">Must is the client secret, which must be in the form of oAuth. Provided at https://twitchapps.com/tmi/. </param>
        /// <param name="channel">The specific chat channel the user wants to access. Typically the streamers name or found after https://www.twitch.tv/ on the specific stream page</param>
        public void setClient(string userName, string password, string channel)
        {
            this.userName = userName;
            this.nickName = userName.Normalize();
            this.channel = channel;
            try
            {
                tcpClient = new TcpClient();
                if (tcpClient.ConnectAsync(ip, port).Wait(1000))
                {
                    inputStream = new StreamReader(tcpClient.GetStream());
                    outputStream = new StreamWriter(tcpClient.GetStream());
                    outputStream.WriteLine("PASS " + password);
                    outputStream.WriteLine("NICK " + nickName);
                    outputStream.WriteLine("USER " + userName + " 8 8 :" + userName);
                    joinChannel(channel);
                }
                else {  }
            }catch(Exception e)
            {
                string message = e.Message;
            }

        }

        /// <summary>
        /// joinChannel connects to a specific Twitch channel via IRC protocol.
        /// </summary>
        /// <param name="ch"><see cref="setClient(string, string, string)"/></param>
        private void joinChannel(string ch)
        {
            channel = ch.Normalize();
            outputStream.WriteLine("JOIN #" + channel);
            outputStream.Flush();
            lastMessage = DateTime.Now - DateTime.Now.Date;
            messagesInSpan++;
        }
        /// <summary>
        /// leaveChannel parts the client from the connected Twitch channel. 
        /// The connection to Twitch IRC server is maintained, but communication to/from the channel is no longer possible.
        /// </summary>
        public void leaveChannel()
        {
            try
            {
                outputStream.WriteLine("PART #" + channel);
                outputStream.Flush();
                ATCBOT_View.Instance.displayText("Discconected from Twitch channel: " + channel);
                channel = "";
                tcpClient.Close();
            }
            catch (Exception e) { string message = e.Message; }
        }
        /// <summary>
        /// getChannel is used to retrieve the channel that the client is currently accessing.
        /// </summary>
        /// <returns><see cref="setClient(string, string, string)"/></returns>
        public string getChannel()
        {
            return channel;
        }
        /// <summary>
        /// sendIrcMessage will send a raw string to the IRC feed.
        /// </summary>
        /// <remarks>
        /// IRC requires specific formatting of messages. Therefore sendIrcMessage is private to prevent messaging errors.
        /// A check will not be sent until <see cref="safeToMessage"/> is true.
        /// </remarks>
        /// <param name="message">String to be sent to the IRC feed</param>
        private void sendIrcMessage(string message)
        {

            while (!safeToMessage()) { }
            outputStream.WriteLine(message);
            outputStream.Flush();
            messagesInSpan++;
        }
        /// <summary>
        /// safeToMessage ensures that the system does not send more messages than allowed by Twitch standards.
        /// Twitch requires that no more than 20 messages are sent in a 30 second span.
        /// </summary>
        /// <returns></returns>
        private bool safeToMessage()
        {
            TimeSpan now = DateTime.Now - DateTime.Now.Date;
            if ((now - lastMessage).CompareTo(new TimeSpan(0, 0, 30)) > 0)
            {
                messagesInSpan = 0;
                lastMessage = DateTime.Now - DateTime.Now.Date;
                return true;
            }
            else if (messagesInSpan <= 19)
            {
                return true;
            }
            else
            {
                ATCBOT_View.Instance.displayText("Twitch message limit reached.");
                return false;
            }
        }
        /// <summary>
        /// connected returns the status of the TcpClient. The connection only be the result of the last communication with the server, therefore not indicate an immediate disconnect.
        /// </summary>
        /// <returns></returns>
        public bool connected()
        {
            return tcpClient.Connected;
        }
        /// <summary>
        /// sendChatMessage is used to recieve a message that is desired to be displayed in the Twitch channels chat.
        /// The method will a apply the proper formatting to be accepted by IRC's protocol.
        /// After formatting the message is sent to <see cref="sendIrcMessage(string)"/>.
        /// </summary>
        /// <param name="message">String to be displayed in chat</param>
        public void sendChatMessage(string message)
        {
            sendIrcMessage(":" + userName + "!" + userName + "@" + userName
               + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
        }
        /// <summary>
        /// sendPong is used to send a specific response message to the Twitch channel.
        /// This method is required to ensure that the client is not disconnected from the channel.
        /// </summary>
        private void sendPong()
        {
            //PING: tmi.twitch.tv
            sendIrcMessage("Pong :tmi.twitch.tv");
        }
        /// <summary>
        /// readMessage pulls a line from the channel stream. 
        /// A check is done for a specific message from Twitch that must be handled via <see cref="sendPong"/>
        /// </summary>
        /// <returns>A raw string from the channel stream without alteration.</returns>
        public string readMessage()
        {
            string message = inputStream.ReadLine();
            if (message != null && message.Contains("PING")) sendPong();
            return message;
        }
    }
}