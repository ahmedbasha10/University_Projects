using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines = new Dictionary<string, string>();
        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;
        string[] parsedString;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        /// 
        public bool ParseRequest()
        {

            string[] parseBlankLine = new string[] { "\r\n\r\n" };
            string[] parse = new string[] { "\r\n" };
            parsedString = requestString.Split(parseBlankLine, StringSplitOptions.None);
            requestLines = parsedString[0].Split(parse, StringSplitOptions.None);

            // Parse Request line
            if (ParseRequestLine() == false)
            {
                return false;
            }
            // Validate blank line exists
            if (ValidateBlankLine() == false)
            {
                return false;
            }

            // Load header lines into HeaderLines dictionary
            if (LoadHeaderLines() == false)
            {
                return false;
            }
            return true;

        }

        private bool ParseRequestLine()
        {
            try
            {
                string[] requestLine = requestLines[0].Split(' ');
                if (requestLine[0] == "GET")
                {
                    if (requestLine[0] == "GET")
                        method = RequestMethod.GET;

                }
                else
                    return false;

                if (requestLine[2] == "HTTP/1.1" || requestLine[2] == "HTTP/1.0" || requestLine[2] == " ")
                {
                    if (requestLine[2] == "HTTP/1.1")
                    {
                        httpVersion = HTTPVersion.HTTP11;
                    }
                    else if (requestLine[2] == "HTTP/1.0")
                    {
                        httpVersion = HTTPVersion.HTTP10;
                    }
                    else
                    {
                        httpVersion = HTTPVersion.HTTP09;
                    }
                }
                else
                {
                    return false;
                }

                if (ValidateIsURI(requestLine[1]) == false)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        private bool ValidateIsURI(string uri)
        {

            if (Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            {
                relativeURI = uri;
                return true;
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool LoadHeaderLines()
        {

            foreach (var val in requestLines)
            {
                if (val == "GET")
                {
                    continue;
                }
                string[] header = val.Split(' ');
                if (header.Length == 0)
                {
                    return false;
                }
                else
                {
                    headerLines.Add(header[0], header[1]);
                }
            }
            return true;
        }

        private bool ValidateBlankLine()
        {

            if (parsedString.Length == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
            return true;
        }

    }
}
