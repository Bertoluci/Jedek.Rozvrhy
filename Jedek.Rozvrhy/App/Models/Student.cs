using System;
using System.Collections.Generic;

namespace Jedek.Rozvrhy.App.Models
{
    class Student : Osoba
    {
        public StudijniObor StudijniObor { get; set; }
        public Dictionary<int, Predmet> ZapsanePredmety { get; set; }
        public Dictionary<int, StudijniSkupina> StudijniSkupiny { get; set; }
        public int Rocnik { get; set; }

        // <den <hodina, rozvrhová akce>>
        public Dictionary<Dny, Dictionary<int, RozvrhovaAkce>> Rozvrh { get; set; }


        public Student(int id, string jmeno, string prijmeni, int rocnik, StudijniObor studijniObor, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
            : base(id, jmeno, prijmeni, osobniCislo, uzivatelskeJmeno, heslo, role)
        {
            Rocnik = rocnik;
            StudijniObor = studijniObor;
            ZapsanePredmety = new Dictionary<int, Predmet>();
            StudijniSkupiny = new Dictionary<int, StudijniSkupina>();

            Rozvrh = new Dictionary<Dny, Dictionary<int, RozvrhovaAkce>>();
            Rozvrh.Add(Dny.Po, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Út, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.St, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Čt, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Pá, new Dictionary<int, RozvrhovaAkce>());
        }


        public override string ToString()
        {
            return String.Format("{0}, [{1}], {2}.ročník", base.ToString(), OsobniCislo, Rocnik);
        }

    }
}