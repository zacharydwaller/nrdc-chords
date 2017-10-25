using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDataFeeder
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string portalUrl = "http://ec2-52-8-224-195.us-west-1.compute.amazonaws.com/";
            ChordsInterface chords = new ChordsInterface(portalUrl);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(chords));
        }
    }
}
