using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.RozvrhovaAkce
{
    class VypisRozvrhoveAkce: View
    {
        public VypisRozvrhoveAkce(Dictionary<string, Object> context)
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
            int bufferSize = 20;

            do // volba řazení
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tVýpis rozvrhových akcí");
                Console.WriteLine("\r\n\t\tVyberte způsob řazení:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: dle ID");
                Console.WriteLine("\t\t2: dle předmětu");
                Console.WriteLine("\t\t3: dle typu výuky");
                Console.WriteLine("\t\t4: dle vyučujícího");
                Console.WriteLine("\t\t5: dle místnosti");
                Console.WriteLine("\t\t6: dle dnu výuky");
                Console.Write("\r\n\t\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 7);
            } while (!ok);


            Dictionary<int, Models.RozvrhovaAkce> rozvrhoveAkce = (Dictionary<int, Models.RozvrhovaAkce>)Context["rozvrhoveAkce"];


            var serazenRozvrhoveAkce = from pair in rozvrhoveAkce
                                   orderby pair.Value.Id ascending
                                   select pair;
            switch (volba)
            {
                case 1:
                    break;
                case 2:
                    serazenRozvrhoveAkce = from pair in rozvrhoveAkce
                                       orderby pair.Value.Predmet.Nazev ascending
                                       select pair;
                    break;
                case 3:
                    serazenRozvrhoveAkce = from pair in rozvrhoveAkce
                                       orderby pair.Value.TypVyuky.ToString() ascending
                                       select pair;
                    break;
                case 4:
                    serazenRozvrhoveAkce = from pair in rozvrhoveAkce
                                       orderby pair.Value.Vyucujici.Prijmeni ascending
                                       select pair;
                    break;
                case 5:
                    serazenRozvrhoveAkce = from pair in rozvrhoveAkce
                                       orderby pair.Value.Mistnost.Cislo ascending
                                       select pair;
                    break;
                case 6:
                    //serazenRozvrhoveAkce = from pair in rozvrhoveAkce
                    //                   orderby pair.Value.Den ascending
                    //                   select pair;
                    serazenRozvrhoveAkce = rozvrhoveAkce.OrderBy(s => s.Value.Den).ThenBy(s => s.Value.Zacatek);
                    break;
            }

            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();

            do
            {
                printHeader();

                var ra = serazenRozvrhoveAkce.ToList();
                int count = ra.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné rozvrhové akce.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("Student", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {

                    actualId.Add(ra[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-5} {1,-30} {2,-15} {3,-25} {4,-15} {5,-8}{6,-5}" //[U{0}/{1}]
                                    , rozvrhoveAkce[actualId[j]].Id, rozvrhoveAkce[actualId[j]].Predmet.Nazev, rozvrhoveAkce[actualId[j]].TypVyuky
                                    , rozvrhoveAkce[actualId[j]].Vyucujici.Prijmeni + " " +rozvrhoveAkce[actualId[j]].Vyucujici.Jmeno
                                    , "[U" + rozvrhoveAkce[actualId[j]].Mistnost.Budova + "/" + rozvrhoveAkce[actualId[j]].Mistnost.Cislo + "]"
                                    , rozvrhoveAkce[actualId[j]].Den + " " + rozvrhoveAkce[actualId[j]].Zacatek + ":00"
                                    , " - " + (rozvrhoveAkce[actualId[j]].Zacatek + rozvrhoveAkce[actualId[j]].Delka) + ":00");
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\t\tPro správu rozvrhové akce zadejte její ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write("\r\n\t\tPro posun stiskněte libovolnou klávesu: ");
                                    Console.ReadKey();
                                    input = String.Empty;
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);

                            // bylo vybráno id rozvrhové akce
                            if (input != String.Empty)
                            {
                                Console.WriteLine();
                                //Console.WriteLine("\t1: editace rozvrhové akce");
                                Console.WriteLine("\t2: odstranění rozvrhové akce  ");
                                Console.WriteLine("\t3: storno (pokračovat ve výpisu) ");

                                volba = 0;
                                ok = false;
                                do // volba editace / výmaz
                                {
                                    Console.Write("\r\n\tVaše volba: ");
                                    key = Console.ReadKey();
                                    ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 1 && volba < 4);
                                } while (!ok);

                                // zpacování volby editace / odstranění / storno
                                switch (volba)
                                {
                                    //case 1: // editace
                                    //    this.OnEditItem(new EditRozvrhovaAkceForm(rozvrhoveAkce[item], (RozvrhovaAkceManager)Context["RozvrhovaAkceManager"]));
                                    //    break;

                                    case 2: // odstranění
                                        if (rozvrhoveAkce.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(rozvrhoveAkce[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                        break;

                                    case 3: // storno
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
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.RozvrhovaAkce)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }


        private void printHeader()
        {
            Console.Clear();
            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\t{0,-5} {1,-30} {2,-15} {3,-25} {4,-15} {5,-8}{6,-5}", "ID", "Předmět", "Typ", "Vyučující", "Místnost", "Termín", "");
            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------------------------------");
        }
    }
}