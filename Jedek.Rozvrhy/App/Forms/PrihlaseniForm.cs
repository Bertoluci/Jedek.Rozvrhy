using System;
using System.Text.RegularExpressions;

namespace Jedek.Rozvrhy.App.Forms
{
    class PrihlaseniForm : Form
    {
        private string uzivatelskeJmeno = string.Empty;
        private string heslo = string.Empty;

        /// <summary>
        /// Chyba přihlášení
        /// </summary>
        public bool Error { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chybaPrihlaseni"></param>
        public PrihlaseniForm(bool chybaPrihlaseni)
        {
            Error = chybaPrihlaseni;
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            bool ok = false;
            do // uživatelské jméno
            {
                PrintHeader();
                Console.Write("\r\n\t\tUživatelské jméno: ");
                uzivatelskeJmeno = Console.ReadLine();
                ok = Regex.IsMatch(uzivatelskeJmeno, @"^[a-z]_[a-z0-9]+$");
                if (!ok)
                {
                    uzivatelskeJmeno = String.Empty;
                }
            } while (!ok);


            ok = false;
            do // heslo
            {
                PrintHeader();
                Console.Write("\t\tHeslo: ");
                heslo = Console.ReadLine();
                heslo = heslo.Trim();
            } while (heslo.Length < 5);
        }


        /// <summary>
        /// Vykreslí hlavičku formuláře
        /// </summary>
        private void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine();
            if (Error)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\tPŘIHLÁŠENÍ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\tZadali jste chybné přihlašovací údaje.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\n\tVyplňte přihlašovací údaje znovu:");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\tPŘIHLÁŠENÍ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\n\tVyplňte přihlašovací údaje:");
            }
            Console.WriteLine();

            // uživatelské jméno
            if (uzivatelskeJmeno != String.Empty)
            {
                Console.WriteLine("\tUživatelské jméno: {0}", uzivatelskeJmeno);
                Console.WriteLine();
            }
        }

        // gettery privátních fieldů (prvky formuláře)
        public string GetUzivatelskeJmeno() { return uzivatelskeJmeno; }
        public string GetHeslo() { return heslo; }

    }
}
