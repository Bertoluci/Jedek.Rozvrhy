﻿using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.RozvrhovaAkce
{
    class Default : View
    {

        public Default(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            int selectedItem = 0;
            ConsoleKeyInfo key;
            bool ok = false;

            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tSpráva rozvrhových akcí.");
                Console.WriteLine("\n\r\tVyberte požadovanou akci:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: Výpis rozvrhových akcí");
                if (Uzivatel.Role != Role.admin)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.WriteLine("\t\t2: Přidání rozvrhové akce");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\t\t3: Zpět");
                Console.WriteLine();
                Console.Write("\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out selectedItem)) && (selectedItem > 0 && selectedItem < 4);
                if (Uzivatel.Role != Role.admin && selectedItem == 2)
                {
                    ok = false;
                }
            } while (!ok);

            if (selectedItem == 1) Request("RozvrhovaAkce", "VypisRozvrhoveAkce", null);
            if (selectedItem == 2) Request("RozvrhovaAkce", "PridejRozvrhovouAkci", null);
            if (selectedItem == 3) Request("Menu", "Default", null);
        }
    }
}