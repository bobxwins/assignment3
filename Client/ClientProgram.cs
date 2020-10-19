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

      /*  public override string ToString()
        {
            return string.Format("Request information:\n\tMethod: {0}, Path: {1}, Date: {2} + Body {3}", Method,Path,Date,Body, string.Join(",",Body.ToArray()));
        }
       THE public override string Tostring() function is supposed to create a new line for each parameter, doesnt work and is unnecesary for this assignment */ 
    }
    class ClientProgram
    {
           
        static void Main(string[] args)
        {

            Request request = new Request() // gives the JSON object parameters
            {

                Method = "update",
                Path = "/api/categories/1",
                Date = 1434360957,
                Body = "1 AssignTest"
            };

            string Jsonrequest=JsonConvert.SerializeObject(request); // creates the method,path etc request, converts into json
            File.WriteAllText(@"request.json", Jsonrequest); // creates a file of the json and saves it on your PC. This is Not neccesary for anything lol.
           // Console.WriteLine(Jsonrequest); // prints the json into the console, also not necessariy for anything really
            Jsonrequest = String.Empty;
            Jsonrequest = File.ReadAllText(@"request.json"); // reads the unnecesary file
           Request DeserializeJson = JsonConvert.DeserializeObject<Request>(Jsonrequest); // Converts Json to string
            Console.WriteLine(DeserializeJson.ToString()); // writes the converted JSON to console, again not neccesariyyiyi

            using var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 5000);

            var stream = client.GetStream();

            var data = Encoding.UTF8.GetBytes("Hello bob" + Jsonrequest); // Json Object gets send to the server from the client.

            stream.Write(data);

            data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);

            Console.WriteLine($"Message from the server: {msg}");
        }
    }
     

}
