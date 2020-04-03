using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    enum TypMistnosti : byte
    {
        Posluchárna = 1, Seminární, Počítačová
    }


    abstract class Mistnost : Model
    {
        public int Id { get; private set; }
        public int Kapacita { get;  set; }
        public int Budova { get;  set; }
        public int Cislo { get;  set; }

        // <den <hodina, rozvrhová akce>>
        public Dictionary<Dny, Dictionary<int, RozvrhovaAkce>> Rozvrh { get; set; }


        public Mistnost(int id, int budova, int cislo, int kapacita)
        {
            Id = id;
            Budova = budova;
            Cislo = cislo;
            Kapacita = kapacita;
            Rozvrh = new Dictionary<Dny, Dictionary<int, RozvrhovaAkce>>();
            Rozvrh.Add(Dny.Po, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Út, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.St, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Čt, new Dictionary<int, RozvrhovaAkce>());
            Rozvrh.Add(Dny.Pá, new Dictionary<int, RozvrhovaAkce>());
        }
    }
}