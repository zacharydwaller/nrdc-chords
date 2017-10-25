using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDataFeeder
{
    public class Logger
    {
        private static Logger instance;

        public TextBox LogBox { get; set; }

        private Logger() {}

        public static Logger Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        public void LogError(string str)
        {
            Console.Error.WriteLine(str);
            LogToBox(str);
        }

        public void Log(string str)
        {
            Console.WriteLine(str);
            LogToBox(str);
        }

        private void LogToBox(string str)
        {
            if (LogBox != null)
            {
                LogBox.AppendText(Environment.NewLine + Environment.NewLine);
                LogBox.AppendText(str);
            }
        }
    }
}
