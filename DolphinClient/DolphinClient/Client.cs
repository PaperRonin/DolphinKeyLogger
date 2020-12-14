using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace DolphinClient
{
    static class Client
    {
        private const int port = 8888;
        private const string server = "127.0.0.1";
        static TcpClient client;
        static NetworkStream stream;

        public static event Action<string> stringReady;
        private static string _buffer = "";
        public static string Buffer
        {
            get => _buffer;
            set
            {
                _buffer = value;
                stringReady?.Invoke(_buffer);
            }
        }


        public static void Run()
        {
            try
            {
                client = new TcpClient();
                while (!client.Connected)
                {
                    client.Connect(server, port);
                }
                stream = client.GetStream();
                while (client.Connected)
                {
                    byte[] data = new byte[1024];
                    StringBuilder response = new StringBuilder();
                    if (stream.DataAvailable)
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        Buffer = (Encoding.UTF8.GetString(data, 0, bytes));
                        Console.WriteLine("\n" + Buffer);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public static void Disconnect()
        {
            client.Close();
            stream.Close();
        }
        public static void SendCommand(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            stream.Write(data, 0, data.Length);
            if (str == "screen")
            {
                data = new byte[1000000];
                stream.Read(data, 0, data.Length);

                using var ms = new MemoryStream(data);
                var bitmap = new Bitmap(ms);
                bitmap.Save("test.png", ImageFormat.Png);
            }
        }
    }
}
