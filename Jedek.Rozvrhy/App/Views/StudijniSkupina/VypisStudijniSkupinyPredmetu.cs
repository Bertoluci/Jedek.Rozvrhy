using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.StudijniSkupina
{
    class VypisStudijniSkupinyPredmetu : View
    {

        public VypisStudijniSkupinyPredmetu(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {

            string input = String.Empty;
            int i = 0;
            int itrBuffer = 0;
            int item = 0;
            int volba = 0;
            ConsoleKeyInfo key;
            bool ok = false;
            bool emptyBuffer = false;
            int bufferSize = 15;

            Dictionary<int, Models.StudijniSkupina> skupiny = (Dictionary<int, Models.StudijniSkupina>)Context["skupiny"];


            var serazeneSkupiny = from pair in skupiny
                                  orderby pair.Value.Id ascending
                                  select pair;

            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();

            do
            {
                printHeader();

                var s = serazeneSkupiny.ToList();
                int count = s.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné studijniSkupiny.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("StudijniSkupina", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {
                    bufferSize = s[i].Value.Predmet.StudijniSkupiny.Count;
                    actualId.Add(s[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-6} {1,-12} {2,-20}"
                                    , skupiny[actualId[j]].Id, skupiny[actualId[j]].StudentiSkupiny.Count, skupiny[actualId[j]].Predmet.Nazev);
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\tPro správu studijní skupiny zadejte její ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write("\r\n\tPro detail studijní skupiny zadejte její ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);

                            // bylo vybráno id skupiny
                            if (input != String.Empty)
                            {
                                if (Uzivatel.Role == Role.admin) // začátek pro admina
                                {
                                    // zkištění, jestli má skupina RA
                                    bool maRA = MaSkupinaRozvrhovouAkci(skupiny[item]);


                                    Console.WriteLine();

                                    if (maRA)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t1: správa studentů studijní skupiny");
                                    Console.WriteLine("\t2: odstranění studijní skupiny");
                                    Console.ForegroundColor = ConsoleColor.Black;

                                    Console.WriteLine("\t3: detail studijní skupiny");
                                    Console.WriteLine("\t4: storno (pokračovat ve výpisu) ");

                                    volba = 0;
                                    ok = false;
                                    do // volba editace / výmaz
                                    {
                                        Console.Write("\r\n\tVaše volba: ");
                                        key = Console.ReadKey();
                                        ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 5);
                                        if (maRA && (volba == 1 || volba == 2))
                                        {
                                            ok = false;
                                        }
                                    } while (!ok);

                                } // konec pro admina ostatním nastavit volba = 3 (ostatní v předešlém kroku vybírají id studijní skupiny přímo pro detail, tedy volbu 3)

                                if (Uzivatel.Role != Role.admin)
                                {
                                    volba = 3;
                                }

                                // zpacování volby odstranění / storno
                                switch (volba)
                                {

                                    case 1: // zápis
                                        Request("StudijniSkupina", "StudentiSkupiny", skupiny[item]);
                                        break;

                                    case 2: // odstranění
                                        if (skupiny.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(skupiny[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                        break;
                                    case 3: // detail
                                        Console.Clear();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.WriteLine("\r\n\t{0}", skupiny[item]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tStudenti skupiny:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        foreach (var st in skupiny[item].StudentiSkupiny)
                                        {
                                            Console.WriteLine("\t\t{0,-22}\t{1,-7}\t{2}.ročník", st.Value.Prijmeni + " " + st.Value.Jmeno, st.Value.StudijniObor.Zkratka, st.Value.Rocnik);
                                        }

                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tRozvrhové akce:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        foreach (var den in skupiny[item].Rozvrh)
                                        {
                                            var items = from pair in den.Value
                                                        orderby pair.Key ascending
                                                        select pair;

                                            int akceId = 0;
                                            foreach (var akce in items)
                                            {
                                                // nezobrazovat duplicity

                                                if (akceId != akce.Value.Id)
                                                {
                                                    Console.WriteLine("\t\t{0,-8}{1,-10} {2,-25} {3,-10} {4,-12} {5,-20}" //[U{0}/{1}]
                                                                    , akce.Value.Den + " " + akce.Value.Zacatek + ":00"
                                                                    , " - " + (akce.Value.Zacatek + akce.Value.Delka) + ":00"
                                                                    , akce.Value.Predmet.Nazev
                                                                    , akce.Value.TypVyuky
                                                                    , "[U" + akce.Value.Mistnost.Budova + "/" + akce.Value.Mistnost.Cislo + "]"
                                                                    , akce.Value.Vyucujici.Prijmeni + " " + akce.Value.Vyucujici.Jmeno);

                                                    akceId = akce.Value.Id;
                                                }

                                            }
                                        }

                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.Write("\r\n\r\n\tPro návrat stiskněte libovolnou klávesu.");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.ReadKey();
                                        break;
                                    case 4: // storno
                                        break;

                                }
                            }

                            if (input == String.Empty)
                            {
                                actualId.RemoveRange(0, actualId.Count);
                            }

                            printHeader();
                            itrBuffer = 0;
                        } while (input != String.Empty && !emptyBuffer); // smyčka načtených
                    }

                }

            } while (input != String.Empty && !emptyBuffer); // hlavní smyčka

        }


        protected override void OnDeleteItem(Model item)
        {
            base.OnDeleteItem(item);
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.StudijniSkupina)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }


        private void printHeader()
        {
            Console.Clear();
            if (Uzivatel.Role == Role.admin || Uzivatel.Role == Role.vyucujici)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\tSTUDIJNÍ SKUPINY JSOU TVOŘENY AUTOMATICKY, UPRAVUJTE OBEZŘETNĚ!\r\n");
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine("\t   -----------------------------------------------------------");
            Console.WriteLine("\t\tID     studentů     Předmět");
            Console.WriteLine("\t   -----------------------------------------------------------");
        }


        private bool MaSkupinaRozvrhovouAkci(Models.StudijniSkupina skupina)
        {
            foreach (var den in skupina.Rozvrh)
            {
                if (den.Value.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}