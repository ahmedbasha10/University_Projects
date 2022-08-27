using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static string Redirection_path = "redirection.txt";
        static void Main(string[] args)
        {
            CreateRedirectionRulesFile();
            Server our_server = new Server(1000, Redirection_path);
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            if (!File.Exists(Redirection_path))
            {
                using (FileStream CreateFile = File.Create(Redirection_path))
                {
                    Byte[] redirectionPage = Encoding.ASCII.GetBytes("aboutus.html aboutus2.html");
                    CreateFile.Write(redirectionPage, 0, redirectionPage.Length);

                }
            }
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
        }
    }
}
