using System;
using System.Collections.Generic;
using System.Linq;
using Jedek.Rozvrhy.Libs;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Views.Uzivatel
{
    class Default : View
    {

        public Default(Dictionary<string, Object> context)
            : base(context)
        {
        }

        public override void Render()
        {
            PrintPredmety();
            PrintRozvrh();
            PrintSeznam();
            Console.WriteLine("\r\n\t\tPro návrat stiskněte libovolnou klávesu.");
            Console.ReadKey();
        }


        private void PrintPredmety()
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (Uzivatel.Role == Role.student)
            {
                Console.WriteLine("\r\n\t\tZAPSANÉ PŘEDMĚTY:");
            }
            else
            {
                Console.WriteLine("\r\n\t\tVYUČOVANÉ PŘEDMĚTY:");
            }
            Console.ForegroundColor = ConsoleColor.Black;
            if (Uzivatel.Role == Role.student)
            {
                Models.Student student = (Models.Student)this.Context["student"];
                Console.WriteLine();
                // dva sloupce
                bool odd = true;
                foreach (var p in student.ZapsanePredmety)
                {
                    if (odd)
                    {
                        Console.Write("\t\t\t{0, -5}\t{1, -30}", p.Value.Zkratka, p.Value.Nazev);
                        odd = false;
                    }
                    else
                    {
                        Console.WriteLine("\t\t{0, -5}\t{1, -30}", p.Value.Zkratka, p.Value.Nazev);
                        odd = true;
                    }
                }
                // nedošlo k odřádkování
                if(odd == false)
                {
                    Console.WriteLine();
                }
            }
            else
            {
                Models.Vyucujici vyucujici = (Models.Vyucujici)this.Context["vyucujici"];
                List<string> prednasky = new List<string>();
                foreach (var predmet in vyucujici.Prednasky)
                {
                    prednasky.Add(predmet.Value.Nazev);
                }
                List<string> cviceni = new List<string>();
                foreach (var predmet in vyucujici.Cviceni)
                {
                    cviceni.Add(predmet.Value.Nazev);
                }
                List<string> seminare = new List<string>();
                foreach (var predmet in vyucujici.Seminare)
                {
                    seminare.Add(predmet.Value.Nazev);
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t\t\tPřednášky: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("{0}", String.Join(", ", prednasky));

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t\t\t  Cvičení: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("{0}", String.Join(", ", cviceni));

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t\t\t Semináře: ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("{0}", String.Join(", ", seminare));
            }
        }


        private void PrintRozvrh()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\r\n\t\tGRAFICKÝ ROZVRH\r\n");
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine("\t{0,-9} {1,-9} {2,-9} {3,-9} {4,-9} {5,-9} {6,-9} {7,-9} {8,-9} {9,-9} {10,-9} {11,-9} {12,-9} {13,-9} {14,-9} {15,-9} "
                , "", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00");

            Console.WriteLine("\t{0,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} ", "", "- - - - -");

            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , Dny.Po, GetPredmet(Dny.Po, 7), GetPredmet(Dny.Po, 8), GetPredmet(Dny.Po, 9), GetPredmet(Dny.Po, 10), GetPredmet(Dny.Po, 11)
                , GetPredmet(Dny.Po, 12), GetPredmet(Dny.Po, 13), GetPredmet(Dny.Po, 14), GetPredmet(Dny.Po, 15), GetPredmet(Dny.Po, 16)
                , GetPredmet(Dny.Po, 17), GetPredmet(Dny.Po, 18), GetPredmet(Dny.Po, 19), GetPredmet(Dny.Po, 20), GetPredmet(Dny.Po, 21));
            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , " ", GetMistnost(Dny.Po, 7), GetMistnost(Dny.Po, 8), GetMistnost(Dny.Po, 9), GetMistnost(Dny.Po, 10), GetMistnost(Dny.Po, 11)
                , GetMistnost(Dny.Po, 12), GetMistnost(Dny.Po, 13), GetMistnost(Dny.Po, 14), GetMistnost(Dny.Po, 15), GetMistnost(Dny.Po, 16)
                , GetMistnost(Dny.Po, 17), GetMistnost(Dny.Po, 18), GetMistnost(Dny.Po, 19), GetMistnost(Dny.Po, 20), GetMistnost(Dny.Po, 21));

            Console.WriteLine("\t{0,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} ", "", "- - - - -");

            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , Dny.Út, GetPredmet(Dny.Út, 7), GetPredmet(Dny.Út, 8), GetPredmet(Dny.Út, 9), GetPredmet(Dny.Út, 10), GetPredmet(Dny.Út, 11)
                , GetPredmet(Dny.Út, 12), GetPredmet(Dny.Út, 13), GetPredmet(Dny.Út, 14), GetPredmet(Dny.Út, 15), GetPredmet(Dny.Út, 16)
                , GetPredmet(Dny.Út, 17), GetPredmet(Dny.Út, 18), GetPredmet(Dny.Út, 19), GetPredmet(Dny.Út, 20), GetPredmet(Dny.Út, 21));
            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , " ", GetMistnost(Dny.Út, 7), GetMistnost(Dny.Út, 8), GetMistnost(Dny.Út, 9), GetMistnost(Dny.Út, 10), GetMistnost(Dny.Út, 11)
                , GetMistnost(Dny.Út, 12), GetMistnost(Dny.Út, 13), GetMistnost(Dny.Út, 14), GetMistnost(Dny.Út, 15), GetMistnost(Dny.Út, 16)
                , GetMistnost(Dny.Út, 17), GetMistnost(Dny.Út, 18), GetMistnost(Dny.Út, 19), GetMistnost(Dny.Út, 20), GetMistnost(Dny.Út, 21));

            Console.WriteLine("\t{0,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} ", "", "- - - - -");

            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , Dny.St, GetPredmet(Dny.St, 7), GetPredmet(Dny.St, 8), GetPredmet(Dny.St, 9), GetPredmet(Dny.St, 10), GetPredmet(Dny.St, 11)
                , GetPredmet(Dny.St, 12), GetPredmet(Dny.St, 13), GetPredmet(Dny.St, 14), GetPredmet(Dny.St, 15), GetPredmet(Dny.St, 16)
                , GetPredmet(Dny.St, 17), GetPredmet(Dny.St, 18), GetPredmet(Dny.St, 19), GetPredmet(Dny.St, 20), GetPredmet(Dny.St, 21));
            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , " ", GetMistnost(Dny.St, 7), GetMistnost(Dny.St, 8), GetMistnost(Dny.St, 9), GetMistnost(Dny.St, 10), GetMistnost(Dny.St, 11)
                , GetMistnost(Dny.St, 12), GetMistnost(Dny.St, 13), GetMistnost(Dny.St, 14), GetMistnost(Dny.St, 15), GetMistnost(Dny.St, 16)
                , GetMistnost(Dny.St, 17), GetMistnost(Dny.St, 18), GetMistnost(Dny.St, 19), GetMistnost(Dny.St, 20), GetMistnost(Dny.St, 21));

            Console.WriteLine("\t{0,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} ", "", "- - - - -");

            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , Dny.Čt, GetPredmet(Dny.Čt, 7), GetPredmet(Dny.Čt, 8), GetPredmet(Dny.Čt, 9), GetPredmet(Dny.Čt, 10), GetPredmet(Dny.Čt, 11)
                , GetPredmet(Dny.Čt, 12), GetPredmet(Dny.Čt, 13), GetPredmet(Dny.Čt, 14), GetPredmet(Dny.Čt, 15), GetPredmet(Dny.Čt, 16)
                , GetPredmet(Dny.Čt, 17), GetPredmet(Dny.Čt, 18), GetPredmet(Dny.Čt, 19), GetPredmet(Dny.Čt, 20), GetPredmet(Dny.Čt, 21));
            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , " ", GetMistnost(Dny.Čt, 7), GetMistnost(Dny.Čt, 8), GetMistnost(Dny.Čt, 9), GetMistnost(Dny.Čt, 10), GetMistnost(Dny.Čt, 11)
                , GetMistnost(Dny.Čt, 12), GetMistnost(Dny.Čt, 13), GetMistnost(Dny.Čt, 14), GetMistnost(Dny.Čt, 15), GetMistnost(Dny.Čt, 16)
                , GetMistnost(Dny.Čt, 17), GetMistnost(Dny.Čt, 18), GetMistnost(Dny.Čt, 19), GetMistnost(Dny.Čt, 20), GetMistnost(Dny.Čt, 21));

            Console.WriteLine("\t{0,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} ", "", "- - - - -");

            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , Dny.Pá, GetPredmet(Dny.Pá, 7), GetPredmet(Dny.Pá, 8), GetPredmet(Dny.Pá, 9), GetPredmet(Dny.Pá, 10), GetPredmet(Dny.Pá, 11)
                , GetPredmet(Dny.Pá, 12), GetPredmet(Dny.Pá, 13), GetPredmet(Dny.Pá, 14), GetPredmet(Dny.Pá, 15), GetPredmet(Dny.Pá, 16)
                , GetPredmet(Dny.Pá, 17), GetPredmet(Dny.Pá, 18), GetPredmet(Dny.Pá, 19), GetPredmet(Dny.Pá, 20), GetPredmet(Dny.Pá, 21));
            Console.WriteLine("\t{0,-9}:{1,-9}:{2,-9}:{3,-9}:{4,-9}:{5,-9}:{6,-9}:{7,-9}:{8,-9}:{9,-9}:{10,-9}:{11,-9}:{12,-9}:{13,-9}:{14,-9}:{15,-9}:"
                , " ", GetMistnost(Dny.Pá, 7), GetMistnost(Dny.Pá, 8), GetMistnost(Dny.Pá, 9), GetMistnost(Dny.Pá, 10), GetMistnost(Dny.Pá, 11)
                , GetMistnost(Dny.Pá, 12), GetMistnost(Dny.Pá, 13), GetMistnost(Dny.Pá, 14), GetMistnost(Dny.Pá, 15), GetMistnost(Dny.Pá, 16)
                , GetMistnost(Dny.Pá, 17), GetMistnost(Dny.Pá, 18), GetMistnost(Dny.Pá, 19), GetMistnost(Dny.Pá, 20), GetMistnost(Dny.Pá, 21));

            Console.WriteLine("\t{0,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} {1,-9} ", "", "- - - - -");
        }


        private string GetPredmet(Dny den, int hodina)
        {
            Dictionary<Dny, Dictionary<int, Models.RozvrhovaAkce>> rozvrh = (Dictionary<Dny, Dictionary<int, Models.RozvrhovaAkce>>)Context["rozvrh"];

            if (Uzivatel.Role == Role.student)
            {
                // student
                string predmet = rozvrh[den].ContainsKey(hodina) ? "  " + rozvrh[den][hodina].Predmet.Zkratka : "";
                return predmet;
            }
            else
            {
                // vyucujici
                string predmet = rozvrh[den].ContainsKey(hodina) ? "  " + rozvrh[den][hodina].Predmet.Zkratka : "";
                return predmet;
            }
        }


        private string GetMistnost(Dny den, int hodina)
        {
            Dictionary<Dny, Dictionary<int, Models.RozvrhovaAkce>> rozvrh = (Dictionary<Dny, Dictionary<int, Models.RozvrhovaAkce>>)Context["rozvrh"];

            if (Uzivatel.Role == Role.student)
            {
                // student
                string mistnost = rozvrh[den].ContainsKey(hodina) ? String.Format(" U{0}/{1}", rozvrh[den][hodina].Mistnost.Budova, rozvrh[den][hodina].Mistnost.Cislo) : "";
                return mistnost;
            }
            else
            {
                // vyucujici
                string mistnost = rozvrh[den].ContainsKey(hodina) ? String.Format(" U{0}/{1}", rozvrh[den][hodina].Mistnost.Budova, rozvrh[den][hodina].Mistnost.Cislo) : "";
                return mistnost;
            }

        }

        private void PrintSeznam()
        {
            Dictionary<Dny, Dictionary<int, Models.RozvrhovaAkce>> rozvrh = (Dictionary<Dny, Dictionary<int, Models.RozvrhovaAkce>>)Context["rozvrh"];

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\r\n\r\n\t\tTABULKOVÝ ROZVRH");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;
            foreach (var den in rozvrh)
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
                        Console.WriteLine("\t\t\t{0,-8}{1,-10} {2,-25} {3,-10} {4,-12} {5,-20}" //[U{0}/{1}]
                                        , akce.Value.Den + " " + akce.Value.Zacatek + ":00"
                                        , " - " + (akce.Value.Zacatek + akce.Value.Delka) + ":00"
                                        , akce.Value.Predmet.Nazev
                                        , akce.Value.TypVyuky
                                        , "[U" + akce.Value.Mistnost.Budova + "/" + akce.Value.Mistnost.Cislo + "]"
                                        , Uzivatel.Role == Role.student ? akce.Value.Vyucujici.Prijmeni + " " + akce.Value.Vyucujici.Jmeno : String.Empty);

                        akceId = akce.Value.Id;
                    }

                }
            }
        }
    }


}