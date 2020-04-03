using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class EditStudentForm : Form
    {

        // U property nelze nastavotat přes out v int.Parse
        private int id = 0;
        private string jmeno = string.Empty;
        private string prijmeni = string.Empty;
        private string osobniCislo = string.Empty;
        private StudijniObor studijniObor = null;
        private int rocnik = 0;


        /// <summary>
        /// Objekt k editaci
        /// </summary>
        public Student Student { get; private set; }


        /// <summary>
        /// Objekt pro správu studentů
        /// </summary>
        public StudijniOborManager StudijniOborManager { get; private set; }


        /// <summary>
        /// Slovník zkratek a id oborů
        /// </summary>
        public Dictionary<string, int> ZkratkyOboru { get; set; }


        /// <summary>
        /// Našeptávač validních hodnot
        /// </summary>
        public string SeznamZkratek { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s"></param>
        /// <param name="som"></param>
        public EditStudentForm(Student s, StudijniOborManager som)
        {
            Student = s;
            id = s.Id;
            jmeno = s.Jmeno;
            prijmeni = s.Prijmeni;
            osobniCislo = s.OsobniCislo;
            rocnik = s.Rocnik;

            StudijniOborManager = som;
            ZkratkyOboru = new Dictionary<string, int>();
            SeznamZkratek = String.Empty;
            foreach (KeyValuePair<int, StudijniObor> obor in StudijniOborManager.StudijniObory)
            {
                SeznamZkratek += obor.Value.Zkratka + ", ";
                ZkratkyOboru.Add(obor.Value.Zkratka, obor.Value.Id);
            }

            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tEditace studenta:");

            do // jmeno
            {
                Console.Write("\r\n\t\tJméno [{0}]: ", Student.Jmeno);
                jmeno = Console.ReadLine();
            } while (!Regex.IsMatch(jmeno, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽÄËÏÖÜäïöüÿ ]+$") && jmeno != String.Empty);


            do // prijmeni
            {
                Console.Write("\t\tPříjmení [{0}]: ", Student.Prijmeni);
                prijmeni = Console.ReadLine();
            } while (!Regex.IsMatch(prijmeni, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽÄËÏÖÜäïöüÿ ]+$") && prijmeni != String.Empty);


            bool ok = false;
            do // osobniCislo
            {
                Console.Write("\t\tOsobní Čislo [{0}]: ", Student.OsobniCislo);
                osobniCislo = Console.ReadLine();
                if (osobniCislo == String.Empty) break; // nechce editovat
                if (osobniCislo == Student.OsobniCislo) break; // zadáno shodné, také nepokračovat s validací
                ok = Regex.IsMatch(osobniCislo, @"^[A][0-9]{5}$");
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
                    Console.WriteLine("\t\tZadali jste špatný formát osobního čísla.");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            } while (!ok);


            string input = String.Empty;
            if (Student.ZapsanePredmety.Count == 0)
            {
                Console.WriteLine("\t\tDostupné studijní obory: {0}", SeznamZkratek);
                do // studijni obor
                {
                    Console.Write("\t\tStudijniObor [{0}]: ", Student.StudijniObor.Zkratka);
                    input = Console.ReadLine();
                } while (!ZkratkyOboru.ContainsKey(input) && input != String.Empty);
                if (input != String.Empty)
                {
                    studijniObor = StudijniOborManager.StudijniObory[ZkratkyOboru[input]];
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\t\tStudijniObor [{0}]", Student.StudijniObor.Zkratka);
                Console.ForegroundColor = ConsoleColor.Black;
            }


            input = String.Empty;
            do // rocnik
            {
                Console.Write("\t\tRočník [{0}]: ", Student.Rocnik.ToString());
                input = Console.ReadLine();
            } while ((!int.TryParse(input, out rocnik) || !(rocnik > 0 && rocnik < 4)) && input != String.Empty);

        }


        // gettery privátních fieldů (prvky formuláře)
        public int GetId() { return id; }
        public string GetJmeno() { return jmeno; }
        public string GetPrijmeni() { return prijmeni; }
        public string GetOsobniCislo() { return osobniCislo; }
        public StudijniObor GetStudijniObor() { return studijniObor; }
        public int GetRocnik() { return rocnik; }

    }
}