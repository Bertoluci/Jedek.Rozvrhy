using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class StudijniObor : Model
    {
        public int Id { get; private set; }
        public string Nazev { get;  set; }
        public string Zkratka { get;  set; }
        public Dictionary<int, Predmet> PredmetyOboru { get; set; }


        public StudijniObor(int id, string nazev, string zkratka)
        {
            Id = id;
            Nazev = nazev;
            Zkratka = zkratka;
            PredmetyOboru = new Dictionary<int, Predmet>();
        }


        public override string ToString()
        {
            return String.Format("{0} [{1}]", Nazev, Zkratka);
        }

    }

}

