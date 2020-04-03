using System;
using System.Collections.Generic;
using System.Linq;

using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Forms;

namespace Jedek.Rozvrhy.App.Views.StudijniObor
{
    class VypisObory : View
    {

        public VypisObory(Dictionary<string, Object> context)
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
            int bufferSize = 10;

            do // volba řazení
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\tVÝPIS STUDIJNÍCH OBORŮ");
                Console.WriteLine("\r\n\tVyberte způsob řazení:");
                Console.WriteLine();
                Console.WriteLine("\t\t1: dle ID");
                Console.WriteLine("\t\t2: dle názvu");
                Console.WriteLine("\t\t3: dle zkratky");

                Console.Write("\r\n\tVaše volba: ");
                key = Console.ReadKey();
                ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 4);
            } while (!ok);


            Dictionary<int, Models.StudijniObor> obory = (Dictionary<int, Models.StudijniObor>)Context["obory"];


            var serazeneObory = from pair in obory
                                orderby pair.Value.Id ascending
                                select pair;
            switch (volba)
            {
                case 1:
                    break;
                case 2:
                    serazeneObory = from pair in obory
                                    orderby pair.Value.Nazev ascending
                                    select pair;
                    break;
                case 3:
                    serazeneObory = from pair in obory
                                    orderby pair.Value.Zkratka ascending
                                    select pair;
                    break;
            }

            // seznam aktuálních id výpisu
            List<int> actualId = new List<int>();

            do
            {
                printHeader();

                var o = serazeneObory.ToList();
                int count = o.Count;
                if (count == 0)
                {
                    Console.WriteLine("\r\n\t\tSystém zatím neobsahuje žádné obory.");
                    Console.Write("\r\n\t\tStiskněte libovolnou klávesu pro návrat. ");
                    Console.ReadKey();
                    Request("StudijniObor", "Default", null);
                }

                for (i = 0, itrBuffer = 0; i < count; i++)
                {

                    actualId.Add(o[i].Key);
                    itrBuffer++;

                    if (itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1)
                    {
                        do
                        {
                            emptyBuffer = false;
                            for (int j = 0; j < actualId.Count; j++)
                            {
                                Console.WriteLine("\t\t{0,-5} {1,-10} {2,-20}", obory[actualId[j]].Id, obory[actualId[j]].Zkratka, obory[actualId[j]].Nazev);
                            }

                            input = String.Empty;
                            item = 0;
                            ok = false;
                            do
                            {
                                if (Uzivatel.Role == Role.admin)
                                {
                                    Console.Write("\r\n\tPro správu studijního oboru zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }
                                else
                                {
                                    Console.Write("\r\n\tPro detail studijního oboru zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                }

                                ok = int.TryParse(input, out item);
                                if (ok) ok = actualId.Contains(item);
                                if (!ok) ok = input == String.Empty;

                            } while (!ok);

                            // bylo vybráno id oboru
                            if (input != String.Empty)
                            {
                                if (Uzivatel.Role == Role.admin) // začátek pro admina
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("\t1: editace studijního oboru");
                                    if (MaOborStudenty(obory[item]))
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                    Console.WriteLine("\t2: odstranění studijního oboru");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine("\t3: správa předmětů studijního oboru");
                                    Console.WriteLine("\t4: detail studijního oboru");
                                    Console.WriteLine("\t5: storno (pokračovat ve výpisu)");

                                    volba = 0;
                                    ok = false;
                                    do // volba editace / výmaz
                                    {
                                        Console.Write("\r\n\tVaše volba: ");
                                        key = Console.ReadKey();
                                        ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 6);
                                        if (MaOborStudenty(obory[item]) && volba == 2)
                                        {
                                            ok = false;
                                        }
                                    } while (!ok);

                                } // konec pro admina ostatním nastavit volba = 4 (ostatní v předešlém kroku vybírají id oboru přímo pro detail, tedy volbu 4)

                                if (Uzivatel.Role != Role.admin)
                                {
                                    volba = 4;
                                }

                                // zpacování volby editace / odstranění / storno
                                switch (volba)
                                {
                                    case 1: // editace
                                        this.OnEditItem(new EditStudijniOborForm(obory[item]));
                                        break;

                                    case 2: // odstranění
                                        if (obory.ContainsKey(item) && actualId.Contains(item))
                                        {
                                            OnDeleteItem(obory[item]);
                                            actualId.Remove(item);
                                            if (actualId.Count == 0)
                                            {
                                                emptyBuffer = true;
                                            }
                                        }
                                        break;

                                    case 3: // předměty
                                        Request("StudijniObor", "PredmetyOboru", obory[item]);
                                        break;

                                    case 4: // detail
                                        Console.Clear();
                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.WriteLine("\r\n\t{0}", obory[item]);
                                        Console.ForegroundColor = ConsoleColor.Black;

                                        int pocetStudujicich = 0;
                                        if (obory[item].PredmetyOboru.Count > 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine("\r\n\tPředměty oboru:\r\n");
                                            Console.ForegroundColor = ConsoleColor.Black;

                                            // dva sloupce
                                            bool odd = true;
                                            foreach (var pr in obory[item].PredmetyOboru)
                                            {
                                                if (odd)
                                                {
                                                    Console.Write("\t\t{0, -5}\t{1, -30}", pr.Value.Zkratka, pr.Value.Nazev);
                                                    pocetStudujicich += pr.Value.Studujici.Count;
                                                    odd = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\t\t{0, -5}\t{1, -30}", pr.Value.Zkratka, pr.Value.Nazev);
                                                    pocetStudujicich += pr.Value.Studujici.Count;
                                                    odd = true;
                                                }
                                            }
                                            // nedošlo k odřádkování
                                            if (odd == false)
                                            {
                                                Console.WriteLine();
                                            }

                                            // inline
                                            //foreach (var pr in obory[item].PredmetyOboru)
                                            //{
                                            //    Console.WriteLine("\t\t{0}\t{1}", pr.Value.Zkratka, pr.Value.Nazev);
                                            //    pocetStudujicich += pr.Value.Studujici.Count;
                                            //}
                                        }

                                        Console.WriteLine();
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\r\n\tPočet studujících: ");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("{0}", pocetStudujicich);

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
            Console.WriteLine("\r\n\tOdstaněn záznam: {0}", (Models.StudijniObor)item);
            Console.Write("\r\n\t\tStiskněte libovolnou klávesu. ");
            Console.ReadKey();
        }

        private void printHeader()
        {
            Console.Clear();
            Console.WriteLine("\t   -------------------------------------------------");
            Console.WriteLine("\t\tID    Zkratka    Název");
            Console.WriteLine("\t   -------------------------------------------------");
        }


        private bool MaOborStudenty(Models.StudijniObor obor)
        {
            foreach (KeyValuePair<int, Models.Predmet> predmet in obor.PredmetyOboru)
            {
                foreach (KeyValuePair<int, Models.Student> student in predmet.Value.Studujici)
                {
                    if (student.Value.StudijniObor == obor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}