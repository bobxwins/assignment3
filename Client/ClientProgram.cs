using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Client
{ 
    class Request
    {
    
         public string Method { get; set; }
        public string Path { get; set; }
        public int Date { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return string.Format("Request information:\n\tMethod: {0}, Path: {1}, Date: {2} + Body {3}", Method,Path,Date,Body, string.Join(",",Body.ToArray()));
        }
    }
    class ClientProgram
    {
           
        static void Main(string[] args)
        {

            Request request = new Request()
            {

                Method = "update",
                Path = "/api/categories/1",
                Date = 1434360957,
                Body = "1 AssignTest"
            };

            string Jsonrequest=JsonConvert.SerializeObject(request); // creates the method,path etc request, converts into json
            File.WriteAllText(@"request.json", Jsonrequest); // creates a file of the json
            Console.WriteLine(Jsonrequest); // prints the json into the console
            Jsonrequest = String.Empty;
            Jsonrequest = File.ReadAllText(@"request.json");
           Request DeserializeJson = JsonConvert.DeserializeObject<Request>(Jsonrequest);
            Console.WriteLine(DeserializeJson.ToString());

            using var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 5000);

            var stream = client.GetStream();

            var data = Encoding.UTF8.GetBytes("Hello bob" + Jsonrequest);

            stream.Write(data);

            data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);

            Console.WriteLine($"Message from the server: {msg}");
        }
    }
     

}
