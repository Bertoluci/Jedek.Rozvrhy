using System;
using System.Collections.Generic;
using Jedek.Rozvrhy.Libs;

namespace Jedek.Rozvrhy.App.Models
{
    enum TypyVyuky
    {
        Přednáška = 1, Seminář, Cvičení
    }

    enum Dny
    {
        Po = 1, Út, St, Čt, Pá
    }

    class RozvrhovaAkce : Model
    {
        public int Id { get; private set; }
        public Predmet Predmet { get; set; }
        public TypyVyuky TypVyuky { get; set; }
        public Vyucujici Vyucujici { get; set; }
        public Mistnost Mistnost { get; private set; }
        public Dny Den { get; set; }
        public int Zacatek { get; private set; }
        public int Delka { get; private set; }
        public Dictionary<int, StudijniSkupina> StudijniSkupiny { get; private set; }


        public RozvrhovaAkce(int id, Predmet predmet, TypyVyuky typVyuky, Vyucujici vyucujici, Mistnost mistnost, Dny den, int zacatek, int delka )
        {
            Id = id;
            Predmet = predmet;
            TypVyuky = typVyuky;
            Vyucujici = vyucujici;
            Den = den;
            Mistnost = mistnost;
            Zacatek = zacatek;
            Delka = delka;
            StudijniSkupiny = new Dictionary<int, StudijniSkupina>();
        }


        public override string ToString()
        {
            return String.Format("ID:{0}  předmět:{1}  místnost:[U{2}/{3}]", Id, Predmet.Zkratka, Mistnost.Budova, Mistnost.Cislo);
        }
     
    }
}
