using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Student
{
    class VypisStudenty : View
    {

        public VypisStudenty(Dictionary<string, Object> context)
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
                Console.WriteLine("\tVýpis studentů");
                Console.WriteLine("\r\n\t\tVyberte způsob řazení:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: dle ID");
                Console.WriteLine("\t\t2: dle osobního čísla");
                Console.WriteLine("\t\t3: dle jména");
                Console.WriteLine("\t\t4: dle příjmení");
                Console.WriteLine("\t\t5: dle ročníku");
                Console.WriteLine("\t\t6: dle studijního oboru");
                Console.Write("\r\n\t\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 7);
            } while (!ok);


            Dictionary<int, Models.Student> studenti = (Dictionary<int, Models.Student>)Context["studenti"];


            var serazeniStudenti = from pair in studenti
                                   orderby pair.Value.Id ascending
                                   select pair;
            switch (volba)
            {
                case 1:
                    break;
                case 2:
                    serazeniStudenti = from pair in studenti
                                       orderby pair.Value.OsobniCislo ascending
                                       select pair;
                    break;
                case 3:
                    serazeniStudenti = from pair in studenti
                                       orderby pair.Value.Jmeno ascending
                                       select pair;
                    break;
                case 4:
                    serazeniStudenti = from pair in studenti
                                       orderby pair.Value.Prijmeni ascending
                                       select pair;
                    break;
                case 5:
                    serazeniStudenti = from pair in studenti
                                       orderby pair.Value.Rocnik ascending
                                       select pair;
                    break;
                case 6:
                    serazeniStudenti = from pair in studenti
                                       orderby pair.Value.StudijniObor.Id ascending
                                       select pair;
                    break;
            }

            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();

            do
            {
                printHeader();

                var s = serazeniStudenti.ToList();
                int count = s.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné studenty.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("Student", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {

                    actualId.Add(s[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-5} {1,-12} {2,-12} {3,-15} {4,-10} {5,-10}"
                                    , studenti[actualId[j]].Id, studenti[actualId[j]].OsobniCislo, studenti[actualId[j]].Jmeno
                                    , studenti[actualId[j]].Prijmeni, studenti[actualId[j]].Rocnik, studenti[actualId[j]].StudijniObor.Zkratka);
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\tPro správu studenta zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write("\r\n\tPro detail studenta zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);

                            // bylo vybráno id studenta
                            if (input != String.Empty)
                            {
                                if (Uzivatel.Role == Role.admin) // začátek pro admina
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("\t1: editace studenta ");

                                    if (studenti[item].StudijniSkupiny.Count > 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t2: odstranění studenta  ");
                                    Console.WriteLine("\t3: správa předmětů studenta");
                                    Console.ForegroundColor = ConsoleColor.Black;

                                    Console.WriteLine("\t4: detail studenta");
                                    Console.WriteLine("\t5: storno (pokračovat ve výpisu) ");

                                    volba = 0;
                                    ok = false;
                                    do // volba editace / výmaz
                                    {
                                        Console.Write("\r\n\tVaše volba: ");
                                        key = Console.ReadKey();
                                        ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 6);
                                        if (studenti[item].StudijniSkupiny.Count > 0 && (volba == 2 || volba == 3))
                                        {
                                            ok = false;
                                        }
                                    } while (!ok);

                                } // konec pro admina ostatním nastavit volba = 4 (ostatní v předešlém kroku vybírají id studenta přímo pro detail, tedy volbu 4)

                                if (Uzivatel.Role != Role.admin)
                                {
                                    volba = 4;
                                }

                                // zpacování volby editace / odstranění / storno
                                switch (volba)
                                {
                                    case 1: // editace
                                        this.OnEditItem(new EditStudentForm(studenti[item], (StudijniOborManager)Context["studijniOborManager"]));
                                        break;

                                    case 2: // odstranění
                                        if (studenti.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(studenti[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                        break;

                                    case 3: // zápis předmětů
                                        Request("Student", "PredmetyStudenta", studenti[item]);
                                        break;

                                    case 4: // detail
                                        Console.Clear();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.Write("\r\n\t{0} {1}", studenti[item].Prijmeni, studenti[item].Jmeno);
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("\t[{0}]\t{1}.ročník", studenti[item].OsobniCislo, studenti[item].Rocnik);
                                        Console.WriteLine("\t\t\t{0}", studenti[item].StudijniObor.Nazev);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tZapsané předměty:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        foreach (var p in studenti[item].ZapsanePredmety)
                                        {
                                            Console.WriteLine("\t\t{0}\t{1}", p.Value.Zkratka, p.Value.Nazev);
                                        }
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tRozvrhové akce:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        foreach (var den in studenti[item].Rozvrh)
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
                                    case 5: // storno
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
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.Student)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }


        private void printHeader()
        {
            Console.Clear();
            Console.WriteLine("\t   --------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Os. číslo    Jméno        Příjmení        Ročník     Obor");
            Console.WriteLine("\t   --------------------------------------------------------------------------");
        }
    }
}