using System;
using System.Collections.Generic;
using System.Linq;

using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class RozvrhovaAkceForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        //private int id = 0;
        private Predmet predmet = null;
        private TypyVyuky typVyuky = 0;
        private Vyucujici vyucujici = null;
        private Mistnost mistnost = null;
        private Dny den = 0;
        private int zacatek = 0;
        private int delka = 0;

        private bool exit = false;
        private string exitMessage = String.Empty;

        private Dictionary<int, StudijniSkupina> studijniSkupiny;

        public PredmetManager PredmetManager { get; private set; }

        public VyucujiciManager VyucujiciManager { get; private set; }

        public MistnostManager MistnostManager { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pm"></param>
        /// <param name="vm"></param>
        /// <param name="mm"></param>
        public RozvrhovaAkceForm(PredmetManager pm, VyucujiciManager vm, MistnostManager mm)
        {
            PredmetManager = pm;
            VyucujiciManager = vm;
            MistnostManager = mm;
            studijniSkupiny = new Dictionary<int, StudijniSkupina>();
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            if (!PredmetInput())
            {
                PrintPreruseniInfo();
                return;
            }

            TypVyukyInput();

            if (!VyucujiciInput())
            {
                PrintPreruseniInfo();
                return;
            }

            if (!MistnostInput())
            {
                PrintPreruseniInfo();
                return;
            }

            if (!DenInput())
            {
                PrintPreruseniInfo();
                return;
            }

            HodinaInput();

            if (!SkupinyInput())
            {
                PrintPreruseniInfo();
                return;
            }
            else
            {
                if (studijniSkupiny.Count == 0)
                {
                    exit = true;
                    exitMessage = "Nebyla vybrána žádná studijní skupina.";
                    PrintPreruseniInfo();
                    return;
                }
            }

            PrintHeader(true);
            Console.WriteLine();
            Console.WriteLine("\t\tPokud chcete zrušit uložení rozvrhové akce stiskněte klávesu Delete");
            Console.WriteLine("\t\tPro uložení stisněte cokoli jiného");
            if (Console.ReadKey().Key == ConsoleKey.Delete)
            {
                exit = true;
                return;
            }
        }


        /// <summary>
        /// Vypíše zprávu o přerušení tvorby rozvrhové akce
        /// </summary>
        private void PrintPreruseniInfo()
        {
            Console.Clear();
            Console.WriteLine("\r\n\t\tTvorba rozvrhové akce byla zrušena.");
            if (exitMessage != String.Empty)
            {
                Console.WriteLine("\r\n\t\t{0}", exitMessage);
            }
            Console.WriteLine("\r\n\t\tStiskněte libovolnou klávesu.");
            Console.ReadKey();
        }


        /// <summary>
        /// Překresluje hlavičku v závislosti na počtu vyplněných údajů
        /// </summary>
        /// <param name="souhrn"></param>
        private void PrintHeader(bool souhrn = false)
        {
            Console.Clear();
            Console.WriteLine();
            if (souhrn) Console.WriteLine("\n\tSUHRN ÚDAJŮ NOVÉ ROZVRHOVÉ AKCE:");
            if (!souhrn) Console.WriteLine("\n\tVYTVOŘENÍ NOVÉ ROZVRHOVÉ AKCE:");
            Console.WriteLine();
            // předmět
            if (predmet != null)
            {
                Console.WriteLine("\t1) Předmět: {0}", predmet);
            }
            else
            {
                Console.WriteLine("\r\n\t1) Volba předmětu:");
            }
            // typ výuky
            if (typVyuky != 0)
            {
                Console.WriteLine("\t2) Typ výuky: {0}", typVyuky);
            }
            else if (predmet != null)
            {
                Console.WriteLine("\r\n\t2) Volba typu výuky:");
            }
            // vyučující
            if (vyucujici != null)
            {
                Console.WriteLine("\t3) Vyucujici: {0}", vyucujici);
            }
            else if (typVyuky != 0)
            {
                Console.WriteLine("\r\n\t3) Volba vyučujícího:");
            }
            // místnost
            if (mistnost != null)
            {
                Console.WriteLine("\t4) Místnost: {0}", mistnost);
            }
            else if (vyucujici != null)
            {
                Console.WriteLine("\r\n\t4) Volba místnosti:");
            }
            // den
            if (den != 0)
            {
                Console.WriteLine("\t5) Den: {0}", den);
            }
            else if (mistnost != null)
            {
                Console.WriteLine("\r\n\t5) Volba dne výuky:");
                Console.WriteLine("\r\n\t   Přehled obsazenosti: (m = obsazená místnost, v = zaneprázdněný vyučující)\r\n");
            }
            // začátek
            if (zacatek != 0)
            {
                Console.WriteLine("\t6) Začátek: {0}:00", zacatek);
            }
            else if (den != 0)
            {
                Console.WriteLine("\r\n\t6) Volba začátku výuky:");
                Console.WriteLine("\r\n\t   Přehled obsazenosti: (m = obsazená místnost, v = zaneprázdněný vyučující)\r\n");
            }
            // studijní skupiny
            if (studijniSkupiny.Count > 0)
            {
                Console.WriteLine("\t7) Studijní skupiny rozvrhové akce:");
                foreach (KeyValuePair<int, StudijniSkupina> skupina in studijniSkupiny)
                {
                    Console.WriteLine("\t\tid: {0}, početStudentů: {1}", skupina.Value.Id, skupina.Value.StudentiSkupiny.Count);
                }
                if (!souhrn) Console.WriteLine("\r\n\tPřidání studijní skupiny:\n\r");
            }
            else if (zacatek != 0)
            {
                Console.WriteLine("\r\n\t7) Výběr studijních skupin:\n\r");
            }
        }


        /// <summary>
        /// Hlavička výběru předmětu
        /// </summary>
        private void printPredmetHeader()
        {
            Console.WriteLine("\t   ------------------------------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Zkratka    Název                             Přednášek  Cvičení   Seminářů");
            Console.WriteLine("\t   ------------------------------------------------------------------------------------------");
        }


        /// <summary>
        /// Výběr předmětu
        /// </summary>
        /// <returns></returns>
        private bool PredmetInput()
        {
            while (predmet == null)
            {
                string input = String.Empty;
                int i = 0;
                int itrBuffer = 0;
                int item = 0;
                bool ok = false;
                bool emptyBuffer = false;
                int bufferSize = 15;

                Dictionary<int, Models.Predmet> predmety = PredmetManager.Predmety;

                var serazenePredmety = from pair in predmety
                                       orderby pair.Value.Id ascending
                                       select pair;


                // seznam aktuálních id výpisu
                List<int> actualId = new List<int>();


                do
                {
                    PrintHeader();
                    printPredmetHeader();

                    var p = serazenePredmety.ToList();
                    int count = p.Count;
                    if (count == 0)
                    {
                        exit = true;
                        exitMessage = "Neexistuje žádný předmět.";
                        return false;
                    }

                    // odstranění předmětů bez studijních skupin a se splněnými rozvrhy pro všechny typy výuky
                    for (i = 0; i < count; i++)
                    {
                        if (p[i].Value.StudijniSkupiny.Count == 0 || !MaPredmetSkupinuBezRozvrhoveAkce(p[i].Value))
                        {
                            p.Remove(p[i]);
                            count--;
                            i--;
                        }
                    }

                    if (count == 0)
                    {
                        exit = true;
                        exitMessage = "Neexistuje žádný předmět se studijní skupinou.";
                        return false;
                    }

                    for (i = 0, itrBuffer = 0; i < count; i++)
                    {
                        actualId.Add(p[i].Key);
                        itrBuffer++;

                        if ((itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1) && predmet == null)
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
                                    Console.Write("\r\n\tPro výběr předmětu zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                    ok = int.TryParse(input, out item);
                                    if (ok) ok = actualId.Contains(item);
                                    if (!ok) ok = input == String.Empty;

                                } while (!ok);

                                // bylo vybráno id předmětu
                                if (input != String.Empty)
                                {
                                    predmet = predmety[item];
                                    return true;
                                }

                                if (input == String.Empty)
                                {
                                    actualId.RemoveRange(0, actualId.Count);
                                }

                                if (predmet == null)
                                {
                                    PrintHeader();
                                    printPredmetHeader();
                                }

                                itrBuffer = 0;
                            } while ((input != String.Empty && !emptyBuffer) && predmet == null); // smyčka načtených
                        }

                    }

                } while ((input != String.Empty && !emptyBuffer) && predmet == null); // hlavní smyčka

                // volba znovuvýběru, nebo storno celé RA
                if (predmet == null)
                {
                    Console.WriteLine("\r\n\t\tUž byly nabídnuty všechny předměty.");
                    Console.WriteLine("\t\tChcete pokračovat ve výběru od začátku? [Enter = Ano]");
                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        exit = true;
                        exitMessage = "Nedošlo k výběru požadovaného předmětu.";
                        return false;
                    }
                }

            } // dokud není vybráno
            return true;
        }


        /// <summary>
        /// Výběr typu výuky
        /// </summary>
        private void TypVyukyInput()
        {
            PrintHeader();

            bool ok = false;
            string input = String.Empty;

            List<string> validniTypy = new List<string>();
            if (predmet.HodinPrednasek > 0 && MaTypVyukySkupinuBezRozvrhoveAkce(predmet, TypyVyuky.Přednáška, predmet.HodinPrednasek))
            {
                validniTypy.Add("Přednáška");
            }
            if (predmet.HodinSeminaru > 0 && MaTypVyukySkupinuBezRozvrhoveAkce(predmet, TypyVyuky.Seminář, predmet.HodinSeminaru))
            {
                validniTypy.Add("Seminář");
            }
            if (predmet.HodinCviceni > 0 && MaTypVyukySkupinuBezRozvrhoveAkce(predmet, TypyVyuky.Cvičení, predmet.HodinCviceni))
            {
                validniTypy.Add("Cvičení");
            }


            do // typ - Přednáška = 1, Seminář, Cvičení TODO neuvádět co předmět nemá
            {
                Console.Write("\t\tTyp výuky [");
                Console.Write(String.Join(", ", validniTypy));
                Console.Write("]: ");
                if(validniTypy.Count == 1)
                {
                    input = validniTypy[0];
                }
                else
                {
                    input = Console.ReadLine();
                }
                
                ok = Enum.IsDefined(typeof(TypyVyuky), input);
                ok = validniTypy.Contains(input);
            } while (!ok);
            bool parse = Enum.TryParse(input, out typVyuky);
        }


        /// <summary>
        /// Hlavička pro výběr vyučujícího
        /// </summary>
        private void printVyucujiciHeader()
        {
            Console.WriteLine("\t   ------------------------------------------------------------------");
            Console.WriteLine("\t\tID    Jméno           Příjmení             Tituly");
            Console.WriteLine("\t   ------------------------------------------------------------------");
        }


        /// <summary>
        /// Výběr vyučujícího
        /// </summary>
        /// <returns></returns>
        private bool VyucujiciInput()
        {
            PrintHeader();
            int volba = 0;
            ConsoleKeyInfo key;
            bool ok = false;
            int quantity = 0;
            bool newVyucujici = false;
            Dictionary<int, Models.Vyucujici> vyucujiciTypuVyuky = new Dictionary<int, Vyucujici>();

            if (typVyuky == TypyVyuky.Přednáška)
            {
                // přednášející
                quantity = predmet.Prednasejici.Count;
                if (quantity > 0)
                {
                    foreach (KeyValuePair<int, Vyucujici> ucitel in predmet.Prednasejici)
                    {
                        vyucujiciTypuVyuky.Add(ucitel.Key, ucitel.Value);
                    }
                }
            }
            else if (typVyuky == TypyVyuky.Seminář)
            {
                // vede seminář
                quantity = predmet.VedeSeminar.Count;
                if (quantity > 0)
                {
                    foreach (KeyValuePair<int, Vyucujici> ucitel in predmet.VedeSeminar)
                    {
                        vyucujiciTypuVyuky.Add(ucitel.Key, ucitel.Value);
                    }
                }
            }
            else
            {
                // cvičící
                quantity = predmet.Cvicici.Count;
                if (quantity > 0)
                {
                    foreach (KeyValuePair<int, Vyucujici> ucitel in predmet.Cvicici)
                    {
                        vyucujiciTypuVyuky.Add(ucitel.Key, ucitel.Value);
                    }
                }
            }

            /***************************************************** není evidován žádný vyučující */
            if (quantity == 0)
            {
                Console.WriteLine();
                Console.WriteLine("\t{0} předmětu zatím nemá evidovaného žádného vyučujícího, zvolte další postup:", typVyuky);
                Console.WriteLine("\t1: přiřadit vyučujícího nyní");
                Console.WriteLine("\t2: storno (ukončit tvorbu rozvrhové akce)");

                volba = 0;
                ok = false;
                do // volba přiřazení / storno
                {
                    Console.Write("\r\n\tVaše volba: ");
                    key = Console.ReadKey();
                    ok = (int.TryParse(key.KeyChar.ToString(), out volba)) && (volba > 0 && volba < 3);
                } while (!ok);

                // zpacování volby přidat / storno
                switch (volba)
                {
                    case 1: // přidat
                        vyucujiciTypuVyuky = VyucujiciManager.Vyucujici;
                        newVyucujici = true;
                        break;

                    case 2: // storno
                        exitMessage = "Není přiřazen žádný vyučující, přidělení odmítnuto.";
                        exit = true;
                        return false;

                }
            }
            /***************************************************** není evidován žádný vyučující **/

            // výběr
            while (vyucujici == null)
            {
                string input = String.Empty;
                int i = 0;
                int itrBuffer = 0;
                int item = 0;
                ok = false;
                bool emptyBuffer = false;
                int bufferSize = 15;



                var serazeniVyucujici = from pair in vyucujiciTypuVyuky
                                        orderby pair.Value.Prijmeni ascending
                                        select pair;


                // seznam aktuálních id výpisu
                List<int> actualId = new List<int>();


                do
                {
                    PrintHeader();
                    printVyucujiciHeader();

                    var v = serazeniVyucujici.ToList();
                    int count = v.Count;

                    for (i = 0, itrBuffer = 0; i < count; i++)
                    {

                        actualId.Add(v[i].Key);
                        itrBuffer++;

                        if ((itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1) && vyucujici == null)
                        {
                            do
                            {
                                emptyBuffer = false;
                                for (int j = 0; j < actualId.Count; j++)
                                {
                                    Console.WriteLine("\t\t{0,-5} {1,-15} {2,-20} {3,-15}"
                                        , vyucujiciTypuVyuky[actualId[j]].Id, vyucujiciTypuVyuky[actualId[j]].Jmeno, vyucujiciTypuVyuky[actualId[j]].Prijmeni, vyucujiciTypuVyuky[actualId[j]].Tituly);
                                }

                                input = String.Empty;
                                item = 0;
                                ok = false;
                                do
                                {
                                    Console.Write("\r\n\tPro výběr vyučujícího zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                    ok = int.TryParse(input, out item);
                                    if (ok) ok = actualId.Contains(item);
                                    if (!ok) ok = input == String.Empty;

                                } while (!ok);

                                // bylo vybráno id vyučujícího
                                if (input != String.Empty)
                                {
                                    vyucujici = vyucujiciTypuVyuky[item];
                                    if (newVyucujici == true)
                                    {
                                        if (typVyuky == TypyVyuky.Přednáška)
                                        {
                                            PredmetManager.AddPrednasejiciho(predmet, vyucujici);
                                        }
                                        else if (typVyuky == TypyVyuky.Seminář)
                                        {
                                            PredmetManager.AddVedeSeminar(predmet, vyucujici);
                                        }
                                        else
                                        {
                                            PredmetManager.AddCviciciho(predmet, vyucujici);
                                        }
                                    }
                                    return true;
                                }

                                if (input == String.Empty)
                                {
                                    actualId.RemoveRange(0, actualId.Count);
                                }

                                if (vyucujici == null)
                                {
                                    PrintHeader();
                                    printVyucujiciHeader();
                                }

                                itrBuffer = 0;
                            } while ((input != String.Empty && !emptyBuffer) && vyucujici == null); // smyčka načtených
                        }

                    }

                } while ((input != String.Empty && !emptyBuffer) && vyucujici == null); // hlavní smyčka

                // volba znovuvýběru, nebo storno celé RA
                if (vyucujici == null)
                {
                    Console.WriteLine("\r\n\t\tUž byly nabídnuti všichni vyučující.");
                    Console.WriteLine("\t\tChcete pokračovat ve výběru od začátku? [Enter = Ano]");
                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        exit = true;
                        exitMessage = "Nedošlo k výběru vyučujícího.";
                        return false;
                    }
                }

            } // dokud není vybráno (vyučující)
            return true;
        }


        /// <summary>
        /// Hlavička pro výběr místnosti
        /// </summary>
        private void printMistnostHeader()
        {
            Console.WriteLine("\t   ----------------------------------------------------------");
            Console.WriteLine("\t\tID    Kapacita     Budova     Číslo místnosti");
            Console.WriteLine("\t   ----------------------------------------------------------");
        }


        /// <summary>
        /// Výběr místnosti
        /// </summary>
        /// <returns></returns>
        private bool MistnostInput()
        {
            while (mistnost == null)
            {
                string input = String.Empty;
                int i = 0;
                int itrBuffer = 0;
                int item = 0;
                bool ok = false;
                bool emptyBuffer = false;
                int bufferSize = 15;

                Dictionary<int, Models.Mistnost> mistnosti = MistnostManager.Mistnosti;

                var predSerazeneMistnosti = from pair in mistnosti
                                            orderby pair.Value.Budova ascending
                                            select pair;

                var serazeneMistnosti = from pair in mistnosti
                                        orderby pair.Value.Cislo ascending
                                        select pair;


                // seznam aktuálních id výpisu
                List<int> actualId = new List<int>();


                do
                {
                    PrintHeader();
                    printMistnostHeader();

                    var m = serazeneMistnosti.ToList();
                    int count = m.Count;

                    for (i = 0, itrBuffer = 0; i < count; i++)
                    {

                        actualId.Add(m[i].Key);
                        itrBuffer++;

                        if ((itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1) && mistnost == null)
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
                                    Console.Write("\r\n\tPro výběr místnosti zadejte jeho ID, nebo stiskněte enter pro posun výpisu: ");
                                    input = Console.ReadLine();
                                    ok = int.TryParse(input, out item);
                                    if (ok) ok = actualId.Contains(item);
                                    if (!ok) ok = input == String.Empty;

                                } while (!ok);

                                // bylo vybráno id předmětu
                                if (input != String.Empty)
                                {
                                    mistnost = mistnosti[item];
                                    return true;
                                }

                                if (input == String.Empty)
                                {
                                    actualId.RemoveRange(0, actualId.Count);
                                }

                                if (mistnost == null)
                                {
                                    PrintHeader();
                                    printMistnostHeader();
                                }

                                itrBuffer = 0;
                            } while ((input != String.Empty && !emptyBuffer) && mistnost == null); // smyčka načtených
                        }

                    }

                } while ((input != String.Empty && !emptyBuffer) && mistnost == null); // hlavní smyčka


                // volba znovuvýběru, nebo storno celé RA
                if (mistnost == null)
                {
                    Console.WriteLine("\r\n\t\tUž byly nabídnuty všechny místnosti.");
                    Console.WriteLine("\t\tChcete pokračovat ve výběru od začátku? [Enter = Ano]");
                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        exit = true;
                        exitMessage = "Nedošlo k výběru místnosti.";
                        return false;
                    }
                }

            } // dokud není vybráno
            return true;
        }


        /// <summary>
        /// Výběr dne
        /// </summary>
        /// <returns></returns>
        private bool DenInput()
        {
            PrintHeader();
            printRozvrh();

            int potrebnaDelka = GetPotrebnouDelkuVyuky();

            bool ok = false;
            string input = String.Empty;
            string mozneDny = String.Empty;
            if (MaPotrebnouDelku(Dny.Po, potrebnaDelka))
            {
                mozneDny += ("Po");
            }
            if (MaPotrebnouDelku(Dny.Út, potrebnaDelka))
            {
                mozneDny += mozneDny == String.Empty ? "Út" : " , Út";
            }
            if (MaPotrebnouDelku(Dny.St, potrebnaDelka))
            {
                mozneDny += mozneDny == String.Empty ? "St" : " , St";
            }
            if (MaPotrebnouDelku(Dny.Čt, potrebnaDelka))
            {
                mozneDny += mozneDny == String.Empty ? "Čt" : " , Čt";
            }
            if (MaPotrebnouDelku(Dny.Pá, potrebnaDelka))
            {
                mozneDny += mozneDny == String.Empty ? "Pá" : " , Pá";
            }

            if (mozneDny != String.Empty)
            {
                do // den
                {
                    Console.Write("\r\n\tVyberte den [");
                    Console.Write(mozneDny);
                    Console.Write("]: ");

                    input = Console.ReadLine();
                    ok = Enum.IsDefined(typeof(Dny), input);
                    if (input == "Po" && !MaPotrebnouDelku(Dny.Po, potrebnaDelka)) ok = false;
                    if (input == "Út" && !MaPotrebnouDelku(Dny.Út, potrebnaDelka)) ok = false;
                    if (input == "St" && !MaPotrebnouDelku(Dny.St, potrebnaDelka)) ok = false;
                    if (input == "Čt" && !MaPotrebnouDelku(Dny.Čt, potrebnaDelka)) ok = false;
                    if (input == "Pá" && !MaPotrebnouDelku(Dny.Pá, potrebnaDelka)) ok = false;
                } while (!ok);
                bool parse = Enum.TryParse(input, out den);
                return parse;
            }
            else
            {
                exitMessage = "V této místnosti již pro danou rozvrhovou akci není prostor.";
                exit = true;
                return false;
            }
        }


        /// <summary>
        /// Vykreslí rozvrh s informací o obsazenosti místnosti a zaneprázdněnosti vyučujícího
        /// </summary>
        private void printRozvrh()
        {

            Console.WriteLine("\t{0,-5} {1,-5} {2,-5} {3,-5} {4,-5} {5,-5} {6,-5} {7,-5} {8,-5} {9,-5} {10,-5} {11,-5} {12,-5} {13,-5} {14,-5} {15,-5} "
                , "", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00");

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Po, GetObsazenost(Dny.Po, 7), GetObsazenost(Dny.Po, 8), GetObsazenost(Dny.Po, 9), GetObsazenost(Dny.Po, 10), GetObsazenost(Dny.Po, 11)
                , GetObsazenost(Dny.Po, 12), GetObsazenost(Dny.Po, 13), GetObsazenost(Dny.Po, 14), GetObsazenost(Dny.Po, 15), GetObsazenost(Dny.Po, 16)
                , GetObsazenost(Dny.Po, 17), GetObsazenost(Dny.Po, 18), GetObsazenost(Dny.Po, 19), GetObsazenost(Dny.Po, 20), GetObsazenost(Dny.Po, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Út, GetObsazenost(Dny.Út, 7), GetObsazenost(Dny.Út, 8), GetObsazenost(Dny.Út, 9), GetObsazenost(Dny.Út, 10), GetObsazenost(Dny.Út, 11)
                , GetObsazenost(Dny.Út, 12), GetObsazenost(Dny.Út, 13), GetObsazenost(Dny.Út, 14), GetObsazenost(Dny.Út, 15), GetObsazenost(Dny.Út, 16)
                , GetObsazenost(Dny.Út, 17), GetObsazenost(Dny.Út, 18), GetObsazenost(Dny.Út, 19), GetObsazenost(Dny.Út, 20), GetObsazenost(Dny.Út, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.St, GetObsazenost(Dny.St, 7), GetObsazenost(Dny.St, 8), GetObsazenost(Dny.St, 9), GetObsazenost(Dny.St, 10), GetObsazenost(Dny.St, 11)
                , GetObsazenost(Dny.St, 12), GetObsazenost(Dny.St, 13), GetObsazenost(Dny.St, 14), GetObsazenost(Dny.St, 15), GetObsazenost(Dny.St, 16)
                , GetObsazenost(Dny.St, 17), GetObsazenost(Dny.St, 18), GetObsazenost(Dny.St, 19), GetObsazenost(Dny.St, 20), GetObsazenost(Dny.St, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Čt, GetObsazenost(Dny.Čt, 7), GetObsazenost(Dny.Čt, 8), GetObsazenost(Dny.Čt, 9), GetObsazenost(Dny.Čt, 10), GetObsazenost(Dny.Čt, 11)
                , GetObsazenost(Dny.Čt, 12), GetObsazenost(Dny.Čt, 13), GetObsazenost(Dny.Čt, 14), GetObsazenost(Dny.Čt, 15), GetObsazenost(Dny.Čt, 16)
                , GetObsazenost(Dny.Čt, 17), GetObsazenost(Dny.Čt, 18), GetObsazenost(Dny.Čt, 19), GetObsazenost(Dny.Čt, 20), GetObsazenost(Dny.Čt, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , Dny.Pá, GetObsazenost(Dny.Pá, 7), GetObsazenost(Dny.Pá, 8), GetObsazenost(Dny.Pá, 9), GetObsazenost(Dny.Pá, 10), GetObsazenost(Dny.Pá, 11)
                , GetObsazenost(Dny.Pá, 12), GetObsazenost(Dny.Pá, 13), GetObsazenost(Dny.Pá, 14), GetObsazenost(Dny.Pá, 15), GetObsazenost(Dny.Pá, 16)
                , GetObsazenost(Dny.Pá, 17), GetObsazenost(Dny.Pá, 18), GetObsazenost(Dny.Pá, 19), GetObsazenost(Dny.Pá, 20), GetObsazenost(Dny.Pá, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");
        }


        /// <summary>
        /// Vrací textovou podobu obsazenosti m = osazená místnost, v = zaneprázdněný vyučující, mv = obojí
        /// </summary>
        /// <param name="den"></param>
        /// <param name="hodina"></param>
        /// <returns></returns>
        private string GetObsazenost(Dny den, int hodina)
        {
            string obsazenost = mistnost.Rozvrh[den].ContainsKey(hodina) ? " m" : "";
            obsazenost += vyucujici.Rozvrh[den].ContainsKey(hodina) ? " v" : "";
            return obsazenost;
        }


        /// <summary>
        /// zjistí, zda je volná místnost, a zároveň může vyučující
        /// </summary>
        /// <param name="den"></param>
        /// <param name="hodina"></param>
        /// <returns></returns>
        private bool JeMoznaVyuka(Dny den, int hodina)
        {
            bool volno = mistnost.Rozvrh[den].ContainsKey(hodina) ? false : true;
            if (volno == false) return false;

            volno = vyucujici.Rozvrh[den].ContainsKey(hodina) ? false : true;
            return volno;
        }


        /// <summary>
        /// vrací délku výuky dle vybraného typu výuky
        /// </summary>
        /// <returns></returns>
        private int GetPotrebnouDelkuVyuky()
        {
            if (typVyuky == TypyVyuky.Přednáška)
            {
                return predmet.HodinPrednasek;
            }
            else if (typVyuky == TypyVyuky.Seminář)
            {
                return predmet.HodinSeminaru;
            }
            return predmet.HodinCviceni;
        }


        /// <summary>
        /// zjišťuje, jestli je v daný den potřebný po sobě jdoucí počet hodin pro výuku
        /// </summary>
        /// <param name="den"></param>
        /// <param name="delka"></param>
        /// <returns></returns>
        private bool MaPotrebnouDelku(Dny den, int delka)
        {
            int usek = 0;
            for (int i = 7; i < 21; i++)
            {
                if (JeMoznaVyuka(den, i))
                {
                    usek++;
                    if (usek >= delka) return true;
                }
                else
                {
                    usek = 0;
                }
            }
            return usek >= delka ? true : false;
        }


        /// <summary>
        /// výběr hodiny
        /// </summary>
        private void HodinaInput()
        {
            PrintHeader();
            printDenRozvrhu(den);
            int potrebnaDelka = GetPotrebnouDelkuVyuky();
            List<int> mozneZacatky = GetMozneZacatky(potrebnaDelka);

            string input = String.Empty;
            bool ok = false;
            int volba = 0;
            do // rocnik
            {
                Console.Write("\r\n\tZadejte začátek výuky [");
                Console.Write(String.Join(", ", mozneZacatky));
                Console.Write("]: ");
                input = Console.ReadLine();
                ok = int.TryParse(input, out volba);
                if (ok)
                {
                    ok = mozneZacatky.Contains(volba) ? true : false;
                }

            } while (!ok);
            zacatek = volba;
            delka = potrebnaDelka;
            // testovací
            //PrintHeader();
        }


        /// <summary>
        /// Vykreslí rozvrh dne s informací o obsazenosti místnosti a zaneprázdněnosti vyučujícího
        /// </summary>
        /// <param name="den"></param>
        private void printDenRozvrhu(Dny den)
        {

            Console.WriteLine("\t{0,-5} {1,-5} {2,-5} {3,-5} {4,-5} {5,-5} {6,-5} {7,-5} {8,-5} {9,-5} {10,-5} {11,-5} {12,-5} {13,-5} {14,-5} {15,-5} "
                , "", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00");

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");

            Console.WriteLine("\t{0,-5}:{1,-5}:{2,-5}:{3,-5}:{4,-5}:{5,-5}:{6,-5}:{7,-5}:{8,-5}:{9,-5}:{10,-5}:{11,-5}:{12,-5}:{13,-5}:{14,-5}:{15,-5}:"
                , den, GetObsazenost(den, 7), GetObsazenost(den, 8), GetObsazenost(den, 9), GetObsazenost(den, 10), GetObsazenost(den, 11)
                , GetObsazenost(den, 12), GetObsazenost(den, 13), GetObsazenost(den, 14), GetObsazenost(den, 15), GetObsazenost(den, 16)
                , GetObsazenost(den, 17), GetObsazenost(den, 18), GetObsazenost(den, 19), GetObsazenost(den, 20), GetObsazenost(den, 21));

            Console.WriteLine("\t{0,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} {1,-5} ", "", "- - -");
        }


        /// <summary>
        /// Vrátí seznam možných začátků výuky (ošetření délky výuky vs souvislý volný prostor)
        /// </summary>
        /// <param name="potrebnaDelka"></param>
        /// <returns></returns>
        private List<int> GetMozneZacatky(int potrebnaDelka)
        {
            List<int> mozneZacatky = new List<int>();

            bool inLine = false;

            for (int i = 7; i < 22; i++)
            {
                for (int j = 0; j < potrebnaDelka; j++)
                {
                    if ((i + j) > 21)
                    {
                        inLine = false;
                    }
                    else
                    {
                        inLine = JeMoznaVyuka(den, i + j);
                    }
                    if (inLine == false) j = potrebnaDelka;
                }
                if (inLine == true)
                {
                    mozneZacatky.Add(i);
                }
            }
            return mozneZacatky;
        }


        /// <summary>
        /// Hlavička výběru studijní skupiny
        /// </summary>
        /// <param name="neobsazeno"></param>
        private void printSkupinyHeader(int neobsazeno)
        {
            Console.WriteLine("\tAktuální kapacita místnosti: {0}", neobsazeno);
            Console.WriteLine("\t   -----------------------------------------------------------");
            Console.WriteLine("\t\tID     studentů     Předmět");
            Console.WriteLine("\t   -----------------------------------------------------------");
        }


        /// <summary>
        /// Výběr studijní skupiny
        /// </summary>
        /// <returns></returns>
        private bool SkupinyInput()
        {
            Dictionary<int, Models.StudijniSkupina> skupiny = predmet.StudijniSkupiny;
            bool konecVkladani = false;
            int pocetStudentuVPridanychSkupinach = 0;
            int neobsazeno = mistnost.Kapacita;
            int pocetNevolitelnychSkupin = 0;
            bool legenda = false;
            bool volitelnaSkupina = false;

            foreach (KeyValuePair<int, Models.StudijniSkupina> skupina in skupiny)
            {
                if (volitelnaSkupina == false) volitelnaSkupina = !MaSkupinaZaneprazdnenehoStudenta(skupina.Value.StudentiSkupiny);
            }

            if (volitelnaSkupina == false)
            {
                exit = true;
                exitMessage = "Ve všech studijních skupinách je na tento termín nějaký zaneprázdněný student.";
                return false;
            }

            while (konecVkladani == false)
            {
                string input = String.Empty;
                int i = 0;
                int itrBuffer = 0;
                int item = 0;
                bool ok = false;
                bool emptyBuffer = false;
                int bufferSize = 15;

                //var serazeniStudenti = studenti.OrderBy(s => s.Rocnik).ThenBy(s => s.StudijniObor.Id);

                var serazeneSkupiny = from pair in skupiny
                                      orderby pair.Value.Id ascending
                                      select pair;


                // seznam aktuálních id výpisu
                List<int> actualId = new List<int>();

                do
                {
                    PrintHeader();
                    printSkupinyHeader(neobsazeno);

                    var ss = serazeneSkupiny.ToList();
                    int count = ss.Count;

                    for (i = 0, itrBuffer = 0; i < count; i++)
                    {

                        if (studijniSkupiny.ContainsKey(ss[i].Key) || MaSkupinaRozvrhovouAkci(ss[i].Value.Rozvrh))
                        {
                            ss.Remove(ss[i]);
                            count--;
                            i--;
                        }
                        else
                        {
                            actualId.Add(ss[i].Key);
                            itrBuffer++;
                        }


                        if ((itrBuffer == bufferSize || (count < bufferSize && itrBuffer == count) || i == count - 1) && konecVkladani == false)
                        {
                            do
                            {
                                emptyBuffer = false;
                                for (int j = 0; j < actualId.Count; j++)
                                {
                                    if (MaSkupinaZaneprazdnenehoStudenta(skupiny[actualId[j]].StudentiSkupiny))
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        legenda = true;
                                        pocetNevolitelnychSkupin++;
                                    }
                                    else if (skupiny[actualId[j]].StudentiSkupiny.Count > neobsazeno)
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        legenda = true;
                                        pocetNevolitelnychSkupin++;
                                    }
                                    Console.WriteLine("\t\t{0,-6} {1,-12} {2,-20}"
                                    , skupiny[actualId[j]].Id, skupiny[actualId[j]].StudentiSkupiny.Count, skupiny[actualId[j]].Predmet.Nazev);

                                    Console.ForegroundColor = ConsoleColor.Black;
                                }
                                // opustit výběr, pokud už zbývají pouze nevybíratelné skupiny
                                //if (pocetNevolitelnychSkupin == ss.Count)
                                //{
                                //    Console.WriteLine("todo oznámení");
                                //    Console.ReadKey();
                                //    konecVkladani = true;
                                //    continue;
                                //}
                                pocetNevolitelnychSkupin = 0;

                                input = String.Empty;
                                item = 0;
                                ok = false;
                                do
                                {
                                    if (legenda == true)
                                    {
                                        Console.WriteLine("\t...................................................................................");
                                        Console.WriteLine("\tLegenda: ");
                                        Console.WriteLine("\tStudijní skupiny, které je možno přiřadit");
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.WriteLine("\tStudijní skupiny, ve kterých jsou studenti, kteří již mají jinou výuku");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("\tStudijní skupiny, s počtem studentů přesahující počet volných míst");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine("\t...................................................................................");
                                        legenda = false;
                                    }
                                    Console.Write("\r\n\tPro výběr Studijní skupiny zadejte její ID, nebo stiskněte enter pro posun výpisu: ");

                                    input = Console.ReadLine();
                                    ok = int.TryParse(input, out item);
                                    if (ok) ok = actualId.Contains(item);
                                    if (ok) ok = !MaSkupinaZaneprazdnenehoStudenta(skupiny[item].StudentiSkupiny);
                                    if (ok) ok = skupiny[item].StudentiSkupiny.Count <= neobsazeno;
                                    if (!ok) ok = input == String.Empty;

                                } while (!ok);

                                // bylo vybráno id skupiny
                                if (input != String.Empty)
                                {
                                    studijniSkupiny.Add(item, skupiny[item]);
                                    actualId.Remove(item);
                                    var vybranaSkupina = ss.Find(x => x.Value.Id == item);
                                    ss.Remove(vybranaSkupina);
                                    count--;
                                    i--;
                                    pocetStudentuVPridanychSkupinach += skupiny[item].StudentiSkupiny.Count;
                                    neobsazeno = mistnost.Kapacita - pocetStudentuVPridanychSkupinach;

                                    // opustit výběr, pokud již není skupina s počtem studentů, který mepřesahuje volná místa
                                    if ((pocetStudentuVPridanychSkupinach + GetMinimalniPocetStudentuVeSkupine(ss)) > mistnost.Kapacita || ss.Count == 0)
                                    {
                                        konecVkladani = true;
                                    }
                                }


                                if (input == String.Empty)
                                {
                                    actualId.RemoveRange(0, actualId.Count);
                                }

                                if (konecVkladani == false)
                                {
                                    PrintHeader();
                                    printSkupinyHeader(neobsazeno);
                                }

                                itrBuffer = 0;
                            } while ((input != String.Empty && !emptyBuffer) && konecVkladani == false); // smyčka načtených
                        }

                    }

                } while ((input != String.Empty && !emptyBuffer) && konecVkladani == false); // hlavní smyčka


                if (konecVkladani == false)
                {
                    Console.WriteLine("\r\n\t\tUž byly nabídnuty všechny studijní skupiny.");
                    Console.WriteLine("\t\tChcete pokračovat ve výběru od začátku? [Enter = Ano]");

                    if (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        konecVkladani = true;
                        return true;
                    }
                }

            } // dokud není vybráno
            return true;
        }


        /// <summary>
        /// Najde skupinu s nejmwnším počtem studentů a vrátí jejich počet
        /// </summary>
        /// <param name="skupiny"></param>
        /// <returns></returns>
        private int GetMinimalniPocetStudentuVeSkupine(List<KeyValuePair<int, StudijniSkupina>> skupiny)
        {
            int minPocetStudentuVeSkupine = 12;
            foreach (KeyValuePair<int, Models.StudijniSkupina> skupina in skupiny)
            {
                if (skupina.Value.StudentiSkupiny.Count < minPocetStudentuVeSkupine) minPocetStudentuVeSkupine = skupina.Value.StudentiSkupiny.Count;
            }
            return minPocetStudentuVeSkupine;
        }


        /// <summary>
        /// Ověří, má-li skupina nějakou rozvrhovou akci pro vybraný typ výuky
        /// </summary>
        /// <param name="rozvrh"></param>
        /// <returns></returns>
        private bool MaSkupinaRozvrhovouAkci(Dictionary<Dny, Dictionary<int, RozvrhovaAkce>> rozvrh)
        {
            foreach (KeyValuePair<Dny, Dictionary<int, RozvrhovaAkce>> rozvrhDne in rozvrh)
            {
                foreach (KeyValuePair<int, RozvrhovaAkce> akce in rozvrhDne.Value)
                {
                    if (akce.Value.Predmet == predmet && akce.Value.TypVyuky == typVyuky)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Zjistí, zda existuje studijní skupina, pro kterou nebyla ještě vytvořena rozvrhová akce (nezávisle na typu výuky)
        /// </summary>
        /// <param name="predmet"></param>
        /// <returns></returns>
        private bool MaPredmetSkupinuBezRozvrhoveAkce(Predmet predmet)
        {
            if (predmet.HodinPrednasek > 0 && MaTypVyukySkupinuBezRozvrhoveAkce(predmet, TypyVyuky.Přednáška, predmet.HodinPrednasek))
            {
                return true;
            }
            if (predmet.HodinSeminaru > 0 && MaTypVyukySkupinuBezRozvrhoveAkce(predmet, TypyVyuky.Seminář, predmet.HodinSeminaru))
            {
                return true;
            }
            if (predmet.HodinCviceni > 0 && MaTypVyukySkupinuBezRozvrhoveAkce(predmet, TypyVyuky.Cvičení, predmet.HodinCviceni))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Zjistí, zda existuje studijní skupina, pro kterou nebyla ještě vytvořena rozvrhová akce pro předmět a typ výuky
        /// </summary>
        /// <param name="predmet"></param>
        /// <param name="typ"></param>
        /// <param name="pocetHodin"></param>
        /// <returns></returns>
        private bool MaTypVyukySkupinuBezRozvrhoveAkce(Predmet predmet, TypyVyuky typ, int pocetHodin)
        {
            int pocetRozvrhovychAkci = 0;
            foreach (KeyValuePair<int, StudijniSkupina> skupina in predmet.StudijniSkupiny)
            {
                foreach (KeyValuePair<Dny, Dictionary<int, RozvrhovaAkce>> rozvrhDne in skupina.Value.Rozvrh)
                {
                    foreach (KeyValuePair<int, RozvrhovaAkce> akce in rozvrhDne.Value)
                    {
                        if (akce.Value.Predmet == predmet && akce.Value.TypVyuky == typ)
                        {
                            pocetRozvrhovychAkci++;
                        }
                    }
                }
            }
            return pocetRozvrhovychAkci / pocetHodin == predmet.StudijniSkupiny.Count ? false : true;
        }


        /// <summary>
        /// Zjistí, zda student nemá  v daný den a daný čas (začátek až začátek plus délka) jinou výuku
        /// </summary>
        /// <param name="skupina"></param>
        /// <returns></returns>
        private bool MaSkupinaZaneprazdnenehoStudenta(Dictionary<int, Student> skupina)
        {
            foreach (KeyValuePair<int, Student> student in skupina)
            {
                foreach (KeyValuePair<int, RozvrhovaAkce> rozvrhDne in student.Value.Rozvrh[den])
                {
                    for (int i = 0; i < delka; i++)
                    {
                        if (rozvrhDne.Key == zacatek + i) // rozvrhDne.Key je hodina začátku RA
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        // gettery privátních fieldů (prvky formuláře)

        //public int GetId() { return id; }
        public Predmet GetPredmet() { return predmet; }
        public TypyVyuky GetTypVyuky() { return typVyuky; }
        public Vyucujici GetVyucujici() { return vyucujici; }
        public Mistnost GetMistnost() { return mistnost; }
        public Dny GetDen() { return den; }
        public int GetZacatek() { return zacatek; }
        public int GetDelka() { return delka; }
        public Dictionary<int, StudijniSkupina> GetStudijniSkupiny() { return studijniSkupiny; }

        public bool GetExit() { return exit; }

    }
}