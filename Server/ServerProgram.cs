using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
 using Assignment3TestSuite;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Server
{
    class ServerProgram
    {
         
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Server started!");

            while (true)
            {
                var client = server.AcceptTcpClient();
                Console.WriteLine("Accepted client!");


                Thread thread = new Thread(new ParameterizedThreadStart(ClientObject));
                thread.Start(client);


            }

            static void ClientObject( object Clientobj)
            {
                var client =  (TcpClient)Clientobj;
                var stream = client.GetStream();


                var msg = Read(client, stream);

                Console.WriteLine($"Message from client {msg}");

                var data = Encoding.UTF8.GetBytes(msg.ToUpper());

                var ResponseMessage = new Response();

                var RequestMessage = new Request();
              
                string Jsonrequest = JsonConvert.SerializeObject(RequestMessage);

                Request DeserializeJson = JsonConvert.DeserializeObject<Request>(Jsonrequest);

                
                Console.WriteLine(DeserializeJson.ToString());

                Console.WriteLine(RequestMessage.method);

                var method = new string[5] { "create", "read", "update", "delete", "echo" };
                if (RequestMessage.method == null)
                {
                    ResponseMessage.Status += "missing method"; // missing method is method is empty
                }
                else
                {
                    bool methodValidation = false;
                    foreach (string indiMethods in method)
                    {
                        if (RequestMessage.method.ToLower() == indiMethods) { methodValidation = true; }
                    }
                    if (!methodValidation) { ResponseMessage.Status = "illegal method"; }
                }
                // method is illegl if its not one of indiMethods
                
                var data2 = Encoding.UTF8.GetBytes(Jsonrequest);
                stream.Write(data2);
                stream.Close();

               

            }
        }


        private static string Read(TcpClient client, NetworkStream stream)
        {
            byte[] data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);
            return msg;
        }

    }
    public class Request
    {
        public string method { get; set; }
        public string path { get; set; }
        public string date { get; set; }
        public string body { get; set; }
    }
}
