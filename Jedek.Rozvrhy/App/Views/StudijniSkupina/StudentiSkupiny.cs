using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.StudijniSkupina
{
    class StudentiSkupiny : View
    {

        public StudentiSkupiny(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            Models.StudijniSkupina skupina = (Models.StudijniSkupina)Context["skupina"];

            Dictionary<int, Models.Student> studentiSkupiny = skupina.StudentiSkupiny;

            Dictionary<int, Models.Student> studenti = (Dictionary<int, Models.Student>)Context["studenti"];

            string input = String.Empty;
            int i = 0;
            int itrBuffer = 0;
            int item = 0;
            int volba = 0;
            ConsoleKeyInfo key;
            bool ok = false;
            bool emptyBuffer = false;
            int bufferSize = 20;
            List<int> actualId = new List<int>();

            do // volba editace
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tSPRÁVA STUDENTŮ SKUPINY");
                Console.WriteLine("\r\n\tZvolte další postup:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: Výpis studentů skupiny");
                Console.WriteLine("\t\t2: Přidání studentů skupině");
                Console.WriteLine("\t\t3: Storno");

                Console.Write("\r\n\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 4);
            } while (!ok);  // volba editace

            switch (volba)
            {
                case 1: // Výpis studentů skupiny + odstaňování
                    do
                    {
                        printHeader(skupina);

                        var serazeniStudentiSkupiny = from pair in skupina.StudentiSkupiny
                                                    orderby pair.Value.Id ascending
                                                    select pair;

                        var ss = serazeniStudentiSkupiny.ToList();
                        int count = ss.Count;
                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tSkupina zatím neobsahuje žádného studenta.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat.");
                            Console.ReadKey();
                            Request("StudijniSkupina", "StudentiSkupiny", skupina);
                        }

                        bool odd = true;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            actualId.Add(ss[i].Key);
                            itrBuffer++;

                            if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                            {
                                do
                                {
                                    odd = true;
                                    emptyBuffer = false;
                                    for (int j = 0; j < actualId.Count; j++)
                                    {
                                        if (odd)
                                        {
                                            Console.Write("\t\t{0,-5} {1,-30}", studentiSkupiny[actualId[j]].Id, studentiSkupiny[actualId[j]].Prijmeni + studentiSkupiny[actualId[j]].Jmeno);
                                            odd = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", studentiSkupiny[actualId[j]].Id, studentiSkupiny[actualId[j]].Prijmeni + studentiSkupiny[actualId[j]].Jmeno);
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        if (!odd)
                                        {
                                            Console.WriteLine();
                                        }
                                        Console.Write("\r\n\tZadejte ID studenta, kterého chcete odebrat, nebo enter posun/ukončení: ");
                                        input = Console.ReadLine();
                                        ok = int.TryParse(input, out item);
                                        if (ok) ok = actualId.Contains(item);
                                        if (!ok) ok = input == String.Empty;

                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (studentiSkupiny.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(studentiSkupiny[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // nebylo zvoleno
                                        actualId.RemoveRange(0, actualId.Count);
                                    }
                                    printHeader(skupina);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 1: odebírání)
                    Request("StudijniSkupina", "StudentiSkupiny", skupina);
                    break;

                case 2: // Plnění skupiny studenty
                    do
                    {
                        printHeader(skupina);
                        var s = studenti.ToList();
                        int count = s.Count;

                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádného studenta.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat.");
                            Console.ReadKey();
                            Request("StudijniSkupina", "StudentiSkupiny", skupina);
                        }

                        bool odd = true;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            if (skupina.StudentiSkupiny.ContainsKey(s[i].Key) || !s[i].Value.ZapsanePredmety.ContainsKey(skupina.Predmet.Id))
                            {
                                continue;
                            }
                            else
                            {
                                actualId.Add(s[i].Key);
                                itrBuffer++;
                            }

                            if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                            {
                                do
                                {
                                    odd = true;
                                    emptyBuffer = false;
                                    for (int j = 0; j < actualId.Count; j++)
                                    {
                                        if (odd)
                                        {
                                            Console.Write("\t\t{0,-5} {1,-30}", studenti[actualId[j]].Id, studenti[actualId[j]].Prijmeni + studenti[actualId[j]].Jmeno);
                                            odd = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", studenti[actualId[j]].Id, studenti[actualId[j]].Prijmeni + studenti[actualId[j]].Jmeno);
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        if (!odd)
                                        {
                                            Console.WriteLine();
                                        }
                                        Console.Write("\r\n\tZadejte ID studenta, kterého chcete zapsat, nebo enter posun/ukončení: ");
                                        input = Console.ReadLine();
                                        ok = int.TryParse(input, out item);
                                        if (ok) ok = actualId.Contains(item);
                                        if (!ok) ok = input == String.Empty;

                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (studenti.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnAddItem(studenti[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // nebylo zvoleno
                                        actualId.RemoveRange(0, actualId.Count);
                                    }
                                    printHeader(skupina);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 2: přidávání)
                    Request("StudijniSkupina", "StudentiSkupiny", skupina);
                    break;

                case 3: // Storno
                    break;
            }

        } // render

        // překrytí události (přidání)
        protected override void OnAddItem(Model item)
        {
            base.OnAddItem(item);
        }

        // překrytí události (odebrání)
        protected override void OnDeleteItem(Model item)
        {
            base.OnDeleteItem(item);
        }


        private void printHeader(Models.StudijniSkupina skupina)
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\t\tSTUDIJNÍ SKUPINY JSOU TVOŘENY AUTOMATICKY, UPRAVUJTE OBEZŘETNĚ!\r\n");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tSpráva studentů studijní skupiny ID: {0} předmětu: {1}", skupina.Id, skupina.Predmet.Nazev);

            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Jméno                                     ID    Jméno");
            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
        }

    }
}