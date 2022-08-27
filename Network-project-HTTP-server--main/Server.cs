using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
/// <summary>
/// /////////
/// </summary>
namespace HTTPServer
{
    class Server
    {
        //Server Members
        Socket serverSocket;
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket       
            LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint HEPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(HEPoint);
            StartServer();

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(1000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket cl_Socket = this.serverSocket.Accept();
                Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                thread.Start(cl_Socket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket cl_socket = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            cl_socket.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] data = new byte[5000];
                    int recievedDataLength = cl_socket.Receive(data);

                    // TODO: break the while loop if receivedLen==0
                    if (recievedDataLength == 0)
                    {
                        Console.WriteLine("Client: ended the connection", cl_socket.RemoteEndPoint);
                        break;
                    }
                    string receivedData = Encoding.ASCII.GetString(data);
                    Console.WriteLine("Received data: " + receivedData);

                    // TODO: Create a Request object using received request string

                    Request req = new Request(receivedData);

                    // TODO: Call HandleRequest Method that returns the response
                    Response res = HandleRequest(req);
                    Console.WriteLine("Response: " + res.ResponseString);
                    // TODO: Send Response back to client
                    byte[] resArray = new byte[5000];
                    resArray = Encoding.ASCII.GetBytes(res.ResponseString);
                    cl_socket.Send(resArray);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class

                    Logger.LogException(ex);
                    break;
                }
            }

            // TODO: close client socket
            cl_socket.Close();
        }

        Response HandleRequest(Request request)
        {
            string Content = "text/html; charset=UTF-8";
            try
            {
                
                //TODO: check for bad request 
                if (request.ParseRequest() == true)
                {
                    
                    //TODO: map the relativeURI in request to get the physical path of the resource.
                    string RedirectPage = GetRedirectionPagePathIFExist(request.relativeURI);
                    //TODO: check for redirect
                    if (RedirectPage != "")
                    {
                        string redirectFile = File.ReadAllText(Configuration.RootPath + Configuration.RedirectionDefaultPageName);
                        Response res = new Response(StatusCode.Redirect, Content, redirectFile, RedirectPage);
                        return res;
                    }
                    string filePath = Configuration.RootPath + request.relativeURI;
                
                    //TODO: check file exists
                    if (File.Exists(filePath))
                    {
                        //TODO: read the physical file
                        string fileText = File.ReadAllText(filePath);
                        Response res = new Response(StatusCode.OK, Content, fileText, "");
                        return res;
                    }
                    else
                    {
                        string notFoundPath = Configuration.RootPath + Configuration.NotFoundDefaultPageName;
                        string notFoundText = File.ReadAllText(notFoundPath);
                        Response res = new Response(StatusCode.NotFound, Content, notFoundText, "");
                        return res;
                    }
                }
                else
                {
                    Console.WriteLine("badreq: ");
                    string badRequestPath = Configuration.RootPath + Configuration.BadRequestDefaultPageName;
                    string badReqText = File.ReadAllText(badRequestPath);
                    Response res = new Response(StatusCode.BadRequest, Content, badReqText, "");
                    return res;
                }
                // Create OK response
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                string internalErrorPath = Configuration.RootPath + Configuration.InternalErrorDefaultPageName;
                string internalErrorText = File.ReadAllText(internalErrorPath);
                Response res = new Response(StatusCode.InternalServerError, Content, internalErrorText, "");
                return res;
            }

        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            foreach (KeyValuePair<string, string> val in Configuration.RedirectionRules)
            {
                if (("/" + val.Key) == relativePath)
                {
                    return val.Value;
                }
            }
            return string.Empty;
        }

        //Handled in HandleRequest()

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                Configuration.RedirectionRules = new Dictionary<string, string>();
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var fileReader = new StreamReader(fileStream, Encoding.UTF8, true, 128);
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    string[] redirection_Name = line.Split(' ');
                    Configuration.RedirectionRules.Add(redirection_Name[0], redirection_Name[1]);
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Console.WriteLine("Error in reading File");
            }
        }
    }
}
