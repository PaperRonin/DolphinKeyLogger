using System;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(Client.Run);
            Thread.Sleep(1000);
            Client.SendCommand("screen");
            Thread.Sleep(100000);

            Console.ReadKey();
        }
    }
}
