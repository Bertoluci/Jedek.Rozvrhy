using System;
using System.Text.RegularExpressions;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class EditVyucujiciForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        private int id = 0;
        private string jmeno = String.Empty;
        private string prijmeni = String.Empty;
        private string tituly = String.Empty;


        /// <summary>
        /// Objekt k editaci
        /// </summary>
        public Vyucujici Vyucujici { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="v"></param>
        public EditVyucujiciForm(Vyucujici v)
        {
            Vyucujici = v;
            jmeno = v.Jmeno;
            prijmeni = v.Prijmeni;
            tituly = v.Tituly;
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tEditace vyučujícího:");

            do // jmeno
            {
                Console.Write("\r\n\t\tJméno [{0}]: ", Vyucujici.Jmeno);
                jmeno = Console.ReadLine();
            } while (!Regex.IsMatch(jmeno, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽÄËÏÖÜäïöüÿ ]+$") && jmeno != String.Empty);


            do // prijmeni
            {
                Console.Write("\t\tPříjmení [{0}]: ", Vyucujici.Prijmeni);
                prijmeni = Console.ReadLine();
            } while (!Regex.IsMatch(prijmeni, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽÄËÏÖÜäïöüÿ ]+$") && prijmeni != String.Empty);


            do // tituly
            {
                Console.Write("\t\tTituly [{0}]: ", Vyucujici.Tituly);
                tituly = Console.ReadLine();
            } while (!Regex.IsMatch(tituly, @"^[a-zA-Z ,.]+$") && tituly != String.Empty);

        }


        // gettery privátních fieldů (prvky formuláře)
       
        public int GetId() { return id; }
        public string GetJmeno() { return jmeno; }
        public string GetPrijmeni() { return prijmeni; }
        public string GetTituly() { return tituly; }

    }
}