using System;
using System.Text.RegularExpressions;

namespace Jedek.Rozvrhy.App.Forms
{
    class VyucujiciForm : Form
    {

        // U property nelze nastavotat přes out v int.Parse
        //private int id = 0;
        private string jmeno;
        private string prijmeni;
        private string tituly;


        /// <summary>
        /// Constructor
        /// </summary>
        public VyucujiciForm()
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
            Console.WriteLine("\n\tVytvoření nového vyučujícího:");

            do // jmeno
            {
                Console.Write("\r\n\t\tJméno: ");
                jmeno = Console.ReadLine();
            } while (!Regex.IsMatch(jmeno, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽÄËÏÖÜäïöüÿ ]+$"));


            do // prijmeni
            {
                Console.Write("\t\tPříjmení: ");
                prijmeni = Console.ReadLine();
            } while (!Regex.IsMatch(prijmeni, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽÄËÏÖÜäïöüÿ ]+$"));


            do // tituly
            {
                Console.Write("\t\tTituly: ");
                tituly = Console.ReadLine();
            } while (!Regex.IsMatch(tituly, @"^[a-zA-Z ,.]+$"));

        }


        // gettery privátních fieldů (prvky formuláře)
       
        // public int GetId() { return id; }
        public string GetJmeno() { return jmeno; }
        public string GetPrijmeni() { return prijmeni; }
        public string GetTituly() { return tituly; }

    }
}
