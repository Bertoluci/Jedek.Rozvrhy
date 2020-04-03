using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class StudijniSkupina : Model
    {
        public int Id { get; private set; }
        public Predmet Predmet { get; set; }
        public Dictionary<int, Student> StudentiSkupiny { get; set; }

        // <den <hodina, rozvrhová akce>>
        public Dictionary<Dny, Dictionary<int, RozvrhovaAkce>> Rozvrh { get; set; }

        public StudijniSkupina(int id, Predmet predmet, Dictionary<int, Student> studentiSkupiny)
        {
            Id = id;
            Predmet = predmet;
            StudentiSkupiny = studentiSkupiny;
            Rozvrh = new Dictionary<Dny, Dictionary<int, RozvrhovaAkce>>();
            Rozvrh.Add(Dny.Po, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Út, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.St, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Čt, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Pá, new Dictionary<int, RozvrhovaAkce>());
        }


        public override string ToString()
        {
            return String.Format("ID:{0}  předmět:{1}  počet studentů:{2}", Id, Predmet.Zkratka, StudentiSkupiny.Count);
        }

    }
}
