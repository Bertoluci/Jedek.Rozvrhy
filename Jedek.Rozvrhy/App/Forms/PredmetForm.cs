using System;
using System.Text.RegularExpressions;

namespace Jedek.Rozvrhy.App.Forms
{
    class PredmetForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        //private int id = 0;
        private string zkratka;
        private string nazev;
        private int hodinPrednasek;
        private int hodinCviceni;
        private int hodinSeminaru;


        /// <summary>
        /// Constructor
        /// </summary>
        public PredmetForm()
        {
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tVytvoření nového předmětu:");

            do // Nazev
            {
                Console.Write("\r\n\t\tNázev: ");
                nazev = Console.ReadLine();
            } while (!Regex.IsMatch(nazev, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽ ]+$"));

            do // Zkratka
            {
                Console.Write("\t\tZkratka: ");
                zkratka = Console.ReadLine();
            } while (!Regex.IsMatch(zkratka, @"^[A-Z0-9]{5}$"));


            string input = String.Empty;
            do // hodinPrednasek
            {
                Console.Write("\t\tHodin přednášek: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out hodinPrednasek));


            do // hodinCviceni
            {
                Console.Write("\t\tHodin cvičení: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out hodinCviceni));


            do // hodinSeminaru
            {
                Console.Write("\t\tHodin seminářů: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out hodinSeminaru));
        }


        // gettery privátních fieldů (prvky formuláře)

        //public int GetId() { return id; }
        public string GetZkratka() { return zkratka; }
        public string GetNazev() { return nazev; }
        public int GetHodinPrednasek() { return hodinPrednasek; }
        public int GetHodinCviceni() { return hodinCviceni; }
        public int GetHodinSeminaru() { return hodinSeminaru; }

    }
}
