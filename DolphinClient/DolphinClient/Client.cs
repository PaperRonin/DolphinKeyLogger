using System;
using System.Diagnostics;
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


        public static void Disconnect()
        {
            client?.Close();
            stream?.Close();
        }
        public static string SendCommand(string str)
        {
            byte[] data = new byte[1000000];
            client = new TcpClient();
            try
            {
                while (!client.Connected)
                {
                    client.Connect(server, port);
                }
                stream = client.GetStream();

                byte[] sendBuffer = Encoding.UTF8.GetBytes(str);
                stream.Write(sendBuffer, 0, sendBuffer.Length);
                if (str == "screen")
                {
                    stream.Read(data, 0, data.Length);

                    using (var ms = new MemoryStream(data))
                    {
                        var bitmap = new Bitmap(ms);
                        bitmap.Save("test.png", ImageFormat.Png);

                    }

                    Disconnect();
                    Process.Start(new ProcessStartInfo("test.png"));
                    return "\n<screenshot saved>";
                }

                int bytes = stream.Read(data, 0, data.Length);
                Buffer = (Encoding.UTF8.GetString(data, 0, bytes));
                Disconnect();
                return "\n" + Buffer;
            }
            catch (SocketException e)
            {
                Disconnect();
                return e.Message;
            }
        }
    }
}
