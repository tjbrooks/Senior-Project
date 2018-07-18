using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Add these two statements to all SimConnect clients 


namespace ATCBot_Dev
{
    static class Program
    {
        /// <summary>
        /// Main serves to begin the UI where the rest of the system is handled.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ATCBOT_View());
        }
    }
}
