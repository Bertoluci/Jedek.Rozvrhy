using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.StudijniObor
{
    class PredmetyOboru : View
    {

        public PredmetyOboru(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            Models.StudijniObor obor = (Models.StudijniObor)Context["obor"];
            Dictionary<int, Models.Predmet> predmety = (Dictionary<int, Models.Predmet>)Context["predmety"];

            string input = String.Empty;
            int i = 0;
            int itrBuffer = 0;
            int item = 0;
            int volba = 0;
            ConsoleKeyInfo key;
            bool ok = false;
            bool emptyBuffer = false;
            List<int> actualId = new List<int>();

            do // volba editace
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tSPRÁVA PŘEDMĚTŮ STUDIJNÍHO OBORU");
                Console.WriteLine("\r\n\tZvolte další postup:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: Výpis předmětů oboru");
                Console.WriteLine("\t\t2: Plnění oboru předměty");
                Console.WriteLine("\t\t3: Storno");

                Console.Write("\r\n\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 4);
            } while (!ok);  // volba editace

            switch (volba)
            {
                case 1: // Výpis předmětů oboru + odstaňování
                    do
                    {
                        printHeader(obor);

                        var serazenePredmety = from pair in obor.PredmetyOboru
                                               orderby pair.Value.Id ascending
                                               select pair;

                        var p = serazenePredmety.ToList();
                        int count = p.Count;
                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tObor zatím neobsahuje žádné předměty.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                            Console.ReadKey();
                            Request("StudijniObor", "PredmetyOboru", obor);
                        }

                        bool odd = true;
                        int pocetNeodstanitelnych = 0;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            actualId.Add(p[i].Key);
                            itrBuffer++;

                            if (itrBuffer == 10 || (count < 10 && itrBuffer == count) || i == count - 1)
                            {
                                do
                                {
                                    odd = true;
                                    emptyBuffer = false;
                                    pocetNeodstanitelnych = 0;
                                    for (int j = 0; j < actualId.Count; j++)
                                    {
                                        if (odd)
                                        {
                                            if (MaPredmetStudenty(predmety[actualId[j]], obor))
                                            {
                                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                                pocetNeodstanitelnych++;
                                            }
                                            Console.Write("\t\t{0,-5} {1,-30}", predmety[actualId[j]].Id, predmety[actualId[j]].Nazev);
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            odd = false;
                                        }
                                        else
                                        {
                                            if (MaPredmetStudenty(predmety[actualId[j]], obor))
                                            {
                                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                                pocetNeodstanitelnych++;
                                            }
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", predmety[actualId[j]].Id, predmety[actualId[j]].Nazev);
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        if (pocetNeodstanitelnych == actualId.Count)
                                        {
                                            if(!odd)
                                            {
                                                Console.WriteLine();
                                            }
                                            Console.Write("\r\n\tStiskněte libovolnou klávesu posun/ukončení: ");
                                            Console.ReadKey();
                                            input = String.Empty;
                                        }
                                        else
                                        {
                                            if (!odd)
                                            {
                                                Console.WriteLine();
                                            }
                                            Console.Write("\r\n\tZadejte ID předmětu, který chcete odebrat, nebo enter posun/ukončení: ");
                                            input = Console.ReadLine();
                                            ok = int.TryParse(input, out item);
                                            if (ok) ok = actualId.Contains(item);
                                            if (!ok) ok = input == String.Empty;
                                            if (input != String.Empty && MaPredmetStudenty(predmety[item], obor))
                                            {
                                                ok = false;
                                            }
                                        }
                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (predmety.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(predmety[item]);
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
                                    printHeader(obor);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 2: přidávání)
                    Request("StudijniObor", "PredmetyOboru", obor);
                    break;

                case 2: // Plnění oboru předměty
                    do
                    {
                        printHeader(obor);
                        var p = predmety.ToList();
                        int count = p.Count;

                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tV systému dosud neexistuje žádný předmět.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                            Console.ReadKey();
                            Request("StudijniObor", "PredmetyOboru", obor);
                        }

                        bool odd = true;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            if (obor.PredmetyOboru.ContainsKey(p[i].Key))
                            {
                                continue;
                            }
                            else
                            {
                                actualId.Add(p[i].Key);
                                itrBuffer++;
                            }

                            if (itrBuffer == 10 || (count < 10 && itrBuffer == count) || i == count - 1)
                            {
                                do
                                {
                                    odd = true;
                                    emptyBuffer = false;
                                    for (int j = 0; j < actualId.Count; j++)
                                    {
                                        if (odd)
                                        {
                                            Console.Write("\t\t{0,-5} {1,-30}", predmety[actualId[j]].Id, predmety[actualId[j]].Nazev);
                                            odd = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", predmety[actualId[j]].Id, predmety[actualId[j]].Nazev);
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        if (!odd)
                                        {
                                            Console.WriteLine();
                                        }
                                        Console.Write("\r\n\tZadejte ID předmětu, který chcete přidat, nebo enter posun/ukončení: ");
                                        input = Console.ReadLine();
                                        ok = int.TryParse(input, out item);
                                        if (ok) ok = actualId.Contains(item);
                                        if (!ok) ok = input == String.Empty;

                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (predmety.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnAddItem(predmety[item]);
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
                                    printHeader(obor);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 2: přidávání)
                    Request("StudijniObor", "PredmetyOboru", obor);
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


        private void printHeader(Models.StudijniObor obor)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tSpráva předmětů studijního oboru: {0}", obor);

            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Název                                     ID    Název");
            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
        }

        private bool MaPredmetStudenty(Models.Predmet predmet, Models.StudijniObor obor)
        {
            foreach (KeyValuePair<int, Models.Student> student in predmet.Studujici)
            {
                if (student.Value.StudijniObor == obor)
                {
                    return true;
                }
            }
            return false;
        }

    }

}