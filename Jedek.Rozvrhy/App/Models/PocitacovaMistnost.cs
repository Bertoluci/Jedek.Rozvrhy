using System;

namespace Jedek.Rozvrhy.App.Models
{
    class PocitacovaMistnost : Mistnost
    {
        public int PocetPC { get; private set; }

        public PocitacovaMistnost(int id, int budova, int cislo, int kapacita)
            : base(id, budova, cislo, kapacita)
        {
        }
        
        public PocitacovaMistnost(int id, int budova, int cislo, int kapacita, int pocetPC)
            : this (id, budova, cislo, kapacita)
        {
            PocetPC = pocetPC;
        }


        public override string ToString()
        {
            return String.Format("[U{0}/{1}], kapacita: {2}, typ: Učebna PC", Budova, Cislo, Kapacita);
        }
    }
}
