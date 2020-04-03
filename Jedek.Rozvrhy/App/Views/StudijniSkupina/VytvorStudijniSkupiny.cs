using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.StudijniSkupina
{
    class VytvorStudijniSkupiny : View
    {
        public VytvorStudijniSkupiny(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            if (Context.ContainsKey("message"))
            {
                // chybové hlášení
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\n\r\n\t{0}", Context["message"]);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\r\n\t\tStiskněte jakoukoli klávesu.");
                Console.ReadKey();
                Request("StudijniSkupina", "Default", null);
            }
            else
            {
                Dictionary<int, Models.StudijniSkupina> studijniSkupiny = (Dictionary<int, Models.StudijniSkupina>)Context["studijniSkupiny"];
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\r\n\r\n\t Bylo vytvořeno {0} studijních skupin", studijniSkupiny.Count);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\r\n\t\tStiskněte jakoukoli klávesu.");
                Console.ReadKey();
                Request("StudijniSkupina", "Default", null);
            }

        }
    }
}