using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{
    public static Dictionary<string, List<string>> Tcpservers = new Dictionary<string, List<string>>()
       {
          {"SetA",new List<string>{"One","Two"}},
          {"SetB",new List<string>{"Three","Four"}},
          {"SetC",new List<string>{"Five","Six"}},
          {"SetD",new List<string>{"Seven","Eight"}},
          {"SetE",new List<string>{"Nine","Ten"}},
       };

    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8000);
        server.Start();
        Console.WriteLine("Server started");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
            t.Start(client);
        }
    }
    public static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string received = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

        string[] parts = received.Split('-');
        string setkey = parts[0];
        string value = parts.Length > 1 ? parts[1] : null;

        if (Tcpservers.TryGetValue(setkey, out var values) && value != null && values.Contains(value))
        {
            foreach (string item in values)
            {
                string msg = $"{DateTime.Now}";
                byte[] outBuffer = Encoding.UTF8.GetBytes(msg);
                stream.Write(outBuffer, 0, outBuffer.Length);
                Thread.Sleep(1000);
            }
        }
        else
        {
            byte[] emptyBuffer = Encoding.UTF8.GetBytes("EMPTY");
            stream.Write(emptyBuffer, 0, emptyBuffer.Length);
        }
        client.Close();
    }
}