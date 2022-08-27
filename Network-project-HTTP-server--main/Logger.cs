using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            // for each exception write its details associated with datetime 
            // This text is added only once to the file.
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            sr.WriteLine("Date: " + DateTime.Now);
            //message:
            sr.WriteLine("Message: " + ex.Message);
            // for each exception write its details associated with datetime 

        }
    }
}

