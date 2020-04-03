using System;
using System.Text.RegularExpressions;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class EditStudijniOborForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        private int id = 0;
        private string nazev = string.Empty;
        private string zkratka = string.Empty;


        /// <summary>
        /// Objekt k editaci
        /// </summary>
        public StudijniObor StudijniObor { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="so"></param>
        public EditStudijniOborForm(StudijniObor so)
        {
            StudijniObor = so;
            nazev = so.Nazev;
            zkratka = so.Zkratka;
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tEditace studijního oboru:");

            do
            {
                Console.Write("\r\n\t\tNázev [{0}]: ", StudijniObor.Nazev);
                nazev = Console.ReadLine();
            } while (!Regex.IsMatch(nazev, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽ ]+$") && nazev != String.Empty);


            do
            {
                Console.Write("\t\tZkratka [{0}]: ", StudijniObor.Zkratka);
                zkratka = Console.ReadLine();
            } while (!Regex.IsMatch(zkratka, @"^[a-zA-Z0-9]+$") && zkratka != String.Empty);
        }


        // gettery privátních fieldů (prvky formuláře)
        public int GetId() { return id; }
        public string GetNazev() { return nazev; }
        public string GetZkratka() { return zkratka; }
    }
}