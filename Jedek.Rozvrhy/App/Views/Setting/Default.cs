using System;
using System.Collections.Generic;
using System.Collections;
using Jedek.Rozvrhy.Libs;


namespace Jedek.Rozvrhy.App.Views.Setting
{
    class Default : View
    {
        public SortedList Parameters { get; private set; }

        public Default(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            Parameters = (SortedList)Context["parameters"];
            
            int selectedItem = 0;
            ConsoleKeyInfo key;
            bool ok = false;

            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tNastavení aplikace.");
                Console.WriteLine("\n\r\tVyberte požadovanou databázi:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: prázdná");
                Console.WriteLine("\t\t2: xml");
                Console.WriteLine("\t\t3: csv");
                Console.WriteLine();
                Console.Write("\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out selectedItem)) && (selectedItem > 0 && selectedItem < 4);
            } while (!ok);

            if (selectedItem == 1) Parameters.Add("repository", Repository.Session);
            if (selectedItem == 2) Parameters.Add("repository", Repository.XML);
            if (selectedItem == 3) Parameters.Add("repository", Repository.CSV);
            
            Request("Menu", "Default", null);
        }
    }
}
