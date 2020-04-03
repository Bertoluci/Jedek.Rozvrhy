using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Views.Student
{
    class PredmetyStudenta : View
    {

        public PredmetyStudenta(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            Models.Student student = (Models.Student)Context["student"]; // predmet => student

            Dictionary<int, Models.Predmet> predmetyOboru = student.StudijniObor.PredmetyOboru; 

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
                Console.WriteLine("\tSPRÁVA PŘEDMĚTŮ STUDENTA");
                Console.WriteLine("\r\n\tZvolte další postup:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: Výpis zapsaných předmětů");
                Console.WriteLine("\t\t2: Zápis předmětů");
                Console.WriteLine("\t\t3: Storno");

                Console.Write("\r\n\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 4);
            } while (!ok);  // volba editace

            switch (volba)
            {
                case 1: // Výpis zapsaných předmětů + odstaňování
                    do
                    {
                        printHeader(student);

                        var serazenePredmetyOboru = from pair in student.ZapsanePredmety
                                                orderby pair.Value.Id ascending
                                                select pair;

                        var po = serazenePredmetyOboru.ToList();
                        int count = po.Count;
                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tStudent zatím nemá zapsán žádný předmět.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat.");
                            Console.ReadKey();
                            Request("Student", "PredmetyStudenta", student);
                        }

                        bool odd = true;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            actualId.Add(po[i].Key);
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
                                            Console.Write("\t\t{0,-5} {1,-30}", predmetyOboru[actualId[j]].Id, predmetyOboru[actualId[j]].Nazev);
                                            odd = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", predmetyOboru[actualId[j]].Id, predmetyOboru[actualId[j]].Nazev);
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        Console.Write("\r\n\tZadejte id předmětu, který chcete odebrat, nebo enter posun/ukončení: ");
                                        input = Console.ReadLine();
                                        ok = int.TryParse(input, out item);
                                        if (ok) ok = actualId.Contains(item);
                                        if (!ok) ok = input == String.Empty;

                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (predmetyOboru.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(predmetyOboru[item]);
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
                                    printHeader(student);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 1: odebírání)
                    Request("Student", "PredmetyStudenta", student);
                    break;

                case 2: // Plnění předmětů vyučujícími
                    do
                    {
                        printHeader(student);
                        var po = predmetyOboru.ToList();
                        int count = po.Count;

                        if (count == 0)
                        {
                            Console.WriteLine("\r\n\t\tStudijní obor nemá dosud přiřazen žádný předmět.");
                            Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat.");
                            Console.ReadKey();
                            Request("Student", "PredmetyStudenta", student);
                        }

                        bool odd = true;
                        for (i = 0, itrBuffer = 0; i < count; i++)
                        {

                            if (student.ZapsanePredmety.ContainsKey(po[i].Key))
                            {
                                continue;
                            }
                            else
                            {
                                actualId.Add(po[i].Key);
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
                                            Console.Write("\t\t{0,-5} {1,-30}", predmetyOboru[actualId[j]].Id, predmetyOboru[actualId[j]].Nazev);
                                            odd = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t{0,-5} {1,-30}", predmetyOboru[actualId[j]].Id, predmetyOboru[actualId[j]].Nazev);
                                            odd = true;
                                        }
                                    }
                                    do
                                    {
                                        Console.Write("\r\n\tZadejte id předmětu, který chcete zapsat, nebo enter posun/ukončení: ");
                                        input = Console.ReadLine();
                                        ok = int.TryParse(input, out item);
                                        if (ok) ok = actualId.Contains(item);
                                        if (!ok) ok = input == String.Empty;

                                    } while (!ok);

                                    if (input != String.Empty)
                                    {
                                        if (predmetyOboru.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnAddItem(predmetyOboru[item]);
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
                                    printHeader(student);
                                    itrBuffer = 0;
                                } while (input != String.Empty && !emptyBuffer);
                            }

                        }

                    } while (input != String.Empty && !emptyBuffer); // (case 2: přidávání)
                    Request("Student", "PredmetyStudenta", student);
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


        private void printHeader(Models.Student student)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tSpráva zapsaných předmětů studenta: {0} {1}", student.Jmeno, student.Prijmeni);

            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Název                                     ID    Název");
            Console.WriteLine("\t   -------------------------------------------------------------------------------------------------");
        }

    }
}

