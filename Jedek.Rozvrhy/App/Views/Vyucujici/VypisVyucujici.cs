using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.Vyucujici
{
    class VypisVyucujici : View
    {
        public VypisVyucujici(Dictionary<string, Object> context)
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
                Console.WriteLine("\tVýpis vyučujících");
                Console.WriteLine("\r\n\t\tVyberte způsob řazení:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: dle ID");
                Console.WriteLine("\t\t2: dle jména");
                Console.WriteLine("\t\t3: dle příjmení");
                Console.WriteLine("\t\t4: dle titulů");

                Console.Write("\r\n\t\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 5);
            } while (!ok);


            Dictionary<int, Models.Vyucujici> vyucujici = (Dictionary<int, Models.Vyucujici>)Context["vyucujici"];


            var serazeniVyucujici = from pair in vyucujici
                                    orderby pair.Value.Id ascending
                                    select pair;
            switch (volba)
            {
                case 1:
                    break;
                case 2:
                    serazeniVyucujici = from pair in vyucujici
                                        orderby pair.Value.Jmeno ascending
                                        select pair;
                    break;
                case 3:
                    serazeniVyucujici = from pair in vyucujici
                                        orderby pair.Value.Prijmeni ascending
                                        select pair;
                    break;
                case 4:
                    serazeniVyucujici = from pair in vyucujici
                                        orderby pair.Value.Tituly ascending
                                        select pair;
                    break;
            }

            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();

            do
            {
                printHeader();

                var v = serazeniVyucujici.ToList();
                int count = v.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné vyučující.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("Vyucujici", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {

                    actualId.Add(v[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-5} {1,-15} {2,-20} {3,-15}", vyucujici[actualId[j]].Id, vyucujici[actualId[j]].Jmeno, vyucujici[actualId[j]].Prijmeni, vyucujici[actualId[j]].Tituly);
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\tPro správu vyučujícího zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write("\r\n\tPro detail vyučujícího zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);


                            // bylo vybráno id vyučujícího
                            if (input != String.Empty)
                            {
                                if (Uzivatel.Role == Role.admin) // začátek pro admina
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("\t1: editace vyučujícího");

                                    if (MaVyucujiciZapsanPredmet(vyucujici[item]) || vyucujici[item].Role == Role.admin.ToString())
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t2: odstranění vyučujícího");
                                    Console.ForegroundColor = ConsoleColor.Black;

                                    Console.WriteLine("\t3: detail vyučujícího");
                                    Console.WriteLine("\t4: storno (pokračovat ve výpisu)");

                                    volba = 0;
                                    ok = false;
                                    do // volba editace / výmaz
                                    {
                                        Console.Write("\r\n\tVaše volba: ");
                                        key = Console.ReadKey();
                                        ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 5);
                                        if ((MaVyucujiciZapsanPredmet(vyucujici[item]) || vyucujici[item].Role == Role.admin.ToString()) && volba == 2)
                                        {
                                            ok = false;
                                        }
                                    } while (!ok);

                                } // konec pro admina ostatním nastavit volba = 3 (ostatní v předešlém kroku vybírají id vyučujícího přímo pro detail, tedy volbu 3)

                                if (Uzivatel.Role != Role.admin)
                                {
                                    volba = 3;
                                }

                                // zpacování volby editace / odstranění / storno
                                switch (volba)
                                {
                                    case 1: // editace
                                        this.OnEditItem(new EditVyucujiciForm(vyucujici[item]));
                                        break;

                                    case 2: // odstranění
                                        if (vyucujici.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(vyucujici[item]);
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
                                        Console.Write("\r\n\t{0} {1}", vyucujici[item].Prijmeni, vyucujici[item].Jmeno);
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine(", {0}", vyucujici[item].Tituly);

                                        List<string> prednasky = new List<string>();
                                        foreach (var predmet in vyucujici[item].Prednasky)
                                        {
                                            prednasky.Add(predmet.Value.Nazev);
                                        }
                                        List<string> cviceni = new List<string>();
                                        foreach (var predmet in vyucujici[item].Cviceni)
                                        {
                                            cviceni.Add(predmet.Value.Nazev);
                                        }
                                        List<string> seminare = new List<string>();
                                        foreach (var predmet in vyucujici[item].Seminare)
                                        {
                                            seminare.Add(predmet.Value.Nazev);
                                        }
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\tPřednášky: ");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("{0}", String.Join(", ", prednasky));

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\t  Cvičení: ");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("{0}", String.Join(", ", cviceni));

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\t Semináře: ");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("{0}", String.Join(", ", seminare));

                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\r\n\tRozvrhové akce:\r\n");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        foreach (var den in vyucujici[item].Rozvrh)
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
                                                    Console.WriteLine("\t\t{0,-8}{1,-10} {2,-25} {3,-10} {4,-12}" //[U{0}/{1}]
                                                                    , akce.Value.Den + " " + akce.Value.Zacatek + ":00"
                                                                    , " - " + (akce.Value.Zacatek + akce.Value.Delka) + ":00"
                                                                    , akce.Value.Predmet.Nazev
                                                                    , akce.Value.TypVyuky
                                                                    , "[U" + akce.Value.Mistnost.Budova + "/" + akce.Value.Mistnost.Cislo + "]");

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
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.Vyucujici)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }


        private void printHeader()
        {
            Console.Clear();
            Console.WriteLine("\t   ------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Jméno           Příjmení             Tituly");
            Console.WriteLine("\t   ------------------------------------------------------------------");
        }



        private bool MaVyucujiciZapsanPredmet(Models.Vyucujici ucitel)
        {
            if (ucitel.Prednasky.Count > 0)
            {
                return true;
            }
            else if (ucitel.Cviceni.Count > 0)
            {
                return true;
            }
            else if (ucitel.Seminare.Count > 0)
            {
                return true;
            }
            return false;
        }


    }
}