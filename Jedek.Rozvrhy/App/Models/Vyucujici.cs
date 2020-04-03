using System;
using System.Collections.Generic;

namespace Jedek.Rozvrhy.App.Models
{
    class Vyucujici : Osoba
    {
        public Dictionary<int, Predmet> Prednasky { get; private set; }
        public Dictionary<int, Predmet> Cviceni { get; private set; }
        public Dictionary<int, Predmet> Seminare { get; private set; }
        public string Tituly { get; set; }

        // <den <hodina, rozvrhová akce>>
        public Dictionary<Dny, Dictionary<int, RozvrhovaAkce>> Rozvrh { get; set; }

        public Vyucujici(int id, string jmeno, string prijmeni, string tituly, string osobniCislo, string uzivatelskeJmeno, string heslo, string role)
            : base(id, jmeno, prijmeni, osobniCislo, uzivatelskeJmeno, heslo, role)
        {
            Tituly = tituly;
            Prednasky = new Dictionary<int, Predmet>();
            Cviceni = new Dictionary<int, Predmet>();
            Seminare = new Dictionary<int, Predmet>();
            Rozvrh = new Dictionary<Dny, Dictionary<int, RozvrhovaAkce>>();
            Rozvrh.Add(Dny.Po, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Út, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.St, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Čt, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Pá, new Dictionary<int, RozvrhovaAkce>());
        }


        public override string ToString()
        {
            return String.Format("{0}{1}", base.ToString(), Tituly == String.Empty ? "" : ", " + Tituly);
        }
    }
}