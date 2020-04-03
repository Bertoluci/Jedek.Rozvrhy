using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class StudentForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        //private int id = 0;
        private string jmeno;
        private string prijmeni;
        private string osobniCislo;
        private StudijniObor studijniObor;
        private int rocnik;


        /// <summary>
        /// Objekt pro správu studijních oborů
        /// </summary>
        public StudijniOborManager StudijniOborManager { get; private set; }


        /// <summary>
        /// Seznam zkratek studijních oborů
        /// </summary>
        public Dictionary<string, int> ZkratkyOboru { get; set; }


        /// <summary>
        /// řetězec zkratek studijních oborů pro nabídku ve výběru
        /// </summary>
        public string SeznamZkratek { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="studijniOborManager"></param>
        public StudentForm(StudijniOborManager studijniOborManager)
        {
            StudijniOborManager = studijniOborManager;
            ZkratkyOboru = new Dictionary<string, int>();
            SeznamZkratek = String.Empty;
            foreach (KeyValuePair<int, StudijniObor> obor in StudijniOborManager.StudijniObory)
            {
                SeznamZkratek += obor.Value.Zkratka + ", ";
                ZkratkyOboru.Add(obor.Value.Zkratka, obor.Value.Id);
            }
            char[] charsToTrim = { ',', ' ' };
            SeznamZkratek = SeznamZkratek.TrimEnd(charsToTrim);
            ShowForm();
        }


        /// <summary>
        /// Vykreslí formulář
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tVytvoření nového studenta:");

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


            bool ok = false;
            do // osobniCislo
            {
                Console.Write("\t\tOsobní Čislo: ");
                osobniCislo = Console.ReadLine();
                ok = Regex.IsMatch(osobniCislo, @"^[A][0-9]{5,7}$");
                if (ok)
                {
                    var foo = StudijniOborManager.Databaze.Studenti.FirstOrDefault(x => x.Value.OsobniCislo == osobniCislo);
                    if (foo.Value != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\tZadané osobní číslo je již evidováno u jiného studenta.");
                        Console.ForegroundColor = ConsoleColor.Black;
                        ok = false;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\tZadali jste špatný formát osobního čísla." + @" (^[A][0-9]{5,7}$)");
                    Console.ForegroundColor = ConsoleColor.Black;
                }

            } while (!ok);


            Console.WriteLine("\t\tDostupné studijní obory: {0}", SeznamZkratek);
            string input = String.Empty;
            do // studijni obor
            {
                Console.Write("\t\tStudijniObor: ");
                input = Console.ReadLine();
            } while (!ZkratkyOboru.ContainsKey(input));
            studijniObor = StudijniOborManager.StudijniObory[ZkratkyOboru[input]];


            input = String.Empty;
            do // rocnik
            {
                Console.Write("\t\tRočník: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out rocnik) || !(rocnik > 0 && rocnik < 4));


        }

        // gettery privátních fieldů (prvky formuláře)

        //public int GetId() { return id; }
        public string GetJmeno() { return jmeno; }
        public string GetPrijmeni() { return prijmeni; }
        public string GetOsobniCislo() { return osobniCislo; }
        public StudijniObor GetStudijniObor() { return studijniObor; }
        public int GetRocnik() { return rocnik; }

    }
}