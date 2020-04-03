using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Views.Predmet
{
    class VedouciSeminarePredmetu : View
    {

        public VedouciSeminarePredmetu(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            Models.Predmet predmet = (Models.Predmet)Context["predmet"];
            Dictionary<int, Models.Vyucujici> vyucujici = (Dictionary<int, Models.Vyucujici>)Context["vyucujici"];

            string input = String.Empty;
            int i = 0;
            int itrBuffer = 0;
            int item = 0;
            int volba = 0;
            ConsoleKeyInfo key;
            bool ok = false;
            bool emptyBuffer = false;
            int bufferSize = 10;
            List<int> actualId = new List<int>();

            do // volba editace
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tSPRÁVA VEDOUCÍCH SEMINÁŘE PŘEDMĚTU [{0}]", predmet.Zkratka);
                Console.WriteLine("\r\n\tZvolte další postup:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: Výpis vedoucích semináře předmětu");
                Console.WriteLine("\t\t2: Plnění předmětu vedoucími semináře");
                Console.WriteLine("\t\t3: Storno");

                Console.Write("\r\n\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 4);
            } while (!ok);  // volba editace

            switch (volba)
            {
                case 1: // Výpis vedoucích semináře předmětu + odstaňování
                    do
                    {
                        printHeader(predmet);

                        var serazeniVyucujici = from pair in predmet.VedeSeminar
                                                orderby pair.Value.Prijmeni ascending
                                                select pair;

                        var v = serazeniVyucujici.ToList();
                        int count = v.Count;
                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tPředmět zatím neobsahuje žádné vedoucí semináře.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                            Console.ReadKey();
                            Request("Predmet", "VedouciSeminarePredmetu", predmet);
                        }

                        bool odd = true;
                        int pocetNeodstanitelnych = 0;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            actualId.Add(v[i].Key);
                            itrBuffer++;

                            if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
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
                                            if (MaVedouciSeminarRozvrhovouAkciPredmetu(vyucujici[actualId[j]], predmet))
                                            {
                                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                                pocetNeodstanitelnych++;
                                            }
                                            Console.Write("\t\t{0,-5} {1,-30}", vyucujici[actualId[j]].Id, vyucujici[actualId[j]].Jmeno + " " + vyucujici[actualId[j]].Prijmeni);
                                            odd = false;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                        }
                                        else
                                        {
                                            if (MaVedouciSeminarRozvrhovouAkciPredmetu(vyucujici[actualId[j]], predmet))
                                            {
                                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                                pocetNeodstanitelnych++;
                                            }
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", vyucujici[actualId[j]].Id, vyucujici[actualId[j]].Jmeno + " " + vyucujici[actualId[j]].Prijmeni);
                                            odd = true;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                        }
                                    }
                                    do
                                    {
                                        if(pocetNeodstanitelnych == actualId.Count)
                                        {
                                            if (!odd)
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
                                            Console.Write("\r\n\tZadejte ID vedoucího semináře, kterého chcete odebrat, nebo enter posun/ukončení: ");
                                            input = Console.ReadLine();
                                            ok = int.TryParse(input, out item);
                                            if (ok) ok = actualId.Contains(item);
                                            if (!ok) ok = input == String.Empty;
                                            if (input != String.Empty && MaVedouciSeminarRozvrhovouAkciPredmetu(vyucujici[item], predmet))
                                            {
                                                ok = false;
                                            }
                                        }
                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (vyucujici.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(vyucujici[item]);
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
                                    printHeader(predmet);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 2: přidávání)
                    Request("Predmet", "VedouciSeminarePredmetu", predmet);
                    break;

                case 2: // Plnění předmětů vedoucími semináře
                    do
                    {
                        printHeader(predmet);

                        var serazeniVyucujici = from pair in vyucujici
                                                orderby pair.Value.Prijmeni ascending
                                                select pair;

                        var p = serazeniVyucujici.ToList();
                        int count = p.Count;

                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tV systému dosud neexistuje žádný vyučující.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat.");
                            Console.ReadKey();
                            Request("Predmet", "VedouciSeminarePredmetu", predmet);
                        }

                        bool odd = true;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            if (predmet.VedeSeminar.ContainsKey(p[i].Key))
                            {
                                continue;
                            }
                            else
                            {
                                actualId.Add(p[i].Key);
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
                                            Console.Write("\t\t{0,-5} {1,-30}", vyucujici[actualId[j]].Id, vyucujici[actualId[j]].Jmeno + " " + vyucujici[actualId[j]].Prijmeni);
                                            odd = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", vyucujici[actualId[j]].Id, vyucujici[actualId[j]].Jmeno + " " + vyucujici[actualId[j]].Prijmeni);
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        if (!odd)
                                        {
                                            Console.WriteLine();
                                        }
                                        Console.Write("\r\n\tZadejte id vedoucího semináře, kterého chcete přidat, nebo enter posun/ukončení: ");
                                        input = Console.ReadLine();
                                        ok = int.TryParse(input, out item);
                                        if (ok) ok = actualId.Contains(item);
                                        if (!ok) ok = input == String.Empty;

                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (vyucujici.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnAddItem(vyucujici[item]);
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
                                    printHeader(predmet);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 2: přidávání)
                    Request("Predmet", "VedouciSeminarePredmetu", predmet);
                    break;

                case 3: // Storno
                    Request("Predmet", "Default", null);
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


        private void printHeader(Models.Predmet predmet)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tSpráva vedoucích semináře předmětu: {0}", predmet);

            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Jméno                                     ID    Jméno");
            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
        }

        private bool MaVedouciSeminarRozvrhovouAkciPredmetu(Models.Vyucujici ucitel, Models.Predmet predmet)
        {
            foreach (KeyValuePair<Dny, Dictionary<int, Models.RozvrhovaAkce>> rozvrhDne in ucitel.Rozvrh)
            {
                foreach (KeyValuePair<int, Models.RozvrhovaAkce> akce in rozvrhDne.Value)
                {
                    if (akce.Value.TypVyuky == TypyVyuky.Seminář && akce.Value.Predmet == predmet)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}