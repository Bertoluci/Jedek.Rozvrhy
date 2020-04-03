using System;
using Jedek.Rozvrhy.App.Models;

namespace Jedek.Rozvrhy.App.Forms
{
    class MistnostForm : Form
    {
        // U property nelze nastavotat přes out v int.Parse
        //private int id = 0;
        private TypMistnosti typ; // Posluchárna, Seminární, Učebna PC
        private int kapacita;
        private int budova;
        private int cislo;


        /// <summary>
        /// Constructor
        /// </summary>
        public MistnostForm()
        {
            ShowForm();
        }


        /// <summary>
        /// Vykreslení formuláře
        /// </summary>
        private void ShowForm()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\n\tVytvoření nové místnosti:");

            string input = String.Empty;
            do // kapacita
            {
                Console.Write("\r\n\t\tKapacita místnosti [12 - 200]: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out kapacita) || !(kapacita > 11 && kapacita < 201));


            do // budova
            {
                Console.Write("\t\tČíslo budovy: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out budova));


            do // cislo
            {
                Console.Write("\t\tČíslo místnosti: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out cislo));


            do // typ
            {
                Console.Write("\t\tTyp místnosti (Posluchárna , Seminární , Počítačová): ");
                input = Console.ReadLine();
            } while (!Enum.IsDefined(typeof(TypMistnosti), input));
            bool parse = Enum.TryParse(input, out typ);
        }


        // gettery privátních fieldů (prvky formuláře)

        //public int GetId() { return id; }
        public int GetKapacita() { return kapacita; }
        public int GetBudova() { return budova; }
        public int GetCislo() { return cislo; }
        public TypMistnosti GetTypMistnosti() { return typ; }

    }
}
