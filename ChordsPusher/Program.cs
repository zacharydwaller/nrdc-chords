using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordsPusher
{
    class Program
    {
        static void Main(string[] args)
        {
            // @ symbol used to allow escaped characters like '/' in web address
            string url = @"http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com";
            var bot = new ChordsBot(url);
            bot.Login();
        }
    }
}
