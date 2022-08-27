using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        string Content_type, ContentLength, Content, redirection;
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            this.code = code;
            this.Content_type = "Content Type : " + contentType + "\r\n";
            this.redirection = "Location : " + redirectoinPath + "\r\n";
            this.ContentLength = "Content-Length : " + content.Length + "\r\n";
            this.Content = content;
            //// TODO: Create the request string
            if (redirection != "")
                this.responseString = GetStatusLine(code) + Content_type + ContentLength + redirection + "\r\n" + Content;
            else
                this.responseString = GetStatusLine(code) + Content_type + ContentLength + "\r\n" + Content;

            // TODO: Create the request string
            Console.WriteLine("Response : " + this.responseString);
        }

        private string GetStatusLine(StatusCode code)
        {
            string statusLine = string.Empty;
            switch (code)
            {
                case StatusCode.OK:
                    statusLine = "HTTP/1.1 200 OK \r\n";
                    break;
                case StatusCode.BadRequest:
                    statusLine = "HTTP/1.1 400 BadRequest \r\n";
                    break;
                case StatusCode.InternalServerError:
                    statusLine = "HTTP/1.1 500 Internal Server Error \r\n";
                    break;
                case StatusCode.NotFound:
                    statusLine = "HTTP/1.1 404 Not Found \r\n";
                    break;
                case StatusCode.Redirect:
                    statusLine = "HTTP/1.1 301 Redirect \r\n";
                    break;
            }
            return statusLine;
        }
    }
}
