using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Predmet
{
    class VypisPredmety : View
    {

        public VypisPredmety(Dictionary<string, Object> context)
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
                Console.WriteLine("\tVýpis předmětů");
                Console.WriteLine("\r\n\t\tVyberte způsob řazení:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: dle ID");
                Console.WriteLine("\t\t2: dle zkratky");
                Console.WriteLine("\t\t3: dle názvu");
                Console.WriteLine("\t\t4: dle hodin přednášek");
                Console.WriteLine("\t\t5: dle hodin cvičení");
                Console.WriteLine("\t\t6: dle hodin seminářů");

                Console.Write("\r\n\t\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 7);
            } while (!ok);

            Dictionary<int, Models.Predmet> predmety = (Dictionary<int, Models.Predmet>)Context["predmety"];

            var serazenePredmety = from pair in predmety
                                   orderby pair.Value.Id ascending
                                   select pair;
            switch (volba)
            {
                case 1:
                    break;
                case 2:
                    serazenePredmety = from pair in predmety
                                       orderby pair.Value.Zkratka ascending
                                       select pair;
                    break;
                case 3:
                    serazenePredmety = from pair in predmety
                                       orderby pair.Value.Nazev ascending
                                       select pair;
                    break;
                case 4:
                    serazenePredmety = from pair in predmety
                                       orderby pair.Value.HodinPrednasek ascending
                                       select pair;
                    break;
                case 5:
                    serazenePredmety = from pair in predmety
                                       orderby pair.Value.HodinCviceni ascending
                                       select pair;
                    break;
                case 6:
                    serazenePredmety = from pair in predmety
                                       orderby pair.Value.HodinSeminaru ascending
                                       select pair;
                    break;
            }


            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();


            do
            {
                printHeader();

                var p = serazenePredmety.ToList();
                int count = p.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné předměty.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("Predmet", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {

                    actualId.Add(p[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-5} {1,-10} {2,-35} {3,-10} {4,-10} {5,-10}", predmety[actualId[j]].Id, predmety[actualId[j]].Zkratka, predmety[actualId[j]].Nazev, predmety[actualId[j]].HodinPrednasek, predmety[actualId[j]].HodinCviceni, predmety[actualId[j]].HodinSeminaru);
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\tPro správu předmětu zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {

                                    Console.Write("\r\n\tPro detail předmětu zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);

                            // bylo vybráno id předmětu
                            if (input != String.Empty)
                            {
                                if (Uzivatel.Role == Role.admin) // začátek pro admina
                                { 
                                    Console.WriteLine();
                                    Console.WriteLine("\t1: editace předmětu");
                                    if (predmety[item].Studujici.Count > 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t2: odstranění předmětu");
                                    Console.ForegroundColor = ConsoleColor.Black;

                                    if (predmety[item].HodinPrednasek == 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t3: správa přednášejících předmětu");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    if (predmety[item].HodinCviceni == 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t4: správa cvičících předmětu");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    if (predmety[item].HodinSeminaru == 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t5: správa vedoucích semináře předmětu");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine("\t6: detail předmětu");
                                    Console.WriteLine("\t7: storno (pokračovat ve výpisu) ");

                                    volba = 0;
                                    ok = false;
                                    do
                                    {
                                        Console.Write("\r\n\tVaše volba: ");
                                        key = Console.ReadKey();
                                        ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 7);
                                        if (ok) ok = (predmety[item].Studujici.Count > 0 && volba == 2) ? false : true;
                                        if (ok) ok = (predmety[item].HodinPrednasek == 0 && volba == 3) ? false : true;
                                        if (ok) ok = (predmety[item].HodinCviceni == 0 && volba == 4) ? false : true;
                                        if (ok) ok = (predmety[item].HodinSeminaru == 0 && volba == 5) ? false : true;
                                    } while (!ok);

                                } // konec pro admina ostatním nastavit volba = 6 (ostatní v předešlém kroku vybírají id předmětu přímo pro detail, tedy volbu 6)

                                if (Uzivatel.Role != Role.admin)
                                {
                                    volba = 6;
                                }

                                // zpacování volby editace / odstranění / storno
                                switch (volba)
                                {
                                    case 1: // editace
                                        this.OnEditItem(new EditPredmetForm(predmety[item]));
                                        break;

                                    case 2: // odstranění
                                        if (predmety.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(predmety[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                        break;

                                    case 3: // přednášející
                                        Request("Predmet", "PrednasejiciPredmetu", predmety[item]);
                                        break;

                                    case 4: // cvičící
                                        Request("Predmet", "CviciciPredmetu", predmety[item]);
                                        break;

                                    case 5: // vedoucí semináře
                                        Request("Predmet", "VedouciSeminarePredmetu", predmety[item]);
                                        break;

                                    case 6: // detail
                                        Console.Clear();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.WriteLine("\r\n\t{0}", predmety[item]);
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        //Console.WriteLine(", {0}", vyucujici[item].Tituly);

                                        // přednášky
                                        if (predmety[item].Prednasejici.Count > 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine("\r\n\tPřednášející:");
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            foreach (var pr in predmety[item].Prednasejici)
                                            {
                                                Console.WriteLine("\t\t{0}, {1}", pr.Value.Prijmeni + " " + pr.Value.Jmeno, pr.Value.Tituly);
                                            }
                                        }

                                        // cvičení
                                        if (predmety[item].Cvicici.Count > 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine("\r\n\tCvičící:");
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            foreach (var pr in predmety[item].Cvicici)
                                            {
                                                Console.WriteLine("\t\t{0}, {1}", pr.Value.Prijmeni + " " + pr.Value.Jmeno, pr.Value.Tituly);
                                            }
                                        }

                                        // semináře
                                        if (predmety[item].VedeSeminar.Count > 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine("\r\n\tVede seminář:");
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            foreach (var pr in predmety[item].VedeSeminar)
                                            {
                                                Console.WriteLine("\t\t{0}, {1}", pr.Value.Prijmeni + " " + pr.Value.Jmeno, pr.Value.Tituly);
                                            }
                                        }

                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\tPočet zapsaných studentů: ");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("{0}", predmety[item].Studujici.Count);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\r\n\tPočet studijních skupin: ");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("{0}", predmety[item].StudijniSkupiny.Count);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tRozvrhové akce:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;

                                        var sorted = predmety[item].RozvrhoveAkce.Values.OrderBy(s => s.Den).ThenBy(s => s.Zacatek);

                                        foreach (var akce in sorted)
                                        {
                                            Console.WriteLine("\t\t{0,-8}{1,-10} {2,-25} {3,-10} {4,-12} {5,-20}" //[U{0}/{1}]
                                                                    , akce.Den + " " + akce.Zacatek + ":00"
                                                                    , " - " + (akce.Zacatek + akce.Delka) + ":00"
                                                                    , akce.Predmet.Nazev
                                                                    , akce.TypVyuky
                                                                    , "[U" + akce.Mistnost.Budova + "/" + akce.Mistnost.Cislo + "]"
                                                                    , akce.Vyucujici.Prijmeni + " " + akce.Vyucujici.Jmeno);
                                        }

                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.Write("\r\n\r\n\tPro návrat stiskněte libovolnou klávesu.");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.ReadKey();
                                        break;

                                    case 7: // storno
                                        //Request("Predmet", "Default", null);
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
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.Predmet)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }


        private void printHeader()
        {
            Console.Clear();
            Console.WriteLine("\t   ------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Zkratka    Název                             Přednášek  Cvičení   Seminářů");
            Console.WriteLine("\t   ------------------------------------------------------------------------------------------");
        }
    }
}