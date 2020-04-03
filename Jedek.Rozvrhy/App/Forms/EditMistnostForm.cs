using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class EditMistnostForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        private int id = 0;
        private int kapacita = 0;
        private int budova = 0;
        private int cislo = 0;


        /// <summary>
        /// Objekt místnosti k editaci
        /// </summary>
        public Mistnost Mistnost { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="m"></param>
        public EditMistnostForm(Mistnost m)
        {
            Mistnost = m;
            id = m.Id;
            kapacita = m.Kapacita;
            budova = m.Budova;
            cislo = m.Cislo;
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tEditace místnosti:");

            string input = String.Empty;
            if (MaMistnostRozvrhovouAkci())
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\r\n\t\tKapacita místnosti [{0}]", Mistnost.Kapacita);
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                do // kapacita
                {
                    Console.Write("\r\n\t\tKapacita místnosti (12 - 200) [{0}]: ", Mistnost.Kapacita);
                    input = Console.ReadLine();
                } while ((!int.TryParse(input, out kapacita) || !(kapacita > 11 && kapacita < 201)) && input != String.Empty);
            }


            do // budova
            {
                Console.Write("\t\tČíslo budovy [{0}]: ", Mistnost.Budova);
                input = Console.ReadLine();
            } while (!int.TryParse(input, out budova) && input != String.Empty);


            do // cislo
            {
                Console.Write("\t\tČíslo místnosti [{0}]: ", Mistnost.Cislo);
                input = Console.ReadLine();
            } while (!int.TryParse(input, out cislo) && input != String.Empty);

        }


        /// <summary>
        /// Zjištuje, jestli je v místnosti nějaká výuka
        /// </summary>
        /// <returns>bool</returns>
        private bool MaMistnostRozvrhovouAkci()
        {
            foreach (KeyValuePair<Dny, Dictionary<int, Models.RozvrhovaAkce>> rozvrhDne in Mistnost.Rozvrh)
            {
                if (rozvrhDne.Value.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }


        // gettery privátních fieldů (prvky formuláře)

        public int GetId() { return id; }
        public int GetKapacita() { return kapacita; }
        public int GetBudova() { return budova; }
        public int GetCislo() { return cislo; }
        //public TypMistnosti GetTypMistnosti() { return typ; }
    }
}
