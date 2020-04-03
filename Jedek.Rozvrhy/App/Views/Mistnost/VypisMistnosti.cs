using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Mistnost
{
    class VypisMistnosti : View
    {
        public VypisMistnosti(Dictionary<string, Object> context)
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

            do // volba řazení
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tVýpis Místností");
                Console.WriteLine("\r\n\t\tVyberte způsob řazení:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: dle ID");
                Console.WriteLine("\t\t2: dle kapacity");
                Console.WriteLine("\t\t3: dle čísla budovy");
                Console.WriteLine("\t\t4: dle čísla místnosti");
                Console.Write("\r\n\t\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 5);
            } while (!ok);


            Dictionary<int, Models.Mistnost> mistnosti = (Dictionary<int, Models.Mistnost>)Context["mistnosti"];


            var serazeneMistnosti = from pair in mistnosti
                                    orderby pair.Value.Id ascending
                                    select pair;
            switch (volba)
            {
                case 1:
                    break;
                case 2:
                    serazeneMistnosti = from pair in mistnosti
                                        orderby pair.Value.Kapacita ascending
                                        select pair;
                    break;
                case 3:
                    serazeneMistnosti = from pair in mistnosti
                                        orderby pair.Value.Budova ascending
                                        select pair;
                    break;
                case 4:
                    serazeneMistnosti = from pair in mistnosti
                                        orderby pair.Value.Cislo ascending
                                        select pair;
                    break;
            }


            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();

            do
            {
                printHeader();

                var m = serazeneMistnosti.ToList();
                int count = m.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné místnosti.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("Mistnost", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {

                    actualId.Add(m[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-6} {1,-12} U{2,-12} {3,-15}", mistnosti[actualId[j]].Id, mistnosti[actualId[j]].Kapacita, mistnosti[actualId[j]].Budova, mistnosti[actualId[j]].Cislo);
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\tPro správu místnosti zadejte její ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write("\r\n\tPro detail místnosti zadejte její ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);

                            // bylo vybráno id místnosti
                            if (input != String.Empty)
                            {
                                if (Uzivatel.Role == Role.admin) // začátek pro admina
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("\t1: editace místnosti");
                                    if (MaMistnostRozvrhovouAkci(mistnosti[item]))
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t2: odstranění místnosti");
                                    Console.ForegroundColor = ConsoleColor.Black;

                                    Console.WriteLine("\t3: detail místnosti");
                                    Console.WriteLine("\t4: storno (pokračovat ve výpisu)");

                                    volba = 0;
                                    ok = false;
                                    do // volba editace / výmaz
                                    {
                                        Console.Write("\r\n\tVaše volba: ");
                                        key = Console.ReadKey();
                                        ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 5);
                                        if (MaMistnostRozvrhovouAkci(mistnosti[item]) && volba == 2)
                                        {
                                            ok = false;
                                        }
                                    } while (!ok);

                                } // konec pro admina ostatním nastavit volba = 3 (ostatní v předešlém kroku vybírají id místnosti přímo pro detail, tedy volbu 3)

                                if (Uzivatel.Role != Role.admin)
                                {
                                    volba = 3;
                                }

                                // zpacování volby editace / odstranění / storno
                                switch (volba)
                                {
                                    case 1: // editace
                                        this.OnEditItem(new EditMistnostForm(mistnosti[item]));
                                        break;

                                    case 2: // odstranění
                                        if (mistnosti.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(mistnosti[item]);
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
                                        Console.WriteLine("\r\n\t{0}", mistnosti[item]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tObsazenost:");
                                        Console.ForegroundColor = ConsoleColor.Black;

                                        printRozvrh(mistnosti[item]);

                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tRozvrhové akce:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        foreach (var den in mistnosti[item].Rozvrh)
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
                                                    Console.WriteLine("\t\t{0,-8}{1,-10} {2,-25} {3,-12} {4,-20}" //[U{0}/{1}]
                                                                    , akce.Value.Den + " " + akce.Value.Zacatek + ":00"
                                                                    , " - " + (akce.Value.Zacatek + akce.Value.Delka) + ":00"
                                                                    , akce.Value.Predmet.Nazev
                                                                    , akce.Value.TypVyuky
                                                                    , akce.Value.Vyucujici.Prijmeni + " " + akce.Value.Vyucujici.Jmeno + ", " + akce.Value.Vyucujici.Tituly);

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


        protected override void OnEditItem(Form form)
        {
            base.OnEditItem(form);
        }


        protected override void OnDeleteItem(Model item)
        {
            base.OnDeleteItem(item);
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.Mistnost)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }


        private void printHeader()
        {
            Console.Clear();
            Console.WriteLine("\t   ----------------------------------------------------------");
            Console.WriteLine("\t\tID    Kapacita     Budova     Číslo místnosti");
            Console.WriteLine("\t   ----------------------------------------------------------");
        }

        private bool MaMistnostRozvrhovouAkci(Models.Mistnost mistnost)
        {
            foreach (KeyValuePair<Dny, Dictionary<int, Models.RozvrhovaAkce>> rozvrhDne in mistnost.Rozvrh)
            {
                if (rozvrhDne.Value.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }


        private void printRozvrh(Models.Mistnost mistnost)
        {
            Console.WriteLine();
            Console.WriteLine("\t{0,-5} {1,-5} {2,-5} {3,-5} {4,-5} {5,-5} {6,-5} {7,-5} {8,-5} {9,-5} {10,-5} {11,-5} {12,-5} {13,-5} {14,-5} {15,-5} "
                , "", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00");

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Po, GetObsazenost(mistnost, Dny.Po, 7), GetObsazenost(mistnost, Dny.Po, 8), GetObsazenost(mistnost, Dny.Po, 9), GetObsazenost(mistnost, Dny.Po, 10), GetObsazenost(mistnost, Dny.Po, 11)
                , GetObsazenost(mistnost, Dny.Po, 12), GetObsazenost(mistnost, Dny.Po, 13), GetObsazenost(mistnost, Dny.Po, 14), GetObsazenost(mistnost, Dny.Po, 15), GetObsazenost(mistnost, Dny.Po, 16)
                , GetObsazenost(mistnost, Dny.Po, 17), GetObsazenost(mistnost, Dny.Po, 18), GetObsazenost(mistnost, Dny.Po, 19), GetObsazenost(mistnost, Dny.Po, 20), GetObsazenost(mistnost, Dny.Po, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Út, GetObsazenost(mistnost, Dny.Út, 7), GetObsazenost(mistnost, Dny.Út, 8), GetObsazenost(mistnost, Dny.Út, 9), GetObsazenost(mistnost, Dny.Út, 10), GetObsazenost(mistnost, Dny.Út, 11)
                , GetObsazenost(mistnost, Dny.Út, 12), GetObsazenost(mistnost, Dny.Út, 13), GetObsazenost(mistnost, Dny.Út, 14), GetObsazenost(mistnost, Dny.Út, 15), GetObsazenost(mistnost, Dny.Út, 16)
                , GetObsazenost(mistnost, Dny.Út, 17), GetObsazenost(mistnost, Dny.Út, 18), GetObsazenost(mistnost, Dny.Út, 19), GetObsazenost(mistnost, Dny.Út, 20), GetObsazenost(mistnost, Dny.Út, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.St, GetObsazenost(mistnost, Dny.St, 7), GetObsazenost(mistnost, Dny.St, 8), GetObsazenost(mistnost, Dny.St, 9), GetObsazenost(mistnost, Dny.St, 10), GetObsazenost(mistnost, Dny.St, 11)
                , GetObsazenost(mistnost, Dny.St, 12), GetObsazenost(mistnost, Dny.St, 13), GetObsazenost(mistnost, Dny.St, 14), GetObsazenost(mistnost, Dny.St, 15), GetObsazenost(mistnost, Dny.St, 16)
                , GetObsazenost(mistnost, Dny.St, 17), GetObsazenost(mistnost, Dny.St, 18), GetObsazenost(mistnost, Dny.St, 19), GetObsazenost(mistnost, Dny.St, 20), GetObsazenost(mistnost, Dny.St, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Čt, GetObsazenost(mistnost, Dny.Čt, 7), GetObsazenost(mistnost, Dny.Čt, 8), GetObsazenost(mistnost, Dny.Čt, 9), GetObsazenost(mistnost, Dny.Čt, 10), GetObsazenost(mistnost, Dny.Čt, 11)
                , GetObsazenost(mistnost, Dny.Čt, 12), GetObsazenost(mistnost, Dny.Čt, 13), GetObsazenost(mistnost, Dny.Čt, 14), GetObsazenost(mistnost, Dny.Čt, 15), GetObsazenost(mistnost, Dny.Čt, 16)
                , GetObsazenost(mistnost, Dny.Čt, 17), GetObsazenost(mistnost, Dny.Čt, 18), GetObsazenost(mistnost, Dny.Čt, 19), GetObsazenost(mistnost, Dny.Čt, 20), GetObsazenost(mistnost, Dny.Čt, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Pá, GetObsazenost(mistnost, Dny.Pá, 7), GetObsazenost(mistnost, Dny.Pá, 8), GetObsazenost(mistnost, Dny.Pá, 9), GetObsazenost(mistnost, Dny.Pá, 10), GetObsazenost(mistnost, Dny.Pá, 11)
                , GetObsazenost(mistnost, Dny.Pá, 12), GetObsazenost(mistnost, Dny.Pá, 13), GetObsazenost(mistnost, Dny.Pá, 14), GetObsazenost(mistnost, Dny.Pá, 15), GetObsazenost(mistnost, Dny.Pá, 16)
                , GetObsazenost(mistnost, Dny.Pá, 17), GetObsazenost(mistnost, Dny.Pá, 18), GetObsazenost(mistnost, Dny.Pá, 19), GetObsazenost(mistnost, Dny.Pá, 20), GetObsazenost(mistnost, Dny.Pá, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");
        }

        private string GetObsazenost(Models.Mistnost mistnost, Dny den, int hodina)
        {
            string obsazenost = mistnost.Rozvrh[den].ContainsKey(hodina) ? "  X" : "";
            return obsazenost;
        }

    }
}