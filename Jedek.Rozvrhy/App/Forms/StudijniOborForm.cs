using System;
using System.Text.RegularExpressions;

namespace Jedek.Rozvrhy.App.Forms
{
    class StudijniOborForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        //private int id = 0;
        private string nazev;
        private string zkratka;


        /// <summary>
        /// Constructor
        /// </summary>
        public StudijniOborForm()
        {
            ShowForm();
        }


        /// <summary>
        /// Vykreslí formulář
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tVytvoření nového studijního oboru:");

            do
            {
                Console.Write("\r\n\t\tNázev: ");
                nazev = Console.ReadLine();
            } while (!Regex.IsMatch(nazev, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽ ]+$"));


            do
            {
                Console.Write("\t\tZkratka: ");
                zkratka = Console.ReadLine();
            } while (!Regex.IsMatch(zkratka, @"^[a-zA-Z0-9]+$"));
        }


        // gettery privátních fieldů (prvky formuláře)

        //public int GetId() { return id; }
        public string GetNazev() { return nazev; }
        public string GetZkratka() { return zkratka; }
    }
}
