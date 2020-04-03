using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    class Predmet : Model, IComparable<Predmet>
    {

        public int Id { get; private set; }
        public string Zkratka { get; set; }
        public string Nazev { get; set; }
        public int HodinPrednasek { get; set; }
        public int HodinCviceni { get; set; }
        public int HodinSeminaru { get; set; }
        public Dictionary<int, Vyucujici> Cvicici { get; set; }
        public Dictionary<int, Vyucujici> Prednasejici { get; set; }
        public Dictionary<int, Vyucujici> VedeSeminar { get; set; }
        public Dictionary<int, Student> Studujici { get; set; }
        public Dictionary<int, StudijniSkupina> StudijniSkupiny { get; set; }
        public Dictionary<int, RozvrhovaAkce> RozvrhoveAkce { get; set; }


        public Predmet(int id, string zkratka, string nazev, int hodinPrednasek, int hodinCviceni, int hodinSeminaru)
        {
            Id = id;
            Zkratka = zkratka;
            Nazev = nazev;
            HodinPrednasek = hodinPrednasek;
            HodinCviceni = hodinCviceni;
            HodinSeminaru = hodinSeminaru;
            Cvicici = new Dictionary<int, Vyucujici>();
            Prednasejici = new Dictionary<int, Vyucujici>();
            VedeSeminar = new Dictionary<int, Vyucujici>();
            Studujici = new Dictionary<int, Student>();
            StudijniSkupiny = new Dictionary<int, StudijniSkupina>();
            RozvrhoveAkce = new Dictionary<int, RozvrhovaAkce>();
        }


        public override string ToString()
        {
            return String.Format("[{0}], {1}, [Pr:{2}, Cv:{3}, Se:{4}]", Zkratka, Nazev, HodinPrednasek, HodinCviceni, HodinSeminaru);
        }


        public int CompareTo(Predmet other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
}