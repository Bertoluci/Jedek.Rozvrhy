using System;
using Jedek.Rozvrhy.App;

namespace Jedek.Rozvrhy
{
    class Program
    {
        const string DefaultController = "Setting";
        const string DefaultAction = "Default";


        static void Main(string[] args)
        {
            //Console.SetWindowSize(150, 50);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;

            Object[] arg = {};
            Application app = new Application();
            app.CreateRequest(DefaultController, DefaultAction, arg);
        }

    }
}
