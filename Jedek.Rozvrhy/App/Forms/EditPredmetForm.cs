using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class EditPredmetForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        private int id = 0;
        private string zkratka = String.Empty;
        private string nazev = String.Empty;
        private int hodinPrednasek;
        private int hodinCviceni;
        private int hodinSeminaru;


        /// <summary>
        /// Objekt k editaci
        /// </summary>
        public Predmet Predmet { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p"></param>
        public EditPredmetForm(Predmet p)
        {
            Predmet = p;
            zkratka = p.Zkratka;
            nazev = p.Nazev;
            hodinPrednasek = p.HodinPrednasek;
            hodinCviceni = p.HodinCviceni;
            hodinSeminaru = p.HodinSeminaru;
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tEditace předmětu:");

            do // Nazev
            {
                Console.Write("\r\n\t\tNázev [{0}]: ", Predmet.Nazev);
                nazev = Console.ReadLine();
            } while (!Regex.IsMatch(nazev, @"^[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽ ]+$") && nazev != String.Empty);


            do // Zkratka
            {
                Console.Write("\t\tZkratka [{0}]: ", Predmet.Zkratka);
                zkratka = Console.ReadLine();
            } while (!Regex.IsMatch(zkratka, @"^[A-Z0-9]{5}$") && zkratka != String.Empty);


            bool ok = false;
            string input = String.Empty;

            if (MaTypVyukyRozvrhovouAkci(TypyVyuky.Přednáška))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\t\tHodin přednášek [{0}] ", Predmet.HodinPrednasek);
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                do // hodinPrednasek
                {
                    Console.Write("\t\tHodin přednášek [{0}]: ", Predmet.HodinPrednasek);
                    input = Console.ReadLine();
                    if (input != String.Empty)
                    {
                        ok = int.TryParse(input, out hodinPrednasek);
                    }
                    else
                    {
                        ok = true;
                    }
                } while (!ok);
            }


            ok = false;
            if (MaTypVyukyRozvrhovouAkci(TypyVyuky.Cvičení))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\t\tHodin Cvičení [{0}] ", Predmet.HodinCviceni);
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                do // hodinCviceni
                {
                    Console.Write("\t\tHodin Cvičení [{0}]: ", Predmet.HodinCviceni);
                    input = Console.ReadLine();
                    if (input != String.Empty)
                    {
                        ok = int.TryParse(input, out hodinCviceni);
                    }
                    else
                    {
                        ok = true;
                    }
                } while (!ok);
            }


            ok = false;
            if (MaTypVyukyRozvrhovouAkci(TypyVyuky.Seminář))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\t\tHodinSeminářů [{0}] ", Predmet.HodinSeminaru);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\r\n\t\tStiskněte libovolnou klávesu.");
                Console.ReadKey();
            }
            else
            {
                do // hodinSeminaru
                {
                    Console.Write("\t\tHodinSeminářů [{0}]: ", Predmet.HodinSeminaru);
                    input = Console.ReadLine();
                    if (input != String.Empty)
                    {
                        ok = int.TryParse(input, out hodinSeminaru);
                    }
                    else
                    {
                        ok = true;
                    }
                } while (!ok);
            }
        }


        /// <summary>
        /// Zjištuje, jestli je v místnosti nějaká výuka typu z parametru
        /// </summary>
        /// <param name="typVyuky"></param>
        /// <returns></returns>
        private bool MaTypVyukyRozvrhovouAkci(TypyVyuky typVyuky)
        {
            foreach (KeyValuePair<int, StudijniSkupina> skupina in Predmet.StudijniSkupiny)
            {
                foreach (KeyValuePair<Dny, Dictionary<int, RozvrhovaAkce>> rozvrhDne in skupina.Value.Rozvrh)
                {
                    foreach (KeyValuePair<int, RozvrhovaAkce> akce in rozvrhDne.Value)
                    {
                        if (akce.Value.TypVyuky == typVyuky)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        // gettery privátních fieldů (prvky formuláře)

        public int GetId() { return id; }
        public string GetZkratka() { return zkratka; }
        public string GetNazev() { return nazev; }
        public int GetHodinPrednasek() { return hodinPrednasek; }
        public int GetHodinCviceni() { return hodinCviceni; }
        public int GetHodinSeminaru() { return hodinSeminaru; }

    }
}
