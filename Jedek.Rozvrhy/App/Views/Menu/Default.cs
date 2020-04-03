using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;


namespace Jedek.Rozvrhy.App.Views.Menu
{
    class Default : View
    {

        public Default(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            Dictionary<string, MenuItem> items = (Dictionary<string, MenuItem>)Context["items"];

            string selectedItem = String.Empty;
            ConsoleKeyInfo key;
            bool ok = false;
            do
            {
                Console.Clear();
                Console.Write("\r\n\tPřihlášen: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{0}", Uzivatel);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\t");
                Console.Write("Role: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", Uzivatel.Role);
                Console.ForegroundColor = ConsoleColor.Black;


                Console.WriteLine("\r\n\tVyberte požadovanou sekci:\r\n");

                foreach (KeyValuePair<string, MenuItem> item in items)
                {
                    Console.WriteLine("\t\t{0}: {1}", item.Key, item.Value.Name);
                    Console.WriteLine();
                }
                Console.Write("\r\n\tVaše volba: ");
                key = Console.ReadKey();
                selectedItem = key.KeyChar.ToString().ToUpper();
                ok = items.ContainsKey(selectedItem);
            } while (!ok);

            // testovací
            Console.Clear();

            this.Request(items[selectedItem].Controller, items[selectedItem].Action, null);

        }
    }
}
