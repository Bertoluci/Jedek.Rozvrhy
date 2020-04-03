using System;

namespace Jedek.Rozvrhy.App.Models
{
    class SeminarniMistnost : Mistnost
    {
        public SeminarniMistnost(int id, int budova, int cislo, int kapacita)
            : base(id, budova, cislo, kapacita)
        {
            // specifika
        }

        public override string ToString()
        {
            return String.Format("[U{0}/{1}], kapacita: {2}, typ: Seminární", Budova, Cislo, Kapacita);
        } 
    }
}
